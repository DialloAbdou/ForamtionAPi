namespace TodoList.Data.Model
{
    public class User
    {
        public Int32 Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String USerToken { get; set; } = String.Empty;
        public List<MyTask> Tasks { get; set; } = new List<MyTask>();

    }
}
