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
                    .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");
                RuleFor(p => p.Deadline)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull();
                RuleFor(p => p.CreatedBy)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
                RuleFor(p => p.State)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull();
            }
        }
    }
}
