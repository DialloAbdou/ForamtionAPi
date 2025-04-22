namespace AspWebApi.Data.Models
{
    public class Personne
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public string DisplayId { get; set; } = string.Empty;

    }
}
