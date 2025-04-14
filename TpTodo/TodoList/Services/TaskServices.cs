using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Data.Model;
using TodoList.Dto;
using TodoList.Repositories;

namespace TodoList.Services
{
    public class TaskServices : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
    
        public TaskServices( ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;   
        }

        private TaskOutPutModel GetTaskToOutPutModel(MyTask task)
        {
            return new TaskOutPutModel
                (
                    task.Id,
                    task.Title,
                    task.StartDate,
                    task.EndDate
                );
        }

        public async Task<IEnumerable<TaskOutPutModel>> GetAllTasks(Int32 userId)
        {
            var tasks = await _taskRepository.GetAllTasks(userId);
            return tasks.ToList().ConvertAll(GetTaskToOutPutModel);
     

        }

        public async Task<TaskOutPutModel> GetTaskById(int taskId, Int32 userId)
        {
     
            var task =  await _taskRepository.GetTaskById(taskId, userId);
            return GetTaskToOutPutModel(task);

        }

        public async Task<TaskOutPutModel> AddTask(TaskInputModel taskInput, Int32 userId)
        {
            var task = new MyTask
            {
                USerId = userId,
                Title = taskInput.Title,
                StartDate = DateTime.Now,
            };
            await _taskRepository.AddTaskAsync(task);
            return GetTaskToOutPutModel(task);
        }

        public async Task<bool> DeleteTask(int id, Int32 userId)
        {
           return   await _taskRepository.DeleteTask(id, userId);
        }

        public async Task<IEnumerable<TaskOutPutModel>> GetTaskActive(Int32 userId)
        {
            var taskActives =await _taskRepository.GetTaskActive(userId);
            return taskActives.ToList().ConvertAll(GetTaskToOutPutModel);
        }

        public async Task<bool> UpdateTask(int id, Int32 userId, TaskInputModel taskInput)
        {
            return await _taskRepository.UpdateTaskAsync(id, userId, taskInput);
        }

        public Task<User?> GetUserAsync(string usertoken, Int32 userId)
        {
            return _userRepository.GetUserAsync(usertoken);
        }
    }
}
