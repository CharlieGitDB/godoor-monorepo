namespace Identity.API.Configurations;

#nullable disable
public record CosmosDbSection
{
    public string EndPointUri { get; set; }

    public string PrimaryKey { get; set; }

    public string DbName { get; set; }
}