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

        public async Task<IEnumerable<TaskOutPutModel>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();
            return tasks.ToList().ConvertAll(GetTaskToOutPutModel);
     

        }

        public async Task<TaskOutPutModel> GetTaskById(int taskId)
        {
     
            var task =  await _taskRepository.GetTaskById(taskId);
            return GetTaskToOutPutModel(task);

        }

        public async Task<TaskOutPutModel> AddTask(TaskInputModel taskInput)
        {
            var task = new MyTask
            {
                Title = taskInput.Title,
                StartDate = DateTime.Now,
            };
            await _taskRepository.AddTaskAsync(task);
            return GetTaskToOutPutModel(task);
        }

        public async Task<bool> DeleteTask(int id)
        {
           return   await _taskRepository.DeleteTask(id);
        }

        public async Task<IEnumerable<TaskOutPutModel>> GetTaskActive()
        {
            var taskActives =await _taskRepository.GetTaskActive();
            return taskActives.ToList().ConvertAll(GetTaskToOutPutModel);
        }

        public async Task<bool> UpdateTask(int id, TaskInputModel taskInput)
        {
            return await _taskRepository.UpdateTaskAsync(id, taskInput);
        }

        public Task<User?> GetUserAsync(string usertoken)
        {
            return _userRepository.GetUserAsync(usertoken);
        }
    }
}
