namespace Identity.API.Configurations;

public class CosmosDbSection
{
    public string EndPointUri { get; set; }

    public string PrimaryKey { get; set; }

    public string DbName { get; set; }
}