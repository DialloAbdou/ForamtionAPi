using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskOutPutModel>> GetAllTasks();
        Task<TaskOutPutModel> GetTaskById(int taskId);
        Task<TaskOutPutModel> AddTask(TaskInputModel taskInput);
         Task<Boolean> UpdateTask(Int32 id, TaskInputModel taskInput);
        Task<Boolean> DeleteTask(Int32 id);
        Task<IEnumerable<TaskOutPutModel>> GetTaskActive();



    }
}
