

using Microsoft.EntityFrameworkCore;
using TodoList.Data;

namespace TodoList.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private TaskDbContext _dbContext;
        public AuthRepository( TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int?> GetUserAuth(string userToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.USerToken == userToken);
            if (user is null) return null;
            return user.Id;
            
        }
    }
}
