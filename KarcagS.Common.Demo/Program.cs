using KarcagS.Common.Middlewares;
using KarcagS.Common.Tools;
using KarcagS.Common.Tools.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UtilsSettings>(builder.Configuration.GetSection("Utils"));

// Add services to the container.

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUtilsService, UtilsService<DbContext>>();
builder.Services.AddScoped<ILoggerService, LoggerService>();

builder.Services.AddDbContext<DbContext>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpInterceptor();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
