using FluentValidation;
using TodoList.Dto;

namespace TodoList.Validations
{
    public class UserValidation:AbstractValidator<UserInputModel>
    {
        public UserValidation()
        {
            RuleFor(u=>u.Name).NotEmpty();
        }
    }
}
