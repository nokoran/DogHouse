using Xunit;
using Assert = Xunit.Assert;
using Moq;
using Dogshouseservice.Api.Controllers;
using Dogshouseservice.Application.Services;
using Dogshouseservice.Application.DTOs;
using Dogshouseservice.Application.Interfaces;
using Dogshouseservice.Domain.Entities;
using Dogshouseservice.Infrastructure.Persistence.Data;
using Dogshouseservice.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dogshouseservice.Tests;

public class DogRepositoryTests
{
    private DbContextOptions<AppDbContext> GetInMemoryDbOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
    
    
    [Fact]
    public async Task GetDogsAsync_ShouldSortByWeightDescending_Correctly()
    {
        var options = GetInMemoryDbOptions();
        
        using (var context = new AppDbContext(options))
        {
            context.Dogs.AddRange(
                new Dog { Id = 1, Name = "Alpha", Color = "black", TailLength = 20, Weight = 50 },
                new Dog { Id = 2, Name = "Beta", Color = "white", TailLength = 15, Weight = 10 },
                new Dog { Id = 3, Name = "Charlie", Color = "brown", TailLength = 25, Weight = 30 }
            );
            await context.SaveChangesAsync();
            var repository = new DogRepository(context);
            
            var result = await repository.GetDogsAsync(
                attribute: "weight", 
                order: "desc", 
                pageNumber: 1, 
                pageSize: 3
            );
            
            var dogsList = result.ToList();
            Assert.Equal(3, dogsList.Count);
            Assert.Equal("Alpha", dogsList[0].Name); // Вага 50
            Assert.Equal("Charlie", dogsList[1].Name); // Вага 30
            Assert.Equal("Beta", dogsList[2].Name); // Вага 10
        }
    }

    [Fact]
    public async Task GetDogsAsync_ShouldReturnObjectsAccordingToPaginationRequest_Correctly()
    {
        var options = GetInMemoryDbOptions();

        using (var context = new AppDbContext(options))
        {
            context.Dogs.AddRange(
                new Dog { Id = 1, Name = "Alpha", Color = "black", TailLength = 20, Weight = 50 },
                new Dog { Id = 2, Name = "Beta", Color = "white", TailLength = 15, Weight = 10 },
                new Dog { Id = 3, Name = "Charlie", Color = "brown", TailLength = 25, Weight = 30 }
            );
            await context.SaveChangesAsync();
            var repository = new DogRepository(context);

            var result = await repository.GetDogsAsync(
                attribute: "name",
                order: "asc",
                pageNumber: 2,
                pageSize: 1
            );
            
            var dogsList = result.ToList();
            Assert.Single(dogsList);
            Assert.Equal("Beta", dogsList[0].Name);
        }
    }

    [Fact]
    public async Task DoesDogNameExistAsync_ShouldReturnTrue_WhenDogFoundInDB()
    {
        var options = GetInMemoryDbOptions();

        using (var context = new AppDbContext(options))
        {
            context.Dogs.AddRange(
                new Dog { Id = 1, Name = "Alpha", Color = "black", TailLength = 20, Weight = 50 },
                new Dog { Id = 2, Name = "Beta", Color = "white", TailLength = 15, Weight = 10 },
                new Dog { Id = 3, Name = "Charlie", Color = "brown", TailLength = 25, Weight = 30 }
            );
            await context.SaveChangesAsync();
            var repository = new DogRepository(context);

            Assert.True(repository.DoesDogNameExistAsync("Beta").Result);
        }
    }
    
    [Fact]
    public async Task DoesDogNameExistAsync_ShouldReturnFalse_WhenDogNotFoundInDB()
    {
        var options = GetInMemoryDbOptions();

        using (var context = new AppDbContext(options))
        {
            context.Dogs.AddRange(
                new Dog { Id = 1, Name = "Alpha", Color = "black", TailLength = 20, Weight = 50 },
                new Dog { Id = 2, Name = "Beta", Color = "white", TailLength = 15, Weight = 10 },
                new Dog { Id = 3, Name = "Charlie", Color = "brown", TailLength = 25, Weight = 30 }
            );
            await context.SaveChangesAsync();
            var repository = new DogRepository(context);

            Assert.False(repository.DoesDogNameExistAsync("Theta").Result);
        }
    }

    [Fact]
    public async Task AddDogAsync_ShouldUpdateDBWithNewEntities_Correctly()
    {
        var options = GetInMemoryDbOptions();

        using (var context = new AppDbContext(options))
        {
            context.Dogs.AddRange(
                new Dog { Id = 1, Name = "Alpha", Color = "black", TailLength = 20, Weight = 50 },
                new Dog { Id = 2, Name = "Beta", Color = "white", TailLength = 15, Weight = 10 },
                new Dog { Id = 3, Name = "Charlie", Color = "brown", TailLength = 25, Weight = 30 }
            );
            await context.SaveChangesAsync();
            var repository = new DogRepository(context);

            await repository.AddDogAsync(new Dog
                { Id = 4, Name = "Theta", Color = "brown", TailLength = 25, Weight = 30 });

            Assert.Equal(4, context.Dogs.Count());
        }
    }
    
}