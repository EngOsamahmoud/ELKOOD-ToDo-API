using FluentValidation;
using ELKOOD.ToDo.Application.DTOs.ToDo;

namespace ELKOOD.ToDo.Application.Validators
{
    public class UpdateToDoItemDtoValidator : AbstractValidator<UpdateToDoItemDto>
    {
        public UpdateToDoItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Category)
                .MaximumLength(50).WithMessage("Category cannot exceed 50 characters");
        }
    }
}