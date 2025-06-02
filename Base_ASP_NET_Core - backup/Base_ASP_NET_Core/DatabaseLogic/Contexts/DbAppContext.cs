using Microsoft.EntityFrameworkCore;
using TestDb.DatabaseLogic.Models;

namespace TestDb.DatabaseLogic.Contexts
{
    public class DbAppContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }

        public DbAppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost; port=3306; user=root; password=; database=my_db;",
                new MySqlServerVersion(new Version(8, 2, 12))
                );
        }
    }
}
