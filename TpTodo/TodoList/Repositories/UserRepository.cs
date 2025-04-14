using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly TaskDbContext _dbContext;
        public UserRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(User input)
        {
            await _dbContext.AddAsync(input);
            await _dbContext.SaveChangesAsync();
            return input;

        }

        public async Task<bool> DeleteUserAsync(int id)
        {

            var result = await _dbContext.Users.Where(u => u.Id == id)
               .ExecuteDeleteAsync();
            return result > 0;

        }

        public async Task<bool> UpdateUserAsync(int id,UserInputModel user)
        {
            var resut = await _dbContext.Users.Where(u => u.Id == id)
             .ExecuteUpdateAsync(
              u =>
              u.SetProperty(u => u.Name, user.Name)
              .SetProperty(u => u.USerToken, user.UserToken));
             
            return resut > 0;
        }

        public async Task<User?> GetUserAsync(String usertoken)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.USerToken == usertoken);
            if (user != null) return user;
            return null;
        }



    }
}
