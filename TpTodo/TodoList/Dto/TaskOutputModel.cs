namespace TodoList.Dto
{
    public record TaskOutPutModel
        (
            Int32 Id, 
            String Title,
            DateTime  StarteDate,
            DateTime ? EndDate
        );
    
    
}
