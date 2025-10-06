using apiconsorcio.Models;
using Microsoft.EntityFrameworkCore;
using apiconsorcio.Data;
using apiconsorcio.Models;

namespace apiconsorcio.Data
{
    public class CampanhaContext:DbContext
    {
        public DbSet<Campanha> Campanha { get; set; }

        public CampanhaContext(DbContextOptions<CampanhaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campanha>()
                .Property(e => e.id)
                .ValueGeneratedOnAdd(); // simula autoincrement
        }
    }
}
