using Microsoft.AspNetCore.Mvc;

namespace Karcags.Common.Tools.Controllers
{
    public interface IController<in TModel>
    {
        IActionResult Get(int id);
        IActionResult GetAll([FromQuery] string orderBy, [FromQuery] string direction);
        IActionResult Delete(int id);
        IActionResult Update(int id, TModel model);
        IActionResult Create(TModel model);
    }
}