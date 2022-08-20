using KarcagS.Shared.Common;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Tools.Table;

public abstract class TableController<T, TKey> : ControllerBase where T : class, IIdentified<TKey>
{
    [HttpGet("Meta")]
    public TableMetaData<T, TKey> GetMetaData() => GetService().GetTableMetaData();

    [HttpGet("Data")]
    public List<T> GetData()
    {
        return GetService().GetData();
    }

    public abstract ITableService<T, TKey> GetService();
}
