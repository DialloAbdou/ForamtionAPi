using TodoList.Dto;

namespace TodoList.Services
{
    public interface IUserServices
    {
        Task<UserOutputModel> CreateUserAsync(UserInputModel input);
        Task<Boolean> UpdateUserAsync(UserInputModel input);
        Task<Boolean> DeleteUserAsync(Int32 id);
        Task<UserOutputModel> GetUserAsync(Int32 id);
        IEnumerable<UserOutputModel> GetAllUSerAsync();
    }
}
