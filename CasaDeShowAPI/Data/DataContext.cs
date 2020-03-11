using CasaDeShowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CasaDeShowAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Casa> Casas { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=sqlitedemo.db");
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Casa>().HasKey(casa => casa.Id);
            modelBuilder.Entity<Casa>().HasMany(casa => casa.Eventos).WithOne(evento => evento.Casa);

            modelBuilder.Entity<Evento>().HasKey(evento => evento.Id);
            modelBuilder.Entity<Evento>().HasOne(evento => evento.Casa);

            modelBuilder.Entity<Usuario>().HasKey(usuario => usuario.Email);

            modelBuilder.Entity<Venda>().HasKey(venda => venda.Id);
            modelBuilder.Entity<Venda>().HasOne(venda => venda.Usuario).WithMany(usuario => usuario.Compras);
            modelBuilder.Entity<Venda>().HasOne(venda => venda.Evento).WithMany(evento => evento.Vendas);
        }
    }
}