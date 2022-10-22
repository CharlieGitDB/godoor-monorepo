using Identity.Domain.Enums;
using Identity.Domain.Extensions;
using Microsoft.AspNetCore.JsonPatch;

namespace Identity.Test.Identity.Domain.Extensions;

[TestClass]
public class JsonPatchDocumentExtensionsTests
{
    [TestMethod]
    public void ProtectedApplyTo_ShouldThrowArgumentNullException_WhenObjectToApplyToIsNull()
    {
        var patchDoc = new JsonPatchDocument<JsonPatchExtensionTestClass>();
        try
        {
            #pragma warning disable
            patchDoc.ProtectedApplyTo<JsonPatchExtensionTestClass>(null, (int) Role.User, error => {});
        }
        catch (ArgumentNullException e)
        {
            Assert.IsNotNull(e);
            return;
        }

        Assert.IsTrue(false); //fail if it makes it here
    }

    [TestMethod]
    public void ProtectedApplyTo_ShouldReturnErrorAction_WhenAccessLevelIsHigherLowerThanAccepted()
    {
        var testClass = new JsonPatchExtensionTestClass();
        var patchDoc = new JsonPatchDocument<JsonPatchExtensionTestClass>();
        patchDoc.Add(path: t => t.Mock, "test");

        patchDoc.ProtectedApplyTo(testClass, userRoleLevel: 1, error => {
            var notPermittedMessage = error.ErrorMessage.Contains("is not permitted");
            Assert.IsTrue(notPermittedMessage);
        });
    }

    [TestMethod]
    public void ProtectedApplyTo_ShouldReturnErrorAction_WhenOperationIsNotPermitted()
    {
        var testClass = new JsonPatchExtensionTestClass();
        var patchDoc = new JsonPatchDocument<JsonPatchExtensionTestClass>();
        patchDoc.Add(path: t => t.Mock, "test");

        patchDoc.ProtectedApplyTo(testClass, userRoleLevel: 0, error => {
            var notPermittedMessage = error.ErrorMessage.Contains("with operation");
            Assert.IsTrue(notPermittedMessage);
        });
    }

    [TestMethod]
    public void ProtectedApplyTo_ShouldNotReturnError_WhenAccessLevelAndOperationIsPermitted()
    {
        var testClass = new JsonPatchExtensionTestClass();
        var patchDoc = new JsonPatchDocument<JsonPatchExtensionTestClass>();
        patchDoc.Add(path: t => t.Mock, "test");

        patchDoc.ProtectedApplyTo(testClass, 0, error => {
            Assert.IsTrue(error.ErrorMessage == null);
        });
    }
}