using AspWebApi.Data.Models;
using AspWebApi.Dto;

namespace AspWebApi.Services
{
    public interface IPersonneService
    {
        Task<PersonOutputModel> AddPersonne(PersonInputModel personne);
        Task<Boolean> UpdatePersonne(string id, PersonInputModel personne);
        Task<Boolean> DeletePersonne(String id);
        Task<PersonOutputModel> GetPersonneById(String id);
        Task<IEnumerable<PersonOutputModel>> GetAllPersonneAsync();

    }
}
