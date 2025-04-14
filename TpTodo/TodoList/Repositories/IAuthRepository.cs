namespace TodoList.Repositories
{
    public interface IAuthRepository
    {
        Task<Int32?> GetUserAuth(String userToken);
    }
}
