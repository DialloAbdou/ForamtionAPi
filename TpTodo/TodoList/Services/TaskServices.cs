using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Data.Model;
using TodoList.Dto;

namespace TodoList.Services
{
    public class TaskServices : ITaskService
    {
        private TaskDbContext _dbContext;
        public TaskServices(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //private List<MyTask> tasks = new List<MyTask>()
        //{
        //    new MyTask{ Id =1, Title ="apprendre c#", StartDate=DateTime.Now},
        //    new MyTask{ Id =2, Title ="formation js", StartDate=DateTime.Now}

        //};

        //public List<TaskOutputModel> GetAll()
        //{

        //    return tasks.ConvertAll(GetTaskToOutPutModel);
        //}

        //public TaskOutputModel? GetbyId(Int32 id)
        //{
        //    var task = tasks.Find(x => x.Id == id);
        //    if (task is not null) return GetTaskToOutPutModel(task);
        //    return null;
        //}

        //public TaskOutputModel AddTask(String title)
        //{
        //    MyTask task = new MyTask { Id = tasks.Max(t => t.Id) + 1, StartDate = DateTime.Now, Title = title };
        //    tasks.Add(task);
        //    return GetTaskToOutPutModel(task);
        //}

        //public TaskOutputModel UpdateTask(Int32 id, TaskInputModel task)
        //{
        //    var taksOld = tasks.Find(t => t.Id == id);
        //    if (taksOld is not null)
        //    {
        //        taksOld.Title = task.Title;
        //        taksOld.StartDate = task.StartDate.GetValueOrDefault();
        //        taksOld.EndDate = task.EndDate == null ? null : task.EndDate.GetValueOrDefault();
        //        return GetTaskToOutPutModel(taksOld);
        //    }
        //    return null;

        //}

        //public List<TaskOutputModel> ActivesTask()
        //{
        //    var taskActives = (tasks.Select(a => a).Where(t => t.EndDate is null).ToList()).ConvertAll(GetTaskToOutPutModel);
        //    return taskActives;
        //}

        //public Boolean IsDeleted(Int32 id)
        //{
        //    var task = tasks.Find(t => t.Id == id);

        //    if (task is null) return false;
        //    tasks.Remove(task);
        //    return true;
        //}
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
            return (await _dbContext.Taskes
                 .ToListAsync()).ConvertAll(GetTaskToOutPutModel);

        }

        public async Task<TaskOutPutModel> GetTaskById(int taskId)
        {
            var task = await _dbContext.Taskes.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task is null) return null;
            return GetTaskToOutPutModel(task);

        }

        public async Task<TaskOutPutModel> AddTask(TaskInputModel taskInput)
        {
            var task = new MyTask
            {
                Title = taskInput.Title,
                StartDate = DateTime.Now,
            };
            await _dbContext.AddAsync(task);
            await _dbContext.SaveChangesAsync();
            return GetTaskToOutPutModel(task);
        }

        public async Task<bool> DeleteTask(int id)
        {
            var result = await _dbContext.Taskes
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<TaskOutPutModel>> GetTaskActive()
        {
            var taskActives = await _dbContext.Taskes
                         .Select(t => t)
                         .Where(t => t.EndDate != null)
                        .ToListAsync();
            return taskActives.ConvertAll(GetTaskToOutPutModel);
        }

        public async Task<bool> UpdateTask(int id, TaskInputModel taskInput)
        {
            var result = await _dbContext.Taskes
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(t => t
                        .SetProperty(t => t.Title, taskInput.Title)
                        .SetProperty(t => t.StartDate, taskInput.StartDate)
                        .SetProperty(t => t.EndDate, taskInput.EndDate));
            return result > 0;

        }


    }
}
