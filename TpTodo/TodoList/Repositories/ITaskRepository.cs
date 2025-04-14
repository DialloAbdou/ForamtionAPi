using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Repositories
{
    public interface ITaskRepository
    {

        Task<IEnumerable<MyTask>> GetAllTasks(Int32 userId);
        Task<MyTask> GetTaskById(Int32 taskId, Int32 userId);
        Task<MyTask> AddTaskAsync(MyTask taskInput);
        Task<Boolean> UpdateTaskAsync(Int32 id, Int32 userId, TaskInputModel taskInput);
        Task<Boolean> DeleteTask(Int32 id, Int32 userId);
        Task<IEnumerable<MyTask>> GetTaskActive(Int32 userId);

    }
}
