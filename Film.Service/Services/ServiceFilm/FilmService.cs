using Film.Datas; // DbContext sınıfı
using Film.DTOs.Category;
using Film.DTOs.Movies;
using Film.Models; // Modeller
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                .Include(y => y.Yönetmen)
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
                throw new ArgumentException("İsim alanı zorunludur."); // Hata kontrolü
            }

            var film = new Models.FilmModel
            {
                Name = filmForInsertion.Name,
                CategoryId = filmForInsertion.CategoryId,
                YönetmenId = filmForInsertion.YönetmenId, 
                Overview = filmForInsertion.Overview,
                Rating = filmForInsertion.Rating,
            };

            _context.Films.Add(film); // Yeni kategori ekle
            _context.SaveChanges(); // Değişiklikleri kaydet

            return film;
        }

        public Models.FilmModel UpdateFilm(FilmForUpdate filmForUpdate)
        {
            var existingFilm = GetFilmById(filmForUpdate.Id);

            if (existingFilm == null)
            {
                throw new KeyNotFoundException($"Film ID {filmForUpdate.Id} bulunamadı."); // Hata kontrolü
            }

            existingFilm.Name = filmForUpdate.Name;
            existingFilm.Overview = filmForUpdate.Overview;
            existingFilm.CategoryId = filmForUpdate.CategoryId;
            existingFilm.YönetmenId = filmForUpdate.YönetmenId;
            existingFilm.Rating = filmForUpdate.Rating;
            existingFilm.IsDeleted = filmForUpdate.IsDeleted; // Tür güncelle

            _context.SaveChanges(); // Değişiklikleri kaydet

            return existingFilm; // Güncellenen kategoriyi döndür
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
    }
}
