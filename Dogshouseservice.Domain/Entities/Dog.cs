namespace Dogshouseservice.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class Dog
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int TailLength { get; set; }
    public int Weight { get; set; }
}