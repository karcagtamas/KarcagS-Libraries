using System.Linq.Expressions;
using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Enums;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using Neo4jClient.Cypher;

namespace KarcagS.API.Table.AutoListTable;

public class Neo4jListTableDataSource<T, TKey> : AutoListTableDataSource<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly Func<QueryModel, Task<ICypherFluentQuery>> Fetcher;
    protected readonly Expression<Func<ICypherFluentQuery, ICypherFluentQuery<T>>> ReturnExpression;
    protected readonly Expression<Func<ICypherFluentQuery, ICypherFluentQuery<long>>> CountExpression;
    protected readonly string AliasPrefix;

    private Neo4jListTableDataSource(
        Func<QueryModel, Task<ICypherFluentQuery>> fetcher,
        Expression<Func<ICypherFluentQuery, ICypherFluentQuery<T>>> returnExpression,
        Expression<Func<ICypherFluentQuery, ICypherFluentQuery<long>>> countExpression,
        string aliasPrefix
    )
    {
        Fetcher = fetcher;
        ReturnExpression = returnExpression;
        CountExpression = countExpression;
        AliasPrefix = aliasPrefix;
    }

    public static Neo4jListTableDataSource<T, TKey> Build(Func<QueryModel, Task<ICypherFluentQuery>> fetcher, string aliasPrefix = "e")
    {
        return Build(fetcher, x => x.Return<T>(e => e.As<T>()), aliasPrefix);
    }

    public static Neo4jListTableDataSource<T, TKey> Build(Func<QueryModel, Task<ICypherFluentQuery>> fetcher, Expression<Func<ICypherFluentQuery, ICypherFluentQuery<T>>> returnExpression, string aliasPrefix = "e")
    {
        return Build(fetcher, returnExpression, x => x.Return(e => e.Count()), aliasPrefix);
    }

    public static Neo4jListTableDataSource<T, TKey> Build(Func<QueryModel, Task<ICypherFluentQuery>> fetcher, Expression<Func<ICypherFluentQuery, ICypherFluentQuery<T>>> returnExpression,
        Expression<Func<ICypherFluentQuery, ICypherFluentQuery<long>>> countExpression, string aliasPrefix = "e")
    {
        return new(fetcher, returnExpression, countExpression, aliasPrefix);
    }

    public override async Task<long> LoadAllDataCountAsync(QueryModel query)
    {
        var baseQuery = await Fetcher(query);
        return (await CountExpression.Compile()(baseQuery).ResultsAsync).Single();
    }

    public override async Task<long> LoadFilteredAllDataCountAsync(QueryModel query, Configuration<T, TKey> configuration)
    {
        var baseQuery = await Fetcher(query);
        baseQuery = ApplyCypherFiltering(query, configuration, baseQuery);
        return (await CountExpression.Compile()(baseQuery).ResultsAsync).Single();
    }

    public override async Task<IEnumerable<T>> LoadDataAsync(QueryModel query, Configuration<T, TKey> configuration)
    {
        var queryBuilder = await Fetcher(query);
        queryBuilder = ApplyCypherFiltering(query, configuration, queryBuilder);

        var ordering = query.IsOrderingNeeded()
            ? ConstructOrderingSettingsFromModel(query, configuration.Columns)
            : DefaultOrdering;
        var orderClauses = new List<string>();

        if (ObjectHelper.IsEmpty(ordering))
        {
            orderClauses.Add($"{AliasPrefix}.Id ASC");
        }
        else
        {
            foreach (var setting in ordering)
            {
                var dir = setting.Direction == OrderDirection.Descend ? "DESC" : "ASC";
                orderClauses.Add($"{ExtractCypherPropertyPath(setting.Exp)} {dir}");
            }

            if (IsIdOrderingFallbackRequired())
            {
                orderClauses.Add($"{AliasPrefix}.Id ASC");
            }
        }

        queryBuilder = queryBuilder.OrderBy(string.Join(", ", orderClauses));

        if (ObjectHelper.IsNotNull(query.Size) && ObjectHelper.IsNotNull(query.Page))
        {
            queryBuilder = queryBuilder.Skip(query.Size.Value * query.Page.Value).Limit(query.Size.Value);
        }

        return await ReturnExpression.Compile()(queryBuilder).ResultsAsync;
    }

    private ICypherFluentQuery ApplyCypherFiltering(QueryModel query, Configuration<T, TKey> configuration, ICypherFluentQuery queryBuilder)
    {
        ObjectHelper.WhenNotNull(query.TextFilter, filter =>
        {
            if (ObjectHelper.IsEmpty(TextFilteredColumns)) return;

            var activeColumns = configuration.Columns
                .Where(col => TextFilteredColumns.Contains(col.Key) && !string.IsNullOrWhiteSpace(col.OrderBy))
                .ToList();

            if (activeColumns.Count == 0) return;

            var cypherExpressions =
                activeColumns.Select(col => $"{AliasPrefix}.{string.Join(".", col.OrderBy.Split('.'))} =~ $regexFilter");
            queryBuilder = (ICypherFluentQuery<T>)queryBuilder
                .Where(string.Join(" OR ", cypherExpressions))
                .WithParam("regexFilter", $"(?i).*{filter}.*");
        });

        return queryBuilder;
    }

    private string ExtractCypherPropertyPath(Expression<Func<T, object?>> expression)
    {
        Expression? body = expression.Body;
        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unary) body = unary.Operand;

        var parts = new List<string>();
        while (body is MemberExpression member)
        {
            parts.Add(member.Member.Name);
            body = member.Expression;
        }

        if (parts.Count == 0) return $"{AliasPrefix}.Id";
        parts.Reverse();
        return $"{AliasPrefix}.{string.Join(".", parts)}";
    }
}