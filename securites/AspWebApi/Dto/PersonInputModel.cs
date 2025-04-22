namespace AspWebApi.Dto
{
    public class PersonInputModel
    {
        public String Nom { get; set; } = string.Empty;
        public String Prenom { get; set; } = string.Empty ;
        public DateTime? BirthDay { get; set; }
        public String Adress { get; set; } = string.Empty;
    }
}
