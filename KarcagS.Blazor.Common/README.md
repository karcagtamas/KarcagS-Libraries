# KarcagS.Blazor.Common

> Common Blazor Library

## HTTP

### Install

1. Register `HelperService`
2. Register `AuthHelperService`
3. Register `DialogHelperService`
4. Register `HttpService`
   - Add Service as Scoped service into `Startup.cs` or `Program.cs`
   ```c#
   services.AddScoped<IHttpService, HttpService>();
   ```
   or
   ```c#
   builder.Services.AddScoped<IHttpService, HttpService>();
   ```
5. Use it.

## Helper

### Install

1. Register own Toaster what have to inherit from `IToasterService`

2. Register `HelperService`
   - Add Service as Scoped service into `Startup.cs` or `Program.cs`
   ```c#
   services.AddScoped<IHelperService, HelperService>();
   ```
   or
   ```c#
   builder.Services.AddScoped<IHelperService, HelperService>();
   ```

## Common Service

> Service for common HTTP calls
>
> - Get element by Id
> - Get element lists
> - Post new element
> - Put element
> - Remove element

## Toaster

> You can add own Toaster service what is have to inherited from `IToasterService`
   
## Components

- Add using into `_Imports.razor`

```
@using Karcags.Blazor.Common
```

### Loader Component

```
<Loader></Loader>
```

### SimpleList

### ListTable
