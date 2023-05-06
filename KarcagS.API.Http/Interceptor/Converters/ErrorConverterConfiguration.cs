using KarcagS.API.Http.Interceptor.Agents;

namespace KarcagS.API.Http.Interceptor.Converters;

public class ErrorConverterConfiguration
{
    public List<IErrorConverterAgent> Agents { get; set; } = new();

    public ErrorConverterConfiguration()
    {

    }

    public ErrorConverterConfiguration AddAgent(IErrorConverterAgent agent)
    {
        if (Agents.All(x => x.GetType() != agent.GetType()))
        {
            Agents.Add(agent);
        }

        return this;
    }
}
