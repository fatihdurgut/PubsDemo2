using FluentValidation;
using PubsApp.Application.DTOs;

namespace PubsApp.Application.Validators;

/// <summary>
/// Validator for CreatePublisherDto
/// Enforces business rules for creating new publishers
/// </summary>
public class CreatePublisherDtoValidator : AbstractValidator<CreatePublisherDto>
{
    private static readonly string[] ValidStates = 
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
        "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
        "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
        "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
    };

    public CreatePublisherDtoValidator()
    {
        RuleFor(x => x.PublisherId)
            .NotEmpty()
            .WithMessage("Publisher ID is required")
            .Length(4)
            .WithMessage("Publisher ID must be exactly 4 characters")
            .Matches(@"^[0-9]{4}$")
            .WithMessage("Publisher ID must be 4 digits (e.g., 9900-9999, 1389, 0877)");

        RuleFor(x => x.PublisherName)
            .NotEmpty()
            .WithMessage("Publisher name is required")
            .MaximumLength(40)
            .WithMessage("Publisher name cannot exceed 40 characters")
            .MinimumLength(2)
            .WithMessage("Publisher name must be at least 2 characters");

        RuleFor(x => x.City)
            .MaximumLength(20)
            .WithMessage("City name cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.State)
            .Length(2)
            .WithMessage("State must be exactly 2 characters")
            .Must(state => ValidStates.Contains(state?.ToUpper()))
            .WithMessage("State must be a valid US state abbreviation (e.g., CA, NY, TX)")
            .When(x => !string.IsNullOrEmpty(x.State));

        RuleFor(x => x.Country)
            .MaximumLength(30)
            .WithMessage("Country name cannot exceed 30 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }
}
