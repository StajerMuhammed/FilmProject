using Film.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; } // Sipariş ID
        public int FilmId { get; set; } // FilmId
        public string FilmName { get; set; } // Film ismi
        public int CategoryId { get; set; }
        public string? KategoriTürü { get; set; } // Kategori bilgisi

        public int YönetmenId { get; set; }
        public string? YönetmenName { get; set; }
        public string Overview { get; set; } = null!;

        public double Rating { get; set; }

        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; } = 0.0m;

        public string Status { get; set; } = "Sepette"; // Enum değerini string olarak döndürüyoruz
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Sipariş Tarihi
    }
}
