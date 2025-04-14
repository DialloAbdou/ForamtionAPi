using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Repositories
{
    public interface ITaskRepository
    {

        Task<IEnumerable<MyTask>> GetAllTasks();
        Task<MyTask> GetTaskById(int taskId);
        Task<MyTask> AddTaskAsync(MyTask taskInput);
        Task<Boolean> UpdateTaskAsync(Int32 id, TaskInputModel taskInput);
        Task<Boolean> DeleteTask(Int32 id);
        Task<IEnumerable<MyTask>> GetTaskActive();

    }
}
