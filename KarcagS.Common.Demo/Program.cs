using KarcagS.Common.Demo;
using KarcagS.Common.Tools;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddHttpLogging(opt => { });

builder.Services.Configure<UtilsSettings>(builder.Configuration.GetSection("Utils"));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Policy", cb =>
    {
        cb.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("https://localhost:7257", "http://localhost:5257");
    });
});

// Add services to the container.

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUtilsService<string>, UtilsService<DbContext, string>>();
builder.Services.AddScoped<ILoggerService, LoggerService<string>>();
builder.Services.AddScoped<IDemoService, DemoService>();

builder.Services.AddDbContext<DbContext>();

builder.Services.AddErrorConverter(conf => { });

builder.Services.AddModelValidatedControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpInterceptor((opt) =>
{
    opt.OnlyApi = true;
});

app.UseHttpsRedirection();

app.UseCors("Policy");

// app.UseAuthorization();

app.MapControllers();

app.UseHttpLogging();

app.Run();
