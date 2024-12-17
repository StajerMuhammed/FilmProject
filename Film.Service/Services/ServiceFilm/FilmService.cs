using Film.Datas; // DbContext sınıfı
using Film.DTOs.Category;
using Film.DTOs.Movies;
using Film.Models; // Modeller
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Film.Services.ServiceFilm
{
    public class FilmService : IFilmService
    {
        private readonly SampleDBContext _context;

        public FilmService(SampleDBContext context)
        {
            _context = context;
        }

        public IEnumerable<FilmModel> GetAllFilms()
        {
            return _context.Films
                .Include(f => f.Kategori) // Kategoriyi dahil et
                .Include(x => x.Yönetmen)
                .Where(c => !c.IsDeleted)
                .ToList();
        }

        public FilmModel GetFilmById(int id)
        {
            var film = _context.Films
                .Include(f => f.Kategori)  // Kategori ilişkisini dahil et
                .Include(f => f.Yönetmen)  // Yönetmen ilişkisini dahil et
                .FirstOrDefault(f => f.Id == id && !f.IsDeleted);  // Silinmemiş filmi bul

            if (film == null)
            {
                throw new Exception("Film bulunamadı");
            }

            return film;  // FilmModel olarak döndür
        }

        public Models.FilmModel CreateFilm(FilmForInsertion filmForInsertion)
        {
            if (string.IsNullOrWhiteSpace(filmForInsertion.Name))
            {
                throw new ArgumentException("İsim alanı zorunludur.");
            }

            // Resim URL'si doğrulama
            if (string.IsNullOrEmpty(filmForInsertion.ImageUrl) ||
                !Uri.IsWellFormedUriString(filmForInsertion.ImageUrl, UriKind.Absolute) ||
                !IsValidImageUrl(filmForInsertion.ImageUrl) ||
                !IsImageAccessible(filmForInsertion.ImageUrl).Result)  // Resmin geçerliliğini kontrol et
            {
                throw new ArgumentException("Geçersiz veya uygun olmayan resim URL'si.");
            }

            var film = new Models.FilmModel
            {
                Name = filmForInsertion.Name,
                CategoryId = filmForInsertion.CategoryId,
                YönetmenId = filmForInsertion.YönetmenId,
                Overview = filmForInsertion.Overview,
                Rating = filmForInsertion.Rating,
                ImageUrl = filmForInsertion.ImageUrl,
                Price   = filmForInsertion.Price
            };

            _context.Films.Add(film);
            _context.SaveChanges();

            return film;
        }

        public Models.FilmModel UpdateFilm(FilmForUpdate filmForUpdate)
        {
            var existingFilm = GetFilmById(filmForUpdate.Id);

            if (existingFilm == null)
            {
                throw new KeyNotFoundException($"Film ID {filmForUpdate.Id} bulunamadı.");
            }

            // Resim URL'si doğrulama
            if (string.IsNullOrEmpty(filmForUpdate.ImageUrl) ||
                !Uri.IsWellFormedUriString(filmForUpdate.ImageUrl, UriKind.Absolute) ||
                !IsValidImageUrl(filmForUpdate.ImageUrl) ||
                !IsImageAccessible(filmForUpdate.ImageUrl).Result)  // Resmin geçerliliğini kontrol et
            {
                throw new ArgumentException("Geçersiz veya uygun olmayan resim URL'si.");
            }

            existingFilm.Name = filmForUpdate.Name;
            existingFilm.Overview = filmForUpdate.Overview;
            existingFilm.CategoryId = filmForUpdate.CategoryId;
            existingFilm.YönetmenId = filmForUpdate.YönetmenId;
            existingFilm.Rating = filmForUpdate.Rating;
            existingFilm.IsDeleted = filmForUpdate.IsDeleted;
            existingFilm.ImageUrl = filmForUpdate.ImageUrl;
            existingFilm.Price = filmForUpdate.Price;

            _context.SaveChanges();

            return existingFilm;
        }

        public void DeleteFilm(int id)
        {
            var film = GetFilmById(id);
            if (film == null)
            {
                throw new KeyNotFoundException($"Film ID {id} bulunamadı."); // Hata kontrolü
            }

            film.IsDeleted = true; // Silinmiş olarak işaretle
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        // Resim URL'sinin doğru uzantıya sahip olup olmadığını kontrol et
        private bool IsValidImageUrl(string url)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".svg" };
            return validExtensions.Any(ext => url.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        // URL'nin gerçekten geçerli bir resme yönlendirip yönlendirmediğini kontrol et
        private async Task<bool> IsImageAccessible(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        // Resim dosyasının içeriği başarıyla alındıysa, bunu resim olarak kabul edebiliriz
                        var contentType = response.Content.Headers.ContentType.MediaType;
                        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                }
            }
            catch
            {
                return false; // URL erişilemiyorsa geçersiz kabul et
            }
        }
    }
}
