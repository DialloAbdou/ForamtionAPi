namespace TodoList.Dto
{
    public class TaskInputModel
    {
        public String Title { get; set; } = String.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
