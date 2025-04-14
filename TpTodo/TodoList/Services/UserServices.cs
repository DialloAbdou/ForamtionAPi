using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Data.Model;
using TodoList.Dto;
using TodoList.Repositories;

namespace TodoList.Services
{
    
    public class UserServices : IUserServices
    {
        private IUserRepository _userRepository;

        public UserServices( IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserOutputModel> CreateUserAsync(UserInputModel input)
        {
            User user = new User
            {
                Name = input.Name,
                USerToken = Guid.NewGuid().ToString(),
            };
           await _userRepository.CreateUserAsync(user);
            return this.GetUserToUserOutput(user);
        }

        public Task<bool> DeleteUserAsync(int id)
        {
          return _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> UpdateUserAsync(int id, UserInputModel input)
        {
            return await _userRepository.UpdateUserAsync(id, input);
        }

        private  UserOutputModel GetUserToUserOutput(User user)
        {
           return  new UserOutputModel
            (
                user.Id,
                user.Name,
                user.USerToken
            );

        }


    }
}
