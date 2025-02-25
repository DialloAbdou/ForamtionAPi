using AspWebApi.Data.Models;
using FluentValidation;

namespace AspWebApi.Validations
{
    public class PersonneValidation : AbstractValidator<Personne>
    {
        public PersonneValidation()
        {
            RuleFor(p => p.Nom).NotEmpty();
            RuleFor(p => p.Prenom).NotEmpty();

        }
    }
}
