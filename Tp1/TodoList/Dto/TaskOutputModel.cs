namespace TodoList.Dto
{
    public record TaskOutputModel
        (
            Int32 Id, 
            String Title,
            DateTime  StarteDate,
            DateTime EndDate
        );
    
    
}
