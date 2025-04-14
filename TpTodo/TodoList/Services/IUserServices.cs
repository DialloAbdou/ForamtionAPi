using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Services
{
    public interface IUserServices
    {
        Task<UserOutputModel> CreateUserAsync(UserInputModel input);
        Task<Boolean> UpdateUserAsync(int id,UserInputModel input);
        Task<Boolean> DeleteUserAsync(Int32 id);
    

    }
}
