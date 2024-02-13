using System.Data;
using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
    {
        public UpdateTaskItemCommandValidator() 
        {
            {
                RuleFor(p => p.TaskName) 
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
                RuleFor(p => p.Description)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(1500).WithMessage("{PropertyName} must not exceed 1500 characters.");
                RuleFor(p => p.CreatedBy)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
                RuleFor(p => p.State)
                    .IsInEnum()
                    .WithMessage("{PropertyName} must be a valid value between 1-3");
            }
        }
    }
}
