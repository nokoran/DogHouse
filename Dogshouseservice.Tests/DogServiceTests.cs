using Xunit;
using Assert = Xunit.Assert;
using Moq;
using Dogshouseservice.Api.Controllers;
using Dogshouseservice.Application.Services;
using Dogshouseservice.Application.DTOs;
using Dogshouseservice.Application.Interfaces;
using Dogshouseservice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dogshouseservice.Tests;

public class DogServiceTests
{
    private readonly Mock<IDogRepository> _mockIDogRepository;
    private readonly DogService _dogService;
    
    public DogServiceTests()
    {
        _mockIDogRepository = new Mock<IDogRepository>();
        _dogService = new DogService(_mockIDogRepository.Object);
    }

    [Fact]
    public async Task CreateDogAsync_InvokesAllMethodsOnce()
    {
        var createDogRequest = new CreateDogRequest
        {
            Name = "Max",
            Color = "Black",
            TailLength = 15,
            Weight = 25
        };

        _mockIDogRepository.Setup(s => s.DoesDogNameExistAsync(createDogRequest.Name))
            .ReturnsAsync(false);

        await _dogService.CreateDogAsync(createDogRequest);
        
        _mockIDogRepository.Verify(repo => 
                repo.DoesDogNameExistAsync(It.IsAny<string>()), 
        Times.Once);
        
        _mockIDogRepository.Verify(repo => 
                repo.AddDogAsync(It.IsAny<Dog>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task CreateDogAsync_ShouldReturnException_IfNameAlreadyExists()
    {
        var createDogRequest = new CreateDogRequest
        {
            Name = "Max",
            Color = "Black",
            TailLength = 15,
            Weight = 25
        };

        _mockIDogRepository.Setup(s => s.DoesDogNameExistAsync(createDogRequest.Name))
            .ReturnsAsync(true);
        
        await Assert.ThrowsAsync<InvalidOperationException>(()
            => _dogService.CreateDogAsync(createDogRequest));
        
        _mockIDogRepository.Verify(repo => 
                repo.AddDogAsync(It.IsAny<Dog>()), 
            Times.Never);
    }
    
    [Fact]
    public async Task GetDogsAsync_ShouldReturnCorrectlyMappedData()
    {
        PaginationQuery query = new PaginationQuery
        {
            Attribute = "Name",
            Order = "ASC",
            PageNumber = 1,
            PageSize = 10
        };
        
        var dogs = new List<Dog>
        {
            new Dog { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 20 }
        };

        _mockIDogRepository.Setup(s => s.GetDogsAsync(query.Attribute, query.Order, query.PageNumber, query.PageSize))
            .ReturnsAsync(dogs);

        var actionResult = await _dogService.GetDogsAsync(query);
        
        Assert.IsAssignableFrom<IEnumerable<DogResponse>>(actionResult);
        
        Assert.Equal("Buddy", actionResult.First().Name);
    }
}