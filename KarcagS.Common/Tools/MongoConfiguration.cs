namespace KarcagS.Common.Tools;

public class MongoConfiguration<Configuration> where Configuration : MongoCollectionConfiguration
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public Configuration CollectionNames { get; set; } = default!;
}
