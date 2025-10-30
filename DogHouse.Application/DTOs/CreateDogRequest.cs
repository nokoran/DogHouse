using System.ComponentModel.DataAnnotations;

namespace DogHouse.Api.DTOs;

public class CreateDogRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Color { get; set; }
    [Required][Range(1, int.MaxValue)]
    public int TailLength { get; set; }
    [Required][Range(1, int.MaxValue)]
    public int Weight { get; set; }
}