using TodoList.Dto;

namespace TodoList
{
    public class TaskServices
    {
        private List<MyTask> tasks = new List<MyTask>()
        {
            new MyTask{ Id =1, Title ="apprendre c#", StartDate=DateTime.Now},
            new MyTask{ Id =2, Title ="formation js", StartDate=DateTime.Now}

        };

        public List<TaskOutputModel> GetAll()
        {
            
            return tasks.ConvertAll(GetTaskToOutPutModel);
        }

        public TaskOutputModel? GetbyId(Int32 id)
        {
            var task = tasks.Find(x => x.Id == id);
            if (task is not null) return GetTaskToOutPutModel(task);
            return null;
        }

        public TaskOutputModel AddTask(String title)
        {
            MyTask task = new MyTask { Id = tasks.Max(t => t.Id) + 1, StartDate = DateTime.Now, Title = title };
            tasks.Add(task);
            return GetTaskToOutPutModel(task);
        }

        public TaskOutputModel UpdateTask(Int32 id, TaskInputModel task)
        {
            var taksOld = tasks.Find(t => t.Id == id);
            if (taksOld is not null)
            {
                taksOld.Title = task.Title;
                taksOld.StartDate = task.StartDate.GetValueOrDefault();
                taksOld.EndDate = task.EndDate == null ? null : task.EndDate.GetValueOrDefault();
                return GetTaskToOutPutModel(taksOld);
            }
            return null;
         
        }

        public List<TaskOutputModel> ActivesTask()
        {
            var taskActives = (tasks.Select(a=>a).Where(t=>t.EndDate is null).ToList()).ConvertAll(GetTaskToOutPutModel);
            return taskActives;
        }

        public Boolean IsDeleted(Int32 id)
        {
            var task = tasks.Find(t=>t.Id == id);   

            if( task is null) return false;
            tasks.Remove(task);
            return true;
        }
        private TaskOutputModel GetTaskToOutPutModel(MyTask task)
        {
            return new TaskOutputModel
                (
                    task.Id,
                    task.Title,
                    task.StartDate,
                    task.EndDate.GetValueOrDefault()
                ); 
        }
    }
}
