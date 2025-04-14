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

        public async Task<bool> DeleteTask(int id, Int32 userId )
        {
            var result = await _dbContext.Taskes.Where(t => t.Id == id && t.USerId==userId)
                            .ExecuteDeleteAsync();
            return result > 0;

        }

        public async Task<IEnumerable<MyTask>> GetAllTasks(Int32 userId)
        {
             return await _dbContext.Taskes.Where(t=>t.USerId==userId).ToListAsync();
            
        }

        public async Task<IEnumerable<MyTask>> GetTaskActive(Int32 userId)
        {
            var taskActives = await _dbContext.Taskes
                 .Where(t => t.EndDate == DateTime.UtcNow && t.USerId == userId)
                 .ToListAsync();
            return taskActives;
        }

        public async Task<MyTask?> GetTaskById(Int32 taskId, Int32 userId)
        {
            MyTask? task = await _dbContext.Taskes
                .FirstOrDefaultAsync(t => t.Id == taskId && t.USerId==userId);
            if (task is not null) return task;
            return null;
        }

        public async Task<bool> UpdateTaskAsync(Int32 id, Int32 userId, TaskInputModel taskInput)
        {
            var task = await _dbContext.Taskes
                .Where(t => t.Id == id && t.USerId == userId)
                .ExecuteUpdateAsync(t => t
                .SetProperty(t => t.Title, taskInput.Title)
                .SetProperty(t => t.StartDate, taskInput.StartDate)
                .SetProperty(t => t.EndDate, taskInput.EndDate));
            return task > 0;
        }

 
    }
}
