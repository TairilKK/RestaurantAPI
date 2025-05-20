namespace RestaurantAPI.Dto;

public record RegisterUserDto(
    string Email,
    string Password,
    string ConfirmPassword,
    string? Nationality,
    DateTime? DateOfBirth,
    int RoleId=1
);