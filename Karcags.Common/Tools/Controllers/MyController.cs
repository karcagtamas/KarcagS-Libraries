using Karcags.Common.Tools.Entities;
using Karcags.Common.Tools.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Karcags.Common.Tools.Controllers;

/// <summary>
/// My Controller
/// </summary>
/// <typeparam name="TEntity">Type of Entity</typeparam>
/// <typeparam name="TModel">Type of Model object</typeparam>
/// <typeparam name="TElement">Type of Return element</typeparam>
public class MyController<TEntity, TKey, TModel, TElement> : ControllerBase, IController<TKey, TModel>
    where TEntity : class, IEntity<TKey>
{
    private readonly IMapperRepository<TEntity, TKey> _service;

    /// <summary>
    /// Init
    /// </summary>
    /// <param name="service">Repository service</param>
    public MyController(IMapperRepository<TEntity, TKey> service)
    {
        this._service = service;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model">Object model</param>
    /// <returns>Ok state</returns>
    [HttpPost]
    public IActionResult Create([FromBody] TModel model)
    {
        this._service.CreateFromModel(model);
        return this.Ok();
    }

    /// <summary>
    /// Delete by Id
    /// </summary>
    /// <param name="id">Id of object</param>
    /// <returns>Ok state</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(TKey id)
    {
        this._service.DeleteById(id);
        return this.Ok();
    }

    /// <summary>
    /// Get by Id
    /// </summary>
    /// <param name="id">Id of object</param>
    /// <returns>Element in ok state</returns>
    [HttpGet("{id}")]
    public IActionResult Get(TKey id)
    {
        return this.Ok(this._service.GetMapped<TElement>(id));
    }

    /// <summary>
    /// Get all element
    /// </summary>
    /// <param name="orderBy">Order by</param>
    /// <param name="direction">Order direction</param>
    /// <returns>List of elements in ok state</returns>
    [HttpGet]
    public IActionResult GetAll([FromQuery] string orderBy, [FromQuery] string direction)
    {
        if (string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(direction))
        {
            return this.Ok(this._service.GetAllMapped<TElement>());
        }

        return this.Ok(this._service.GetAllMappedAsOrdered<TElement>(orderBy, direction));
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="id">Id of object</param>
    /// <param name="model">Model of object</param>
    /// <returns>Ok state</returns>
    [HttpPut("{id}")]
    public IActionResult Update(TKey id, TModel model)
    {
        this._service.UpdateByModel(id, model);
        return this.Ok();
    }
}
