using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class ApplicationDbContext : DbContext
    {

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Promocao> Usuario { get; set; }
        public DbSet<Jogo> Jogo { get; set; }
        public DbSet<Promocao> Promocao { get; set; }
        public DbSet<Pedido> Pedido { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:ConnectionString"]);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);          
        }
    }
}
