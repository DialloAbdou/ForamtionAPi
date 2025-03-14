using FluentValidation;
using TodoList.Dto;

namespace TodoList.Validations
{
    public class TaskValidation:AbstractValidator<TaskInputModel>
    {
        public TaskValidation()
        {
            RuleFor(t=>t.Title).NotEmpty();
        }
    }
}
