using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;

namespace Prestamium.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //Fluent Api

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().Property(x => x.Name).HasMaxLength(50);
        }

        public DbSet<Client> Clients { get; set; }
    }
}
