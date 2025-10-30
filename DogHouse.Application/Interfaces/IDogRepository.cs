using DogHouse.Domain.Entities;

namespace DogHouse.Application.Interfaces;

public interface IDogRepository
{
    Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize);
    Task AddDogAsync(Dog dog);
    Task<bool> DoesDogNameExistAsync(string name);
}