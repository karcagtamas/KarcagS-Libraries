using KarcagS.Shared.Helpers;
using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public class ErrorConverter : IErrorConverter
{
    private readonly List<IErrorConverterAgent> agents;

    public ErrorConverter()
    {
        agents = new();
    }

    public ErrorConverter(List<IErrorConverterAgent> agents)
    {
        this.agents = agents;
    }

    public HttpResultError ConvertException(Exception exception)
    {
        foreach (var agent in agents)
        {
            var result = agent.TryToConvert(exception);

            if (ObjectHelper.IsNotNull(result))
            {
                return result;
            }
        }

        return new BasicErrorConverterAgent().TryToConvert(exception) ?? new HttpResultError();
    }
}
