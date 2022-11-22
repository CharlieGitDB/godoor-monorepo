using System.Security.Claims;
using AutoWrapper.Wrappers;
using FluentValidation;
using FluentValidation.Results;
using Identity.API.Controllers.v1;
using Identity.API.Models.Request;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;

#nullable disable warnings

namespace Identity.Test.Identity.API.Controllers.v1;

[TestClass]
public class MeControllerTests
{   
    [TestMethod]
    public async Task GetOne_WithMatchingOid_ShouldReturnOneUser()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockData = Task.FromResult(new User
        {
            Oid = fakeoid
        });
        mockUserRepository.Setup(r => r.GetByIdAsync(fakeoid))
            .Returns(mockData);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new ("oid", fakeoid)
        }, "mock"));


        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = user
            }
        };
        var result = await controller.Get();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult) result;
        var okResultValue = (User) okResult.Value;
        Assert.IsNotNull(okResultValue);
        Assert.AreEqual(okResultValue.Oid, fakeoid);
    }

    
    [TestMethod]
    public async Task Post_ShouldReturnOkResult()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        var mockUserRepository = new Mock<IUserRepository>();
        var createUserRequest = new CreateUserRequest
        {
            ObjectId = fakeoid,
            DisplayName = "fakename",
            GivenName = "test",
            Surname = "lastname"
        };

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);

        var result = await controller.Post(createUserRequest);
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }
    
    [TestMethod]
    public async Task Post_ShouldReturnBadRequest_WhenThereIsNoOidInBody()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<CreateUserRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new ("Oid", "oid required")
                {
                    ErrorCode = "111"
                }
            }));
        var mockUserRepository = new Mock<IUserRepository>();
        var createUserRequest = new CreateUserRequest();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new ("oid", fakeoid)
        }, "mock"));


        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = user
            }
        };

        try {
            var result = await controller.Post(createUserRequest);
        } catch (ApiProblemDetailsException exception)
        {
            Assert.IsNotNull(exception);
            Assert.AreEqual(StatusCodes.Status422UnprocessableEntity, actual: exception.StatusCode);
        }
    }
    
    [TestMethod]
    public async Task Patch_ShouldReturnNotFoundWhenUserIsNotFound()
    {
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Patch( new JsonPatchDocument<User>());
        
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    
    [TestMethod]
    public async Task Patch_ShouldReturnOkResult()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockData = Task.FromResult(new User
        {
            Oid = fakeoid
        });
        mockUserRepository.Setup(r => r.GetByIdAsync(fakeoid))
            .Returns(mockData);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new ("oid", fakeoid)
        }, "mock"));

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = user
            }
        };
        var result = await controller.Patch(new JsonPatchDocument<User>());
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult) result;
        var okResultValue = (User) okResult.Value;
        Assert.IsNotNull(okResultValue);
    }
    
    [TestMethod]
    public async Task Delete_ShouldReturnOkResult()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(r => r.GetByIdAsync(fakeoid))
            .Returns(Task.FromResult(new User()));

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Delete();
        
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    
    [TestMethod]
    public async Task Delete_ShouldReturnNotFound()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new ("oid", fakeoid)
        }, "mock"));

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = user
            }
        };
        var result = await controller.Delete();
        
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
}