using DogHouse.Api.DTOs;
using DogHouse.Application.Interfaces;
using DogHouse.Domain.Entities;

namespace DogHouse.Application.Services;

public class DogService : IDogService
{
    private readonly IDogRepository _dogRepository;
    public DogService(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }
    public async Task<IEnumerable<DogResponse>> GetDogsAsync(PaginationQuery query)
    {
        var dogs = await _dogRepository.GetDogsAsync(query.Attribute, query.Order, query.PageNumber, query.PageSize);
        return dogs.Select(dog => new DogResponse
        {
            Name = dog.Name,
            Color = dog.Color,
            TailLength = dog.TailLength,
            Weight = dog.Weight
        });
    }

    public async Task CreateDogAsync(CreateDogRequest request)
    {
        if (await _dogRepository.DoesDogNameExistAsync(request.Name))
        {
            throw new InvalidOperationException("Dog with this name already exists.");
        }
        Dog newDog = new Dog
        {
            Name = request.Name,
            Color = request.Color,
            TailLength = request.TailLength,
            Weight = request.Weight
        };
        await _dogRepository.AddDogAsync(newDog);
    }
}