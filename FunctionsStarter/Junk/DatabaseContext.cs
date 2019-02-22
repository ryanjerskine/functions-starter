using Microsoft.EntityFrameworkCore;

namespace FunctionsStarter.Junk
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Individual> Individuals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=:memory:");
        }
    }
    public class Individual
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}