namespace RestaurantAPI.Models;

public record RestaurantQuery
(
    string? SearchPhrase,
    int PageNumber,
    int PageSize,
    string? SortBy,
    SortDirection? SortDirection
);