using Identity.API.Configurations;
using Microsoft.AspNetCore.Authorization;

public class ApiConnectorRequirement : ApiConnectorSection, IAuthorizationRequirement
{
}