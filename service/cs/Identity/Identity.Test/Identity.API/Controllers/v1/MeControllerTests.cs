using FluentValidation;
using Identity.API.Controllers.v1;
using Identity.API.Models.Request;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
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
}