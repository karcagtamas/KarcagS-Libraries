using KarcagS.API.Shared.Attributes;
using KarcagS.API.Shared.Exceptions;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.API.Table;

public abstract class TableController<T, TKey> : ControllerBase where T : class, IIdentified<TKey>
{
    [HttpGet("Meta")]
    public Task<TableMetaData> GetMetaData() => GetService().GetTableMetaDataAsync();

    [HttpGet("Data")]
    [QueryModelExtraParamsActionFilter]
    public async Task<ActionResult<TableResult<TKey>>> GetData([FromQuery] QueryModel query)
    {
        try
        {
            return await GetService().GetDataAsync(query);
        }
        catch (TableNotAuthorizedException)
        {
            throw new ServerException("Not authorized.", "Server.Message.NotAuthorized");
        }
    }

    protected abstract ITableService<T, TKey> GetService();
}