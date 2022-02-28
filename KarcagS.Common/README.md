# Guide

## Attributes

### MaxNumber

> Validation attribute for a maximum number value.

### MinNumber

> Validation attribute for a minimum number value.

### User

The `Repository` uses for automatic user id inject (**current user**).

## Helpers

### Write Helper

Contains general `String` converters.

### DateHelper

Contains general `String` converters (from **DateTime**).

## Middlewares

### Database Migration

Automatic migration executing.

For the using you need to add into the `Program.cs` start flow.

Example of using the extension method:

```c#
var app = builder.Build();

app.Migrate<DbContext>();
```

> The `DbContext` can be any Database Context class

### Exception Handler

An exception handler interceptor. Any success request will be wrapped into a `HttpResult` class. This object will contains the result, the status code or the error list.

For the using:

```c#
var app = builder.Build();

app.UseMiddleware<HttpInterceptor>();
```

or

```c#
var app = builder.Build();

app.UseHttpInterceptor();
```

> Important! For the using you have to add the `LoggerService` to the application. Further information below.

## Tools

### Controllers

Use `MapperController` for five general endpoint.

- Get entity by Id
- Get entitiy list
- Create from a model
- Update by model
- Delete by Id

For the using: inherit Controller from the `MapperController`.

> Important! Need at least one MapperRepository solution.

### Email

> Under construction.

### Export

> Under construction.

### Repository

Contain general solution for the DbContext access for entities and provide mapping solution also.

`Repository` is a solution for basic DbContext transaction. `MapperRepository` provides mapping possibility.

> Important! For the `MapperRepository` you need add `AutoMapper` to the application.

### Services

#### Utils

Provides utils functionality for a WebAPI application.

Get current user from HttpContext or its properties like userId, userEmail, userName.

For the using:

```c#
var builder = WebApplication.CreateBuilder();
builder.Services.Configure<UtilsSettings>(builder.Configuration.GetSection("Utils"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUtilsService, UtilsService<NoteWebContext>>();
```

> Important! You have to add the `HttpContextAccessor` to your application and add `UtilsSettings` as configuration.

#### Logger

Provides basic logging functionality.

For the using:

```c#
var builder = WebApplication.CreateBuilder();
builder.Services.Configure<UtilsSettings>(builder.Configuration.GetSection("Utils"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUtilsService, UtilsService<NoteWebContext>>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
```
