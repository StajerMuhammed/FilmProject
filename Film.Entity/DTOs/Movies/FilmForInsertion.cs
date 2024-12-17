using System.ComponentModel.DataAnnotations;

namespace Film.DTOs.Movies
{
    public class FilmForInsertion
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Bu alan zorunludur")]
        [MaxLength(500)]
        public string Overview { get; set; } = null!;
        public double Rating { get; set; }

        // Yeni eklenen alan

        public int YönetmenId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public decimal Price { get; set; } = 0.0m;
    }
}
