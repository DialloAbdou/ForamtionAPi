using System.Runtime.CompilerServices;

namespace TodoList.Endpoints
{
    public  static class TaskEndpoints
    {
        public static RouteGroupBuilder GetTaskEndpoints(this RouteGroupBuilder groupe)
        {
            groupe.MapGet("/", (TaskServices taskServices) => taskServices.GetAll());
            return groupe;
        }

     
    }
}
