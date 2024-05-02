using AllRecipes_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllRecipes_API.Data
{
    public class PostgresDbContext : DbContext
    {
        
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }
        
        public DbSet<RecipeSql> RecipesSql { get; set; }
        
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseNpgsql("YourConnectionString");
        // }
    }
}
