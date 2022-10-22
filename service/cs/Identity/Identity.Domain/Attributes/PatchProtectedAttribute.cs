using Identity.Domain.Enums;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Identity.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PatchProtectedAttribute : Attribute
{
    public int Accesslevel { get; set; }

    public int[]? AllowedOperationTypes { get; set; }

    public PatchProtectedAttribute(){}
    // TODO: Make this apply at class level
    public PatchProtectedAttribute(int accessLevel)
    {
        this.Accesslevel = accessLevel;
    }

    public PatchProtectedAttribute(int accessLevel, int[] operationTypes)
    {
        this.Accesslevel = accessLevel;

        List<int> operationTypeList = operationTypes.ToList();
        operationTypeList.ForEach(o =>
        {
            var operationType = (OperationType?) o;
            if (!operationType.HasValue)
            {
                throw new ArgumentException($"{o} is not a valid patch operation type");
            }
        });
        this.AllowedOperationTypes = operationTypes;
    }
}