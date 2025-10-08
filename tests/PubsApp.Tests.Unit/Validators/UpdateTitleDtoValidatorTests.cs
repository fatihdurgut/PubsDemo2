using FluentValidation.TestHelper;
using PubsApp.Application.DTOs;
using PubsApp.Application.Validators;
using Xunit;

namespace PubsApp.Tests.Unit.Validators;

public class UpdateTitleDtoValidatorTests
{
    private readonly UpdateTitleDtoValidator _validator;

    public UpdateTitleDtoValidatorTests()
    {
        _validator = new UpdateTitleDtoValidator();
    }

    [Fact]
    public void TitleName_WhenProvidedAndValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { TitleName = "Valid Title" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.TitleName);
    }

    [Fact]
    public void TitleName_WhenProvidedAndEmpty_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { TitleName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        // Empty strings don't trigger validation in update scenarios - the field is simply skipped
        result.ShouldNotHaveValidationErrorFor(x => x.TitleName);
    }

    [Fact]
    public void TitleName_WhenNotProvided_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { TitleName = null };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.TitleName);
    }

    [Theory]
    [InlineData("business")]
    [InlineData("psychology")]
    public void Type_WhenProvidedAndValid_ShouldNotHaveValidationError(string type)
    {
        // Arrange
        var dto = new UpdateTitleDto { Type = type };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Type);
    }

    [Theory]
    [InlineData("fiction")]
    [InlineData("romance")]
    public void Type_WhenProvidedAndInvalid_ShouldHaveValidationError(string type)
    {
        // Arrange
        var dto = new UpdateTitleDto { Type = type };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    [Fact]
    public void Type_WhenNotProvided_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { Type = null };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Type);
    }

    [Theory]
    [InlineData("1389")]
    [InlineData("0736")]
    public void PublisherId_WhenProvidedAndValid_ShouldNotHaveValidationError(string publisherId)
    {
        // Arrange
        var dto = new UpdateTitleDto { PublisherId = publisherId };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherId);
    }

    [Theory]
    [InlineData("138")] // Too short
    [InlineData("13899")] // Too long
    public void PublisherId_WhenProvidedAndInvalid_ShouldHaveValidationError(string publisherId)
    {
        // Arrange
        var dto = new UpdateTitleDto { PublisherId = publisherId };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(19.99)]
    public void Price_WhenProvidedAndValid_ShouldNotHaveValidationError(decimal price)
    {
        // Arrange
        var dto = new UpdateTitleDto { Price = price };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Price_WhenProvidedAndNegative_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { Price = -0.01m };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public void Royalty_WhenProvidedAndValid_ShouldNotHaveValidationError(int royalty)
    {
        // Arrange
        var dto = new UpdateTitleDto { Royalty = royalty };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Royalty);
    }

    [Fact]
    public void Royalty_WhenProvidedAndExceeds100_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { Royalty = 101 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Royalty);
    }

    [Fact]
    public void PublishedDate_WhenProvidedAndInFuture_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { PublishedDate = DateTime.Now.AddDays(1) };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishedDate);
    }

    [Fact]
    public void PublishedDate_WhenProvidedAndInPast_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { PublishedDate = DateTime.Now.AddDays(-1) };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublishedDate);
    }

    [Fact]
    public void PublishedDate_WhenDefaultValue_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdateTitleDto { PublishedDate = default };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublishedDate);
    }
}
