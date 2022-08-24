using System.Data;
using FluentValidation;

namespace Identity.API.Models.Request;

public class CreateUserRequest
{
    public string Oid { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Oid).NotEmpty();
    }
}