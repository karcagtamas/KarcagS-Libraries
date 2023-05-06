using KarcagS.API.Export.CSV;
using KarcagS.API.Export.Excel;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Export;

public static class ExportExtensions
{
    public static IServiceCollection AddExportServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvService, CsvService>();
        services.AddScoped<IExcelService, ExcelService>();

        return services;
    }
}