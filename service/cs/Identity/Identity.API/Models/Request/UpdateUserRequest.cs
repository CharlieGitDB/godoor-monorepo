using Identity.Domain.Enums;

namespace Identity.API.Models.Request;

public class UpdateUserRequest
{
    public Role? Role { get; set; }
    
    public bool? Active { get; set; }
}