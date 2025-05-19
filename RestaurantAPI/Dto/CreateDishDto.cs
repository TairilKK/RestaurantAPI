using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Dto;

public record CreateDishDto(
    [Required]
    string Name,
    string Description,
    decimal Price
);