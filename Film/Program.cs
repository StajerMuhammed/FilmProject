using Film.Datas;
using Film.Models;
using Film.Service.Services.ServiceRole;
using Film.Service.Services.ServiceUser;
using Film.Services.ServiceCategory;
using Film.Services.ServiceFilm;
using Film.Services.ServiceYonetmen;
using Film.WebAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Film
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IFilmService, FilmService>();
            builder.Services.AddScoped<IYonetmenService, YönetmenService>();
            builder.Services.AddScoped<IRoleService,RoleService>();
            builder.Services.AddScoped<IUserService,UserService>();


            // Veritabaný baðlantý dizesi yapýlandýrmasý
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<SampleDBContext>(options =>
                options.UseSqlServer(connectionString));






            // Döngüsel referans problemini çözmek için JSON ayarlarýný yap
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            // Swagger yapýlandýrmasý
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Swagger ve Https ayarlarý
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
