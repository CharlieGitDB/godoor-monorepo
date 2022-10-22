using System.Reflection;
using Identity.Domain.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace Identity.Domain.Extensions;

public static class JsonPatchDocumentExtensions
{
    // Inspired by: https://stackoverflow.com/a/71174546
    public static void ProtectedApplyTo<T>(
        this JsonPatchDocument<T> patchDoc, 
        T objectToApplyTo,
        int userRoleLevel,
        Action<JsonPatchError> errorAction) where T : class
    {
        if (patchDoc == null)
        {
            throw new ArgumentNullException(nameof(patchDoc));
        }

        if (objectToApplyTo == null)
        {
            throw new ArgumentNullException(nameof(objectToApplyTo));
        }

        patchDoc.Operations.ForEach(op =>
        {
            if (!string.IsNullOrWhiteSpace(op.path))
            {
                var pathToPatch = op.path.Trim('/').ToLowerInvariant();
                var objectToPatch = objectToApplyTo.GetType().Name;
                var attributesFilter = BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
                var fieldToPatch = typeof(T).GetProperties(attributesFilter).FirstOrDefault(p => p.Name.Equals(pathToPatch, StringComparison.InvariantCultureIgnoreCase));

                var fieldPatchProtectedAttribute = fieldToPatch?
                    .GetCustomAttributes(typeof(PatchProtectedAttribute),false)
                    .Cast<PatchProtectedAttribute>()
                    .SingleOrDefault();

                if (fieldPatchProtectedAttribute != null)
                {
                    if (userRoleLevel > fieldPatchProtectedAttribute.Accesslevel)
                    {
                        errorAction(new JsonPatchError(
                            objectToApplyTo,
                            op,
                            $"Current user is not permitted to patch {objectToPatch}.{fieldToPatch!.Name}"));
                    }
                    else if (fieldPatchProtectedAttribute.AllowedOperationTypes != null && !fieldPatchProtectedAttribute.AllowedOperationTypes.Contains((int) op.OperationType))
                    {
                        errorAction(new JsonPatchError(
                            objectToApplyTo,
                            op,
                            $"Current user is not permitted to patch {objectToPatch}.{fieldToPatch!.Name} with operation ${op.OperationType}"));
                    }
                }
            }
        });

        patchDoc.ApplyTo(objectToApplyTo, errorAction);
    }
}