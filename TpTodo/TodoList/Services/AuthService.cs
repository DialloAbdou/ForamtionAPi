using TodoList.Repositories;

namespace TodoList.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService( IAuthRepository authRepository)
        {
            _authRepository = authRepository;      
        }

        public async Task<int?> GetUserAuth(HttpContext httpContext)
        {
            var usertoken = httpContext.Request.Headers["UserToken"].ToString();
            return await _authRepository.GetUserAuth(usertoken);
        }
    }
}
