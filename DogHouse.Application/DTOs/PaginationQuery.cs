using System.ComponentModel.DataAnnotations;

namespace DogHouse.Api.DTOs;

public class PaginationQuery
{
    public string? Attribute { get; set; }
    public string? Order { get; set; }
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}