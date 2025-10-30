using Dogshouseservice.Infrastructure.Persistence.Data;
using Dogshouseservice.Infrastructure.Persistence.Repositories;
using Dogshouseservice.Application.Interfaces;
using Dogshouseservice.Application.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDogRepository, DogRepository>();
builder.Services.AddScoped<IDogService, DogService>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;


    options.AddFixedWindowLimiter(policyName: "default", windowOptions =>
    {
        windowOptions.PermitLimit = 10;
        windowOptions.Window = TimeSpan.FromSeconds(1);
    });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRateLimiter();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();