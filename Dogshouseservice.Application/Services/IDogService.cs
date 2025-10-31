using Dogshouseservice.Application.DTOs;

namespace Dogshouseservice.Application.Services;

public interface IDogService
{
    Task<IEnumerable<DogResponse>> GetDogsAsync(PaginationQuery query);
    Task CreateDogAsync(CreateDogRequest request);
}