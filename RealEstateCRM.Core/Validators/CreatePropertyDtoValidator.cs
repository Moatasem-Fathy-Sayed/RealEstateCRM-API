using FluentValidation;
using RealEstateCRM.Core.DTOs;

namespace RealEstateCRM.Core.Validators
{
    /// <summary>
    /// Validator class for CreatePropertyDto ensuring all required real estate fields comply with business rules.
    /// Inherits from AbstractValidator of FluentValidation framework.
    /// </summary>
    public class CreatePropertyDtoValidator : AbstractValidator<CreatePropertyDto>
    {
        public CreatePropertyDtoValidator()
        {
            // Title validation rules: must not be empty and should have reasonable length
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Property title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            // Description validation rules: maximum allowed length configuration
            RuleFor(p => p.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

            // Financial Price validation rules: must be strictly greater than zero
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Property price must be greater than zero.");

            // Area Size validation rules: property area size must be positive
            RuleFor(p => p.AreaSize)
                .GreaterThan(0).WithMessage("Area size must be greater than zero square meters.");

            // Bedroom count validation rules: non-negative constraint
            RuleFor(p => p.Bedrooms)
                .GreaterThanOrEqualTo(0).WithMessage("Bedrooms count cannot be negative.");

            // Bathroom count validation rules: non-negative constraint
            RuleFor(p => p.Bathrooms)
                .GreaterThanOrEqualTo(0).WithMessage("Bathrooms count cannot be negative.");

            // Address validation rules: required field for physical property location
            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Property address is required.")
                .MaximumLength(300).WithMessage("Address cannot exceed 300 characters.");

            // City validation rules: required field for location sorting
            RuleFor(p => p.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.");

            // Enum PropertyType validation: ensure valid enum value is provided
            RuleFor(p => p.Type)
                .IsInEnum().WithMessage("Invalid property type selected.");
        }
    }
}