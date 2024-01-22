using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.TaskItems.Commands.AssignTaskItemToUser
{
    public class AssignTaskItemToUserCommandValidator : AbstractValidator<AssignTaskItemToUserCommand>
    {
        public AssignTaskItemToUserCommandValidator()
        {
            RuleFor(x => x.TaskItemId).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
