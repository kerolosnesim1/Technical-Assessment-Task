using FluentValidation;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Validators.Tasks;

public class UpdateTaskStatusDtoValidator : AbstractValidator<UpdateTaskStatusDto>
{
    public UpdateTaskStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Task status is required")
            .IsInEnum().WithMessage("Task status must be a valid value (Todo, InProgress, Done)");
    }
}