using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Karcags.Common.Middlewares;

public static class DatabaseMigration
{
    public static WebApplication Migrate<T>(this WebApplication app) where T : DbContext
    {
        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();
            try
            {
                var db = scope.ServiceProvider.GetRequiredService<T>();

                logger.LogInformation("Start migration...");
                db.Database.EnsureCreated();

                logger.LogInformation("Database migrated...");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unexpected error during the migration");
            }
        }

        return app;
    }
}