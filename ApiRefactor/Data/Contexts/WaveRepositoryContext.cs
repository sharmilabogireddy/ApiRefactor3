using ApiRefactor.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRefactor.Data.Contexts
{
    public class WaveRepositoryContext : DbContext
    {
        public WaveRepositoryContext()
        {

        }
        public WaveRepositoryContext(DbContextOptions options)
    : base(options)
        {
        }
        public virtual DbSet<Wave> Waves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wave>();
        }
    }
}
