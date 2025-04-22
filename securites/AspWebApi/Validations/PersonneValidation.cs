using AspWebApi.Data.Models;
using AspWebApi.Dto;
using FluentValidation;

namespace AspWebApi.Validations
{
    public class PersonneValidation : AbstractValidator<PersonInputModel>
    {
        public PersonneValidation()
        {
            RuleFor(p => p.Nom).NotEmpty();
            RuleFor(p => p.Prenom).NotEmpty();

        }
    }
}
