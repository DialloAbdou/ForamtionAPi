using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskOutPutModel>> GetAllTasks( Int32 userId);
        Task<TaskOutPutModel> GetTaskById(int taskId, Int32 userId);
        Task<TaskOutPutModel> AddTask(TaskInputModel taskInput, Int32 userId);
         Task<Boolean> UpdateTask(Int32 id, Int32 userId, TaskInputModel taskInput);
        Task<Boolean> DeleteTask(Int32 id, Int32 userId);
        Task<IEnumerable<TaskOutPutModel>> GetTaskActive(Int32 userId);
        Task<User?> GetUserAsync(String usertoken, Int32 userId);


            
    }
}
