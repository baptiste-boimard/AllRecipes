using AllRecipes_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllRecipes_API.Data
{
    public class PostgresDbContext : DbContext
    {
        
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }
        
        public DbSet<RecipeSql?> RecipesSql { get; set; }
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Unity> Unities { get; set; }
        public DbSet<Name> Names { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de la relation RecipeSql et Ingredient
            modelBuilder.Entity<RecipeSql>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId);

            // Configuration de la relation Ingredient et Quantity
            modelBuilder.Entity<Quantity>()
                .HasMany(q => q.Ingredients)
                .WithOne(i => i.Quantity)
                .HasForeignKey(i => i.QuantityId);
            
            // Configuration pour le champ Description soit unique
            modelBuilder.Entity<Unity>()
                .HasIndex(u => u.Description)
                .IsUnique();

            // Configuration de la relation Ingredient et Unity
            modelBuilder.Entity<Unity>()
                .HasMany(u => u.Ingredients)
                .WithOne(i => i.Unity)
                .HasForeignKey(i => i.UnityId);
            
            
            // Configuration de la relation Ingredient et Name
            modelBuilder.Entity<Name>()
                .HasMany(n => n.Ingredients)
                .WithOne(i => i.Name)
                .HasForeignKey(i => i.NameId);
        }
    }
}
