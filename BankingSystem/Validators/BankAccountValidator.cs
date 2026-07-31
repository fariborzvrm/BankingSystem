using BankingSystem.Application.DTOs;
using FluentValidation;

namespace BankingSystem.API.Validators
{
    public class BankAccountValidator : AbstractValidator<BranchNameDto>
    {
        public BankAccountValidator() {

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(20).WithMessage("Branch name must not exceed 20 characters.")
                .MinimumLength(3).WithMessage("Branch name must be at least 3 characters long.");               

        }
    }
}
