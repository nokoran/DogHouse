using Xunit;
using Assert = Xunit.Assert;
using Moq;
using Dogshouseservice.Api.Controllers;
using Dogshouseservice.Application.Services;
using Dogshouseservice.Application.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace Dogshouseservice.Tests;

public class DogsControllerTests 
{
    private readonly Mock<IDogService> _mockDogService;
    private readonly DogsController _controller;
    
    public DogsControllerTests()
    {
        _mockDogService = new Mock<IDogService>();
        _controller = new DogsController(_mockDogService.Object);
    }
    
    [Fact]
    public void Ping_ShouldReturnOk_WithVersionString()
    {
        var actionResult = _controller.Ping();
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<string>(okResult.Value);
        Assert.Equal("Dogshouseservice.Version1.0.1", returnValue);
    }

    [Fact]
    public async Task GetDogs_ShouldReturnOk_WithListOfDogs()
    {
        var dogs = new List<DogResponse>
        {
            new DogResponse { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 20 }
        };
        _mockDogService.Setup(s => s.GetDogsAsync(It.IsAny<PaginationQuery>()))
            .ReturnsAsync(dogs);
        var actionResult = await _controller.GetDogs(new PaginationQuery());
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnDogs = Assert.IsAssignableFrom<IEnumerable<DogResponse>>(okResult.Value);
        Assert.Single((List<DogResponse>)returnDogs);
    }
    
    [Fact]
    public async Task CreateDog_ShouldReturnCreated_WhenDogIsCreated()
    {
        var createDogRequest = new CreateDogRequest
        {
            Name = "Max",
            Color = "Black",
            TailLength = 15,
            Weight = 25
        };
        
        _mockDogService.Setup(s => s.CreateDogAsync(createDogRequest))
            .Returns(Task.CompletedTask);
        
        var actionResult = await _controller.CreateDog(createDogRequest);
        var createdResult = Assert.IsAssignableFrom<ObjectResult>(actionResult);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
     public async Task CreateDog_ShouldReturnConflict_WhenDogAlreadyExists()
    {
        var createDogRequest = new CreateDogRequest
        {
            Name = "Max",
            Color = "Black",
            TailLength = 15,
            Weight = 25
        };
        
        _mockDogService.Setup(s => s.CreateDogAsync(createDogRequest))
            .ThrowsAsync(new InvalidOperationException());
        
        var actionResult = await _controller.CreateDog(createDogRequest);
        var conflictResult = Assert.IsAssignableFrom<ConflictObjectResult>(actionResult);
        Assert.Equal(409, conflictResult.StatusCode);
        
    }

    [Fact]
    public async Task CreateDog_ShouldReturnBadRequest_WhenDataIsInvalid()
    {
        var createDogRequest = new CreateDogRequest
        {
            Name = "Max",
            Color = "Black",
            TailLength = -15,
            Weight = 25
        };
        
        _controller.ModelState.AddModelError("TailLength", "Tail length must be positive");
        
        var actionResult = await _controller.CreateDog(createDogRequest);
        var badRequestResult = Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
        Assert.Equal(400, badRequestResult.StatusCode);
        
        _mockDogService.Verify(service => 
                service.CreateDogAsync(It.IsAny<CreateDogRequest>()), 
            Times.Never);
    }
}