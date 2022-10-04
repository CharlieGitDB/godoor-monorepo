namespace Identity.Domain.Entities;

public class Base
{
    public Guid Id { get; set; } 
    public DateTime Created { get; set; }
    public string CreatedByOid { get; set; }
    
    public DateTime Modified { get; set; }
    public string ModifiedByOid { get; set; }
}