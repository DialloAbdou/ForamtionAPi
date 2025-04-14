namespace TodoList.Services
{
    public interface IAuthService
    {
        Task<Int32?> GetUserAuth(HttpContext httpContext);
    }
}
