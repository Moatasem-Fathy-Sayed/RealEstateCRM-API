using FluentValidation;
using RealEstateCRM.Core.DTOs;

namespace RealEstateCRM.Core.Validators
{
    public class CreateInteractionLogDtoValidator : AbstractValidator<CreateInteractionLogDto>
    {
        public CreateInteractionLogDtoValidator()
        {
            RuleFor(x => x.LeadId)
                .GreaterThan(0).WithMessage("Valid Lead ID is required.");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Interaction notes cannot be empty.")
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid interaction type.");
        }
    }
}