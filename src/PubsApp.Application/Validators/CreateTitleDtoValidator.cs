using FluentValidation;
using PubsApp.Application.DTOs;

namespace PubsApp.Application.Validators;

/// <summary>
/// Validator for CreateTitleDto
/// Enforces business rules for creating new titles
/// </summary>
public class CreateTitleDtoValidator : AbstractValidator<CreateTitleDto>
{
    private static readonly string[] ValidTypes = 
    {
        "business",
        "popular_comp",
        "psychology",
        "trad_cook",
        "mod_cook",
        "UNDECIDED"
    };

    public CreateTitleDtoValidator()
    {
        RuleFor(x => x.TitleId)
            .NotEmpty()
            .WithMessage("Title ID is required")
            .Length(6)
            .WithMessage("Title ID must be exactly 6 characters")
            .Matches(@"^[A-Z]{2}[0-9]{4}$")
            .WithMessage("Title ID must be in format: 2 uppercase letters followed by 4 digits (e.g., BU1032)");

        RuleFor(x => x.TitleName)
            .NotEmpty()
            .WithMessage("Title name is required")
            .MaximumLength(80)
            .WithMessage("Title name cannot exceed 80 characters");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required")
            .Must(type => ValidTypes.Contains(type))
            .WithMessage($"Type must be one of: {string.Join(", ", ValidTypes)}");

        RuleFor(x => x.PublisherId)
            .Length(4)
            .WithMessage("Publisher ID must be exactly 4 characters")
            .Matches(@"^[0-9]{4}$")
            .WithMessage("Publisher ID must be 4 digits")
            .When(x => !string.IsNullOrEmpty(x.PublisherId));

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Price cannot exceed 999,999.99")
            .When(x => x.Price.HasValue);

        RuleFor(x => x.Advance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Advance must be greater than or equal to 0")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Advance cannot exceed 999,999.99")
            .When(x => x.Advance.HasValue);

        RuleFor(x => x.Royalty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Royalty must be greater than or equal to 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Royalty percentage cannot exceed 100")
            .When(x => x.Royalty.HasValue);

        RuleFor(x => x.YtdSales)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Year-to-date sales must be greater than or equal to 0")
            .When(x => x.YtdSales.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(200)
            .WithMessage("Notes cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.PublishedDate)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Published date cannot be in the future");
    }
}
