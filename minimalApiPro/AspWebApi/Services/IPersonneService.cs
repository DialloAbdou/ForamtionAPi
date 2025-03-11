using AspWebApi.Data.Models;
using AspWebApi.Dto;

namespace AspWebApi.Services
{
    public interface IPersonneService
    {
        Task<PersonOutputModel> AddPersonne(PersonInputModel personne);
        Task<Boolean> UpdatePersonne(Int32 id, PersonInputModel personne);
        Task<Boolean> DeletePersonne(Int32 id);
        Task<PersonOutputModel> GetPersonneById(Int32 id);
        Task<IEnumerable<PersonOutputModel>> GetAllPersonneAsync();

    }
}
