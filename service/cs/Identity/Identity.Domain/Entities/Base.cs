using Identity.Domain.Attributes;

#nullable disable

namespace Identity.Domain.Entities;

public class Base
{
    [PatchProtected]
    public string Oid { get; set; }
   
    [PatchProtected]
    public DateTime Created { get; set; }

    [PatchProtected]
    public string CreatedByOid { get; set; }
    
    [PatchProtected]
    public DateTime Modified { get; set; }
    
    [PatchProtected]
    public string ModifiedByOid { get; set; }
}