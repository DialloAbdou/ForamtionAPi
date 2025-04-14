using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private TaskDbContext _dbContext;
        public TaskRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MyTask> AddTaskAsync(MyTask taskInput)
        {
            await _dbContext.Taskes.AddAsync(taskInput);
             return taskInput;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var result = await _dbContext.Taskes.Where(t => t.Id == id)
                            .ExecuteDeleteAsync();
            return result > 0;

        }

        public async Task<IEnumerable<MyTask>> GetAllTasks()
        {
            var tasks = await _dbContext.Taskes.ToListAsync();
            return tasks;
        }

        public async Task<IEnumerable<MyTask>> GetTaskActive()
        {
            var taskActives = await _dbContext.Taskes
                 .Where(t => t.EndDate == DateTime.UtcNow)
                 .ToListAsync();
            return taskActives;
        }

        public async Task<MyTask?> GetTaskById(int taskId)
        {
            MyTask? task = await _dbContext.Taskes.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task is not null) return task;
            return null;
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskInputModel taskInput)
        {
            var task = await _dbContext.Taskes
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(t => t
                .SetProperty(t => t.Title, taskInput.Title)
                .SetProperty(t => t.StartDate, taskInput.StartDate)
                .SetProperty(t => t.EndDate, taskInput.EndDate));
            return task > 0;
        }

 
    }
}
