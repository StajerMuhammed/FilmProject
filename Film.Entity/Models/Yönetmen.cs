using System.ComponentModel.DataAnnotations.Schema;

namespace Film.Models
{
    public class Yönetmen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BirtDay { get; set; }

        public bool IsDeleted { get; set; }

        [InverseProperty("Yönetmen")]
        public virtual ICollection<FilmModel> Filmler { get; set; } 


    }
}