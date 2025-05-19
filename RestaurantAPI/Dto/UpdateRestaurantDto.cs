using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Dto;

public record UpdateRestaurantDto(
    [MaxLength(30)]
    string Name,
    string Description,
    bool HasDelivery
);