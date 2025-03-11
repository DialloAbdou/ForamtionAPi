using AspWebApi.Data;
using AspWebApi.Data.Models;
using AspWebApi.Dto;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AspWebApi.Services
{
    public class PersonneService:IPersonneService
    {
        ApiDbContext _dbcontext;
        public PersonneService(ApiDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<PersonOutputModel> AddPersonne(PersonInputModel personne)
        {
            var person = new Personne
            { 
                Nom = personne.Nom,
                Prenom = personne.Prenom,
                Birthday = personne.BirthDay.GetValueOrDefault(),

            };
            await _dbcontext.Personnes.AddAsync(person);
            await _dbcontext.SaveChangesAsync();
            return ToOutPutModelPerson(person);
            //return new PersonOutputModel
            //             (
            //                  person.Id,
            //                 $"{person.Nom} {person.Prenom}",
            //                  person.Birthday == DateTime.MinValue ? null : person.Birthday
            //              );
        }

        public async Task<bool> DeletePersonne(Int32 id)
        { 
            var result = await _dbcontext.Personnes
                 .Where(p => p.Id == id)
                 .ExecuteDeleteAsync();
            if (result > 0) return true;
            return false;
        }

        public async Task<IEnumerable<PersonOutputModel>> GetAllPersonneAsync()
        {
            return (await _dbcontext.Personnes.ToListAsync()).ConvertAll(ToOutPutModelPerson);
        }

        public async Task<PersonOutputModel> GetPersonneById(int id)
        {
            var person = await _dbcontext.Personnes
                                .Where(predicate => predicate.Id == id)
                                .FirstOrDefaultAsync();

            if (person == null) return null;
            return ToOutPutModelPerson(person) ;
        }

        public async Task<bool> UpdatePersonne(Int32 id, PersonInputModel personne)
        {
            var result = await _dbcontext.Personnes
                                    .Where(p => p.Id == id)
                                    .ExecuteUpdateAsync(p => p.SetProperty(p => p.Nom, personne.Nom)
                                    .SetProperty(p=>p.Prenom, personne.Prenom)
                                    .SetProperty(p=>p.Birthday, personne.BirthDay));  
            if (result > 0) return true;
            return false;
        }

        private  PersonOutputModel ToOutPutModelPerson( Personne person )
        {
           return  new PersonOutputModel
                             (
                                  person.Id,
                                 $"{person.Nom} {person.Prenom}",
                                  person.Birthday == DateTime.MinValue ? null : person.Birthday
                              );
        }


    }
}
