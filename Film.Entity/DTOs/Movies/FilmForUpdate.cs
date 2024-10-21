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
        [MaxLength(250)]
        public string Overview { get; set; } = null!;
        public double Rating { get; set; }
        public bool IsDeleted { get; set; }

        // Yeni eklenen alan
        [Required]
        public int YönetmenId { get; set; }
    }
}
