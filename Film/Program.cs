using Film.Datas;
using Film.Models;
using Film.Services.ServiceCategory;
using Film.Services.ServiceFilm;
using Film.Services.ServiceYonetmen;
using Film.WebAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace Film
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // D�ng�sel referanslar� �nlemek i�in ReferenceHandler.Preserve kullan
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IFilmService, FilmService>();
            builder.Services.AddScoped<IYonetmenService, Y�netmenService>();

            // uygulaman�z�n SQL Server veritaban�yla etkili bir �ekilde ileti�im kurabilmesi i�in gerekli yap�land�rmay� haz�rlar.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SampleDBContext>(options =>
                options.UseSqlServer(connectionString));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
