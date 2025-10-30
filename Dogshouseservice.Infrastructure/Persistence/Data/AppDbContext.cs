using Dogshouseservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Dogshouseservice.Infrastructure.Persistence.Data;

public class AppDbContext : DbContext
{
    public DbSet<Dog> Dogs { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }
}