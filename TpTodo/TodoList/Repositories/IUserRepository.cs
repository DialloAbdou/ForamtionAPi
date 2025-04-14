using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User input);
        Task<Boolean> UpdateUserAsync(int id,UserInputModel user );
        Task<Boolean> DeleteUserAsync(Int32 id);
        Task<User?> GetUserAsync(String usertoken);


    }
}
