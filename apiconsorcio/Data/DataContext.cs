using Microsoft.EntityFrameworkCore;
using apiconsorcio.Data;
using apiconsorcio.Models;


namespace apiconsorcio.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Dados> Dados { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
           protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dados>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd(); // simula autoincrement
        }

    }
}

