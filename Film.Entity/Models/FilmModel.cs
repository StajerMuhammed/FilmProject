using Film.DTOs.Movies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Film.Models
{
    public class FilmModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public Category? Kategori { get; set; } = null;
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Overview { get; set; } = null!;
        public double Rating { get; set; }
        public bool IsDeleted { get; set; }
        public Yönetmen Yönetmen { get; set; }
        [ForeignKey("Yönetmen")]
        public int YönetmenId { get; set; }

        public string ImageUrl { get; set; } = null!;

    }

}