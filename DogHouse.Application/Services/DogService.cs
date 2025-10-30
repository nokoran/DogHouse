using DogHouse.Api.DTOs;

namespace DogHouse.Application.Services;

public class DogService : IDogService
{
    public async Task<IEnumerable<DogResponse>> GetDogsAsync(PaginationQuery query)
    {
        throw new NotImplementedException();
    }

    public async Task CreateDogAsync(CreateDogRequest request)
    {
        throw new NotImplementedException();
    }
}