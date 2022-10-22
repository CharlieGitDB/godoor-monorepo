using Identity.Domain.Attributes;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Identity.Test.Identity.Domain.Extensions;

#nullable disable warnings

public class JsonPatchExtensionTestClass
{
    [PatchProtected(Accesslevel = 0, AllowedOperationTypes = new[] { (int) OperationType.Add })]
    public string Mock { get; set; }
}