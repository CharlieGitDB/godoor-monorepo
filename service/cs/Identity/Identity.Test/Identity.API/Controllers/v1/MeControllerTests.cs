using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Identity.API.Controllers.v1;
using Identity.API.Models.Request;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Identity.Test.Identity.API.Controllers.v1;

[TestClass]
public class MeControllerTests
{

    [TestMethod]
    public async Task GetAll_ShouldReturnListOfUsers()
    {
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockData = Task.FromResult(new List<User>
        {
            new(),
            new()
        });
        mockUserRepository.Setup(r => r.GetAllAsync())
           .Returns(mockData);

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Get();
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult) result;
        var okResultValue = (List<User>) okResult.Value;
        Assert.IsNotNull(okResultValue);
        Assert.AreEqual(okResultValue.Count, 2);
    }
    
    [TestMethod]
    public async Task GetOne_WithMatchingOid_ShouldReturnOneUser()
    {
        var oid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockData = Task.FromResult(new User
        {
            Oid = oid
        });
        mockUserRepository.Setup(r => r.GetByOidAsync(oid))
            .Returns(mockData);

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Get(oid);
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult) result;
        var okResultValue = (User) okResult.Value;
        Assert.IsNotNull(okResultValue);
        Assert.AreEqual(okResultValue.Oid, oid);
    }
    
    [TestMethod]
    public async Task GetOne_WithoutMatchingOid_ShouldReturnNotFound()
    {
        var oid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockData = Task.FromResult(new User
        {
            Oid = oid
        });
        mockUserRepository.Setup(r => r.GetByOidAsync("wrongid"))
            .Returns(mockData);

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Get(oid);
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
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
            Oid = fakeoid
        };
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
        var result = await controller.Post(createUserRequest);
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
    
    [TestMethod]
    public async Task Put_ShouldReturnNotFoundWhenUserIsNotFound()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Patch(fakeoid, new UpdateUserRequest());
        
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    
    [TestMethod]
    public async Task Put_ShouldReturnOkResult()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        var mockData = Task.FromResult(new User
        {
            Oid = fakeoid
        });
        mockUserRepository.Setup(r => r.GetByOidAsync(fakeoid))
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
        var result = await controller.Patch(fakeoid, new UpdateUserRequest());
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult) result;
        var okResultValue = (int) okResult.Value;
        Assert.IsNotNull(okResultValue);
    }
    
    [TestMethod]
    public async Task Delete_ShouldReturnOkResult()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(r => r.GetByOidAsync(fakeoid))
            .Returns(Task.FromResult(new User()));

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Delete(fakeoid);
        
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    
    [TestMethod]
    public async Task Delete_ShouldReturnNotFound()
    {
        var fakeoid = "fakeoid";
        var mockValidator = new Mock<IValidator<CreateUserRequest>>();
        var mockUserRepository = new Mock<IUserRepository>();

        var controller = new MeController(mockValidator.Object, mockUserRepository.Object);
        var result = await controller.Delete(fakeoid);
        
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
}