using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Tools.Controllers;

public interface IMapperController<TKey, in TModel>
{
    IActionResult Get(TKey id);
    IActionResult GetAll([FromQuery] string orderBy, [FromQuery] string direction);
    IActionResult Delete(TKey id);
    IActionResult Update(TKey id, TModel model);
    IActionResult Create(TModel model);
}
