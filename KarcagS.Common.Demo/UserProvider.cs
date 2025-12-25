using KarcagS.API.Shared.Configurations;
using KarcagS.API.Shared.Services;
using Microsoft.Extensions.Options;

namespace KarcagS.Common.Demo;

public class UserProvider(IHttpContextAccessor contextAccessor, IOptions<UtilsSettings> utilsOptions, DemoContext context) : HttpUserProvider<string>(contextAccessor, utilsOptions)
{
    private readonly DemoContext context = context;
}