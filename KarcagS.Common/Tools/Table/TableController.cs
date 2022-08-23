using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Tools.Table;

public abstract class TableController<T, TKey> : ControllerBase where T : class, IIdentified<TKey>
{
    [HttpGet("Meta")]
    public TableMetaData GetMetaData() => GetService().GetTableMetaData();

    [HttpGet("Data")]
    public TableResult<TKey> GetData([FromQuery] QueryModel query) => GetService().GetData(query);

    public abstract ITableService<T, TKey> GetService();
}
