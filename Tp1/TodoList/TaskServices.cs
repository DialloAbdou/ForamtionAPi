namespace TodoList
{
    public class TaskServices
    {
        private List<MyTask> tasks = new List<MyTask>()
        {
            new MyTask{ Id =1, Title ="apprendre c#", StartDate=DateTime.Now},
            new MyTask{ Id =2, Title ="formation js", StartDate=DateTime.Now}

        };

        public List<MyTask> GetAll()
        {

            return tasks;
        }

        public MyTask? GetbyId(Int32 id)
        {
            var task = tasks.Find(x => x.Id == id);
            if (task is not null) return task;
            return null;
        }

        public MyTask AddTask(String title)
        {
            MyTask task = new MyTask { Id = tasks.Max(t => t.Id) + 1, StartDate = DateTime.Now, Title = title };
            tasks.Add(task);
            return task;
        }

        public MyTask UpdateTask(Int32 id, MyTask task)
        {
            var taksOld = tasks.Find(t => t.Id == id);
            if (taksOld is not null)
            {
                taksOld.Title = task.Title;
                taksOld.StartDate = task.StartDate;
                taksOld.EndDate = task.EndDate;
            }
            return taksOld;
        }

        public List<MyTask> ActivesTask()
        {
            var taskActives = tasks.Select(a=>a).Where(t=>t.EndDate is null).ToList();
            return taskActives;
        }

        public Boolean IsDeleted(Int32 id)
        {
            var task = tasks.Find(t=>t.Id == id);   

            if( task is null) return false;
            tasks.Remove(task);
            return true;
        }
    }
}
