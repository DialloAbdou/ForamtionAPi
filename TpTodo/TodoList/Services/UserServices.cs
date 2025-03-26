using System.Threading.Tasks;
using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Services
{
    
    public class UserServices : IUserServices
    {
        private static List<User> _users;
        public UserServices()
        {
            _users = new List<User> 
            { 
                new User{ Id =1, Name= "Bah", USerToken= Guid.NewGuid().ToString() }
            };
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
        public  async Task<UserOutputModel> CreateUserAsync(UserInputModel input)
        {
            var usr = new User
            { 
                Id = _users.Max(x => x.Id)+1,
                Name = input.Name,
                USerToken =  Guid.NewGuid().ToString(),
            };
            _users.Add(usr);
            return  GetUserToUserOutput (usr);
        }

        public IEnumerable<UserOutputModel> GetAllUSerAsync()
        {
            return _users.ConvertAll(GetUserToUserOutput);
        }


        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<UserOutputModel> GetUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(UserInputModel input)
        {
            throw new NotImplementedException();
        }

      
    }
}
