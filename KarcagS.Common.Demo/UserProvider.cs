using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Services;
using Microsoft.Extensions.Options;

namespace KarcagS.Common.Demo;

public class UserProvider : HttpUserProvider<string>
{
    private readonly DemoContext context;

    public UserProvider(IHttpContextAccessor contextAccessor, IOptions<UtilsSettings> utilsOptions, DemoContext context) : base(contextAccessor, utilsOptions)
    {
        this.context = context;
    }

    public override Task<T?> GetCurrentUser<T>() where T : class
    {
        // context.Set<User>()

        return base.GetCurrentUser<T>();
    }
}