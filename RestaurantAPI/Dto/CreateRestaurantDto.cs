using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Dto;

public record CreateRestaurantDto(
    [Required]
    [MaxLength(30)]
    string Name,
    string? Description,
    string Category,
    bool HasDelivery,
    string? ContactEmail,
    string? ContactNumber,

    [Required]
    [MaxLength(50)]
    string City,
    [Required]
    [MaxLength(50)]
    string Street,
    string PostalCode
);