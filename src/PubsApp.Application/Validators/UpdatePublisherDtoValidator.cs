using FluentValidation;
using PubsApp.Application.DTOs;

namespace PubsApp.Application.Validators;

/// <summary>
/// Validator for UpdatePublisherDto
/// Enforces business rules for updating existing publishers
/// </summary>
public class UpdatePublisherDtoValidator : AbstractValidator<UpdatePublisherDto>
{
    private static readonly string[] ValidStates = 
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
        "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
        "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
        "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
    };

    public UpdatePublisherDtoValidator()
    {
        RuleFor(x => x.PublisherName)
            .NotEmpty()
            .WithMessage("Publisher name is required")
            .MaximumLength(40)
            .WithMessage("Publisher name cannot exceed 40 characters")
            .MinimumLength(2)
            .WithMessage("Publisher name must be at least 2 characters")
            .When(x => !string.IsNullOrEmpty(x.PublisherName));

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
