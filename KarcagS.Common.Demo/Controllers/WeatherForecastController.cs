using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KarcagS.Common.Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpPost]
    public TestModel Test([FromBody] TestModel model)
    {
        return model;
    }

    [HttpGet("Test")]
    public void Test()
    {
        throw new Exception("Error");
    }

    public class TestModel
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public DateTime? DateTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
    }
}