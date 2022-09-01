using KarcagS.Common.Tools.HttpInterceptor.Agents;

namespace KarcagS.Common.Tools.HttpInterceptor.Converters;

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
