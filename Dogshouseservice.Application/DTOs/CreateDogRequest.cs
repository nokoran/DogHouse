using System.ComponentModel.DataAnnotations;

namespace Dogshouseservice.Application.DTOs;

public class CreateDogRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Color { get; set; }
    [Range(1, int.MaxValue)]
    public int TailLength { get; set; }
    [Range(1, int.MaxValue)]
    public int Weight { get; set; }
}