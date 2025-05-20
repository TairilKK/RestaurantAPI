using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators;

public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
{
    private readonly int[] _allowedPageSizes = [5, 10, 15];

    private readonly string[] _allowedSortByColumnNames =
    [
        nameof(Restaurant.Name),
        nameof(Restaurant.Description),
        nameof(Restaurant.Category)
    ];
    public RestaurantQueryValidator()
    {
        RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(", ", _allowedPageSizes)}]");
            }
        });

        RuleFor(r => r.SortBy).Must(value => value is null || _allowedSortByColumnNames.Contains(value))
            .WithMessage($"Sort by is optional, or must be in [{string.Join(",", _allowedSortByColumnNames)}]");
    }
}