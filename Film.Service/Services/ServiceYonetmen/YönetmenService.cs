using Film.Datas;
using Film.DTOs.Category;
using Film.DTOs.Yönetmen;
using Film.Models;
using Film.Services.ServiceFilm;
using Microsoft.EntityFrameworkCore;

namespace Film.Services.ServiceYonetmen
{
    public class YönetmenService : IYonetmenService
    { 
        private readonly SampleDBContext _context;
        private readonly IFilmService _filmservice;

        public YönetmenService(SampleDBContext context, IFilmService filmservice)
        {
            _context = context;
            _filmservice = filmservice;
        }

        public Yönetmen CreatYonetmen(YönetmenForInsertion yönetmenForInsertion)
        {
            var yonetmen = new Yönetmen
            {
                BirtDay = yönetmenForInsertion.BirtDay,
                Filmler = new List<FilmModel>(),

                IsDeleted = false,
                Name = yönetmenForInsertion.Name,

            };

            foreach (var id in yönetmenForInsertion.FilmlerId)
            {
                var film = _filmservice.GetFilmById(id);
                yonetmen.Filmler.Add(film);
            }

            _context.Yonetmens.Add(yonetmen);
            _context.SaveChanges();
            return yonetmen;
        }

        public bool DeleteYönetmen(int id)
        {
            var yonetmen =_context.Yonetmens.Where(y  => y.Id == id).FirstOrDefault();
            if (yonetmen == null)
            {
                return false;
            }
            yonetmen.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Yönetmen> GetAllYonetmen()
        {
            // Filmleri de dahil etmek için Include kullanıyoruz
            var yonetmen = _context.Yonetmens
                .Include(y => y.Filmler) // Filmleri dahil et
                .Where(y => !y.IsDeleted)  // Silinmemiş olanları getir
                .ToList();

            return yonetmen;
        }

        public Yönetmen GetByID(int id)
        {
            var yonetmen = _context.Yonetmens
                .Include(y => y.Filmler) // Filmler ilişkisini dahil et
                .Where(y => y.Id == id && !y.IsDeleted) // Hem ID eşleşmeli hem de silinmemiş olmalı
                .FirstOrDefault();

            if (yonetmen == null) // Burada film kontrolü değil, yonetmen kontrolü yapılmalı
            {
                throw new Exception("Yönetmen bulunamadı"); // Hata mesajını güncelledim
            }

            return yonetmen;
        }


        public Yönetmen UpdateYonetmen(YönetmenForUpdate yönetmenForUpdate)
        {
            var existingYonetmen = GetByID(yönetmenForUpdate.Id);

            if (existingYonetmen == null)
            {
                throw new KeyNotFoundException($"Yonetmen ID {yönetmenForUpdate.Id} bulunamadı.");
            }

            // Yönetmen bilgilerini güncelle
            existingYonetmen.Name = yönetmenForUpdate.Name;
            existingYonetmen.IsDeleted = yönetmenForUpdate.IsDeleted;
            existingYonetmen.BirtDay = yönetmenForUpdate.BirtDay;

            // Film Id'ler üzerinden Filmler'i güncelle
            if (yönetmenForUpdate.FilmlerId != null)
            {
                // Yönetmenin yeni filmlerini ekliyoruz
                var yeniFilmler = _context.Films
                    .Where(f => yönetmenForUpdate.FilmlerId.Contains(f.Id))
                    .ToList();

                existingYonetmen.Filmler = yeniFilmler;
            }

            _context.Yonetmens.Update(existingYonetmen);
            _context.SaveChanges();

            return existingYonetmen;
        }


    }
}
