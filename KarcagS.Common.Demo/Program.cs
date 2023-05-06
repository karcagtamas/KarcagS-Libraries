using AutoMapper;
using KarcagS.API.Http.Interceptor;
using KarcagS.API.Http.Interceptor.Converters;
using KarcagS.API.Repository;
using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Services;
using KarcagS.API.Table;
using KarcagS.Common.Demo;
using KarcagS.Common.Demo.Mappers;
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
builder.Services.AddScoped<IUtilsService<string>, UtilsService<DemoContext, string>>();
builder.Services.AddScoped<IUserProvider<string>, UserProvider>();
builder.Services.AddScoped<ILoggerService, LoggerService<string>>();
builder.Services.AddScoped<IDemoService, DemoService>();
builder.Services.AddScoped<IGenderService, GenderService>();

// Add AutoMapper
var mapperConfig = new MapperConfiguration(conf => { conf.AddProfile<GenderMapper>(); });
builder.Services.AddSingleton(mapperConfig.CreateMapper());

var connString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContextPool<DemoContext>(opt =>
    opt.UseLazyLoadingProxies()
        .UseMySql(connString, ServerVersion.AutoDetect(connString), b => b.MigrationsAssembly("KarcagS.Common.Demo")));

builder.Services.AddErrorConverter(conf => { conf.AddAgent(new TableErrorConverterAgent()); });

builder.Services.AddModelValidatedControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpInterceptor((opt) => { opt.OnlyApi = true; });

app.UseHttpsRedirection();

app.UseCors("Policy");

// app.UseAuthorization();

app.MapControllers();

app.UseHttpLogging();

app.Run();