using FluentValidation;
using PubsApp.Application.DTOs;

namespace PubsApp.Application.Validators;

/// <summary>
/// Validator for UpdateAuthorDto
/// </summary>
public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
{
    public UpdateAuthorDtoValidator()
    {
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(40).WithMessage("Last name must not exceed 40 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(20).WithMessage("First name must not exceed 20 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .Length(12).WithMessage("Phone number must be exactly 12 characters")
            .Matches(@"^\d{3}-\d{3}-\d{4}$").WithMessage("Phone number must be in format ###-###-####");

        RuleFor(x => x.Address)
            .MaximumLength(40).WithMessage("Address must not exceed 40 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.City)
            .MaximumLength(20).WithMessage("City must not exceed 20 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.City));

        RuleFor(x => x.State)
            .Length(2).WithMessage("State must be exactly 2 characters")
            .Matches(@"^[A-Z]{2}$").WithMessage("State must be 2 uppercase letters")
            .When(x => !string.IsNullOrWhiteSpace(x.State));

        RuleFor(x => x.Zip)
            .Length(5).WithMessage("ZIP code must be exactly 5 characters")
            .Matches(@"^\d{5}$").WithMessage("ZIP code must be 5 digits")
            .When(x => !string.IsNullOrWhiteSpace(x.Zip));
    }
}
