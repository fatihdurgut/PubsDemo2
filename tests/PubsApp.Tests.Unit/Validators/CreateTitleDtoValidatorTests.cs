using FluentValidation.TestHelper;
using PubsApp.Application.DTOs;
using PubsApp.Application.Validators;
using Xunit;

namespace PubsApp.Tests.Unit.Validators;

public class CreateTitleDtoValidatorTests
{
    private readonly CreateTitleDtoValidator _validator;

    public CreateTitleDtoValidatorTests()
    {
        _validator = new CreateTitleDtoValidator();
    }

    [Theory]
    [InlineData("BU1032")] // Valid format
    [InlineData("TC7777")]
    [InlineData("PS2091")]
    public void TitleId_WhenValidFormat_ShouldNotHaveValidationError(string titleId)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = titleId, TitleName = "Test", Type = "business", PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.TitleId);
    }

    [Theory]
    [InlineData("")] // Empty
    [InlineData("BU12")] // Too short
    [InlineData("BU12345")] // Too long
    [InlineData("bu1032")] // Lowercase
    [InlineData("B11032")] // Only one letter
    [InlineData("BU10A2")] // Letter in number position
    [InlineData("1B1032")] // Number in letter position
    public void TitleId_WhenInvalidFormat_ShouldHaveValidationError(string titleId)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = titleId, TitleName = "Test", Type = "business", PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TitleId);
    }

    [Fact]
    public void TitleName_WhenEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "", Type = "business", PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TitleName);
    }

    [Fact]
    public void TitleName_WhenExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTitleDto 
        { 
            TitleId = "BU1032", 
            TitleName = new string('A', 81), // 81 characters (max is 80)
            Type = "business", 
            PublishedDate = DateTime.Now 
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TitleName);
    }

    [Theory]
    [InlineData("business")]
    [InlineData("popular_comp")]
    [InlineData("psychology")]
    [InlineData("trad_cook")]
    [InlineData("mod_cook")]
    [InlineData("UNDECIDED")]
    public void Type_WhenValid_ShouldNotHaveValidationError(string type)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = type, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Type);
    }

    [Theory]
    [InlineData("fiction")]
    [InlineData("romance")]
    [InlineData("")]
    [InlineData("Business")] // Case sensitive
    public void Type_WhenInvalid_ShouldHaveValidationError(string type)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = type, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    [Theory]
    [InlineData("1389")] // Valid format
    [InlineData("0736")]
    [InlineData("0877")]
    public void PublisherId_WhenValidFormat_ShouldNotHaveValidationError(string publisherId)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = "business", PublisherId = publisherId, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublisherId);
    }

    [Theory]
    [InlineData("138")] // Too short
    [InlineData("13899")] // Too long
    [InlineData("138A")] // Contains letter
    public void PublisherId_WhenInvalidFormat_ShouldHaveValidationError(string publisherId)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = "business", PublisherId = publisherId, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10.99)]
    [InlineData(999999.99)]
    public void Price_WhenValid_ShouldNotHaveValidationError(decimal price)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = "business", Price = price, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1000000)]
    public void Price_WhenInvalid_ShouldHaveValidationError(decimal price)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = "business", Price = price, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Royalty_WhenValid_ShouldNotHaveValidationError(int royalty)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = "business", Royalty = royalty, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Royalty);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Royalty_WhenInvalid_ShouldHaveValidationError(int royalty)
    {
        // Arrange
        var dto = new CreateTitleDto { TitleId = "BU1032", TitleName = "Test", Type = "business", Royalty = royalty, PublishedDate = DateTime.Now };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Royalty);
    }

    [Fact]
    public void PublishedDate_WhenInFuture_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTitleDto 
        { 
            TitleId = "BU1032", 
            TitleName = "Test", 
            Type = "business", 
            PublishedDate = DateTime.Now.AddDays(1) 
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishedDate);
    }

    [Fact]
    public void PublishedDate_WhenInPast_ShouldNotHaveValidationError()
    {
        // Arrange
        var dto = new CreateTitleDto 
        { 
            TitleId = "BU1032", 
            TitleName = "Test", 
            Type = "business", 
            PublishedDate = DateTime.Now.AddDays(-1) 
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PublishedDate);
    }

    [Fact]
    public void Notes_WhenExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTitleDto 
        { 
            TitleId = "BU1032", 
            TitleName = "Test", 
            Type = "business", 
            Notes = new string('A', 201), // 201 characters (max is 200)
            PublishedDate = DateTime.Now 
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void YtdSales_WhenNegative_ShouldHaveValidationError()
    {
        // Arrange
        var dto = new CreateTitleDto 
        { 
            TitleId = "BU1032", 
            TitleName = "Test", 
            Type = "business", 
            YtdSales = -1,
            PublishedDate = DateTime.Now 
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.YtdSales);
    }
}
