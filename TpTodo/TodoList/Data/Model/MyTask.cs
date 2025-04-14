namespace TodoList.Data.Model
{
    public class MyTask
    {
        public Int32 Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32 USerId { get; set; }
        public User User { get; set; }

    }
}
