using FluentValidation.TestHelper;
using PubsApp.Application.DTOs;
using PubsApp.Application.Validators;
using Xunit;

namespace PubsApp.Tests.Unit.Validators;

public class UpdatePublisherDtoValidatorTests
{
    private readonly UpdatePublisherDtoValidator _validator;

    public UpdatePublisherDtoValidatorTests()
    {
        _validator = new UpdatePublisherDtoValidator();
    }

    #region PublisherName Tests

    [Theory]
    [InlineData("AB")] // Minimum length
    [InlineData("New Moon Books")]
    [InlineData("This is a very long publisher name!!")] // 40 characters
    public void PublisherName_WhenProvidedAndValid_ShouldNotHaveValidationError(string name)
    {
        // Arrange
        var dto = new UpdatePublisherDto { PublisherName = name };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherName);
    }

    [Fact]
    public void PublisherName_WhenProvidedAndEmpty_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto { PublisherName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        // Empty strings don't trigger validation in update scenarios - the field is simply skipped
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherName);
    }

    [Fact]
    public void PublisherName_WhenNotProvided_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto { PublisherName = null };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherName);
    }

    [Fact]
    public void PublisherName_WhenProvidedAndExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto 
        { 
            PublisherName = new string('A', 41) // 41 characters
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherName);
    }

    #endregion

    #region City Tests

    [Theory]
    [InlineData("Boston")]
    [InlineData("New York")]
    [InlineData("San Francisco")]
    public void City_WhenProvidedAndValid_ShouldNotHaveValidationError(string city)
    {
        // Arrange
        var dto = new UpdatePublisherDto { City = city };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void City_WhenProvidedAndExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto 
        { 
            City = new string('A', 21) // 21 characters
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void City_WhenNotProvided_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto { City = null };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.City);
    }

    #endregion

    #region State Tests

    [Theory]
    [InlineData("CA")]
    [InlineData("NY")]
    [InlineData("TX")]
    [InlineData("MA")]
    public void State_WhenProvidedAndValidUSState_ShouldNotHaveValidationError(string state)
    {
        // Arrange
        var dto = new UpdatePublisherDto { State = state };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.State);
    }

    [Theory]
    [InlineData("ZZ")] // Invalid state code
    [InlineData("XX")]
    [InlineData("123")]
    public void State_WhenProvidedAndInvalid_ShouldHaveValidationError(string state)
    {
        // Arrange
        var dto = new UpdatePublisherDto { State = state };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
    }

    [Fact]
    public void State_WhenNotProvided_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto { State = null };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.State);
    }

    #endregion

    #region Country Tests

    [Theory]
    [InlineData("USA")]
    [InlineData("United States")]
    [InlineData("Canada")]
    public void Country_WhenProvidedAndValid_ShouldNotHaveValidationError(string country)
    {
        // Arrange
        var dto = new UpdatePublisherDto { Country = country };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Country);
    }

    [Fact]
    public void Country_WhenProvidedAndExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto 
        { 
            Country = new string('A', 31) // 31 characters
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    [Fact]
    public void Country_WhenNotProvided_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new UpdatePublisherDto { Country = null };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Country);
    }

    #endregion
}
