using System.Text.Json.Serialization;
using FluentValidation;

#nullable disable

namespace Identity.API.Models.Request;

// {
//  "email": "johnsmith@fabrikam.onmicrosoft.com",
//  "identities": [ 
//      {
//      "signInType":"federated",
//      "issuer":"facebook.com",
//      "issuerAssignedId":"0123456789"
//      }
//  ],
//  "displayName": "John Smith",
//  "objectId": "11111111-0000-0000-0000-000000000000",
//  "givenName":"John",
//  "lastName":"Smith",
//  "step": "PostFederationSignup",
//  "client_id":"<guid>",
//  "ui_locales":"en-US"
// }

public class CreateUserRequest
{
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    
    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("surname")]
    public string Surname { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.ObjectId).NotEmpty();
        RuleFor(x => x.GivenName).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
    }
}