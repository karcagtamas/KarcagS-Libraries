using KarcagS.Common.Tools;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IUtilsService, UtilsService<DbContext>>();
builder.Services.AddScoped<ILoggerService, LoggerService>();

builder.Services.AddDbContext<DbContext>();

builder.Services.AddModelValidatedControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpInterceptor();

app.UseHttpsRedirection();

app.UseCors("Policy");

// app.UseAuthorization();

app.MapControllers();

app.Run();
