using System.ComponentModel.DataAnnotations;

namespace Film.DTOs.Movies
{
    public class FilmForUpdate
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(500)]
        public string Overview { get; set; } = null!;
        public double Rating { get; set; }
        public bool IsDeleted { get; set; }

        public int YönetmenId { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
