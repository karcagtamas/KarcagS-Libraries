using KarcagS.Common.Attributes;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Tools.Table;

public abstract class TableController<T, TKey> : ControllerBase where T : class, IIdentified<TKey>
{
    [HttpGet("Meta")]
    public TableMetaData GetMetaData() => GetService().GetTableMetaData();

    [HttpGet("Data")]
    [QueryModelExtraParamsActionFilter]
    public async Task<ActionResult<TableResult<TKey>>> GetData([FromQuery] QueryModel query)
    {
        try
        {
            return await GetService().GetData(query);
        }
        catch (TableNotAuthorizedException)
        {
            throw new ServerException("Not authorized.", "Server.Message.NotAuthorized");
        }
    }

    protected abstract ITableService<T, TKey> GetService();
}
