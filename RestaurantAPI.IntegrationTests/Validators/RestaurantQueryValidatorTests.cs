using FluentValidation.TestHelper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;

namespace RestaurantAPI.IntegrationTests.Validators;

public class RestaurantQueryValidatorTests
{
    public static IEnumerable<object[]> GetSampleValidData()
    {
        var list = new List<RestaurantQuery>
        {
            new RestaurantQuery(null, 1, 10, null, null),
            new RestaurantQuery(null, 2, 15, null, null),
            new RestaurantQuery(null, 1, 15, nameof(Restaurant.Name), null),
            new RestaurantQuery(null, 14, 5, null, null)
        };

        return list.Select(q => new object[] { q });
    }

    [Theory]
    [MemberData(nameof(GetSampleValidData))]
    public void Validate_ForCorrectModel_ReturnsSuccess(RestaurantQuery model)
    {
        var validator = new RestaurantQueryValidator();

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    public static IEnumerable<object[]> GetSampleInvalidData()
    {
        var list = new List<RestaurantQuery>
        {
            new RestaurantQuery(null, 1, 11, null, null),
            new RestaurantQuery(null, 0, 15, null, null),
            new RestaurantQuery(null, -1, 15, nameof(Restaurant.Name), null),
            new RestaurantQuery(null, 14, 5, "HAHHAHAHAHAHA", null),
        };

        return list.Select(q => new object[] { q });
    }

    [Theory]
    [MemberData(nameof(GetSampleInvalidData))]
    public void Validate_ForIncorrectModel_ReturnsFail(RestaurantQuery model)
    {
        var validator = new RestaurantQueryValidator();

        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }

}