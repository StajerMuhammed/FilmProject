namespace Film.DTOs.Yönetmen
{
    public class YönetmenDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime BirtDay { get; set; }
        public virtual ICollection<string>? FilmAdları { get; set; }
    }
}
