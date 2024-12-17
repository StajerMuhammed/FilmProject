using System.ComponentModel.DataAnnotations;
using Film.DTOs.Category; // CategoriDTO'yu ekle

namespace Film.DTOs.Movies
{
    public class FilmDTO
    {
        public int Id { get; set; }


        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }
        public string? KategoriTürü { get; set; } // Kategori bilgisi

        public int YönetmenId { get; set; }
        public string? YönetmenName { get; set; }
        public string Overview { get; set; } = null!;

        public double Rating { get; set; }

        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; } = 0.0m;

    }
}