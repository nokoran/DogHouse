using DogHouse.Api.DTOs;

namespace DogHouse.Application.Services;

public interface IDogService
{
    Task<IEnumerable<DogResponse>> GetDogsAsync(PaginationQuery query);
    Task CreateDogAsync(CreateDogRequest request);
}