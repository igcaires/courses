using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Models.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Description = "Comportamental",
                },
                new Category
                {
                    CategoryId = 2,
                    Description = "Programação",
                },
                new Category
                {
                    CategoryId = 3,
                    Description = "Qualidade",
                },
                new Category
                {
                    CategoryId = 4,
                    Description = "Processos",
                }
            );
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
