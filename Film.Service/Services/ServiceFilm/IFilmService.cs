using Film.DTOs.Category;
using Film.DTOs.Movies;
using Film.Models;
using System.Collections.Generic;

namespace Film.Services.ServiceFilm
{
    public interface IFilmService
    {
        IEnumerable<FilmModel> GetAllFilms(); // Tüm Film DTO'larını getir
        Models.FilmModel GetFilmById(int id); // ID'ye göre Film Getir
        Models.FilmModel CreateFilm(FilmForInsertion Film); // Yeni Film oluşturur
        Models.FilmModel UpdateFilm(FilmForUpdate Film); // Filmi güncelle
        void DeleteFilm(int id); // Filmi sil
    }
}
