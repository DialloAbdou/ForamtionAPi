namespace TodoList
{
    public class MyTask
    {
        public int Id { get; set; }
        public String Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime?EndDate { get; set; }

    }
}
