using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Projects.Commands.DeleteUserFromProject
{
    public class DeleteUserFromProjectCommandValidator : AbstractValidator<DeleteUserFromProjectCommand>
    {
        public DeleteUserFromProjectCommandValidator()
        {
            RuleFor(p => p.OwnerUsername)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}