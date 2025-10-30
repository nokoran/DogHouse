using DogHouse.Application.Interfaces;
using DogHouse.Domain.Entities;
using DogHouse.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace DogHouse.Infrastructure.Persistence.Repositories;

public class DogRepository : IDogRepository
{
    private readonly AppDbContext _context;
    
    public DogRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize)
    {
        var query = _context.Dogs.AsQueryable();
        bool isDescending = order?.ToLower() == "desc";
        switch (attribute?.ToLower())
        {
            case "name":
                query = isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name);
                break;
            case "color":
                query = isDescending ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color);
                break;
            case "taillength":
                query = isDescending ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength);
                break;
            case "weight":
                query = isDescending ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight);
                break;
            default:
                query = isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name);
                break;
        }
        
        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return await query.ToListAsync();
    }

    public async Task AddDogAsync(Dog dog)
    {
        await _context.Dogs.AddAsync(dog);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DoesDogNameExistAsync(string name)
    {
        return await _context.Dogs.AnyAsync(d => d.Name.ToLower() == name.ToLower());
    }
}