using FluentValidation.TestHelper;
using PubsApp.Application.DTOs;
using PubsApp.Application.Validators;
using Xunit;

namespace PubsApp.Tests.Unit.Validators;

public class CreatePublisherDtoValidatorTests
{
    private readonly CreatePublisherDtoValidator _validator;

    public CreatePublisherDtoValidatorTests()
    {
        _validator = new CreatePublisherDtoValidator();
    }

    #region PublisherId Tests

    [Theory]
    [InlineData("1389")]
    [InlineData("0736")]
    [InlineData("9999")]
    public void PublisherId_WhenValidFormat_ShouldNotHaveValidationError(string publisherId)
    {
        // Arrange
        var dto = new CreatePublisherDto { PublisherId = publisherId, PublisherName = "Test Publisher" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherId);
    }

    [Theory]
    [InlineData("")] // Empty
    [InlineData("138")] // Too short
    [InlineData("13899")] // Too long
    [InlineData("ABC1")] // Contains letters
    public void PublisherId_WhenInvalidFormat_ShouldHaveValidationError(string publisherId)
    {
        // Arrange
        var dto = new CreatePublisherDto { PublisherId = publisherId, PublisherName = "Test Publisher" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
    }

    #endregion

    #region PublisherName Tests

    [Theory]
    [InlineData("AB")] // Minimum length
    [InlineData("New Moon Books")]
    [InlineData("This is a very long publisher name!!")] // 40 characters
    public void PublisherName_WhenValidLength_ShouldNotHaveValidationError(string name)
    {
        // Arrange
        var dto = new CreatePublisherDto { PublisherId = "1389", PublisherName = name };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherName);
    }

    [Fact]
    public void PublisherName_WhenEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePublisherDto { PublisherId = "1389", PublisherName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherName);
    }

    [Fact]
    public void PublisherName_WhenTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePublisherDto { PublisherId = "1389", PublisherName = "A" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherName);
    }

    [Fact]
    public void PublisherName_WhenExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
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
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
            PublisherName = "Test Publisher",
            City = city
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.City);
    }

    [Fact]
    public void City_WhenExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
            PublisherName = "Test Publisher",
            City = new string('A', 21) // 21 characters
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.City);
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
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
            PublisherName = "Test Publisher",
            State = state
        };

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
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
            PublisherName = "Test Publisher",
            State = state
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.State);
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
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
            PublisherName = "Test Publisher",
            Country = country
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Country);
    }

    [Fact]
    public void Country_WhenExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreatePublisherDto 
        { 
            PublisherId = "1389", 
            PublisherName = "Test Publisher",
            Country = new string('A', 31) // 31 characters
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Country);
    }

    #endregion
}
