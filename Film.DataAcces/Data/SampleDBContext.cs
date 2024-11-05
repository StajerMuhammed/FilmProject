using Film.Entity;
using Film.Entity.Models;
using Microsoft.EntityFrameworkCore;


namespace Film.Datas
    {
        public class SampleDBContext : DbContext
        {
            public SampleDBContext(DbContextOptions<SampleDBContext> options)
                : base(options)
            {
            }
        public SampleDBContext()
        {
        }
        public virtual DbSet<User> Users { get; set; } // DbSet adını çoğul yapabilirsiniz
        public virtual DbSet<Role> Role { get; set; } // DbSet adını çoğul yapabilirsinizs
        public virtual DbSet<Film.Models.FilmModel> Films { get; set; } // DbSet adını çoğul yapabilirsiniz
        public virtual DbSet<Film.Models.Category> Categories { get; set; } // DbSet adını çoğul yapabilirsiniz
        public virtual DbSet<Film.Models.Yönetmen> Yonetmens { get; set; } // DbSet adını çoğul yapabilirsiniz

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DL-MAKTAS1;Initial Catalog=Movies;Integrated Security=True;Encrypt=True;Trust Server Certificate=True").EnableSensitiveDataLogging();
        }
        }
}

