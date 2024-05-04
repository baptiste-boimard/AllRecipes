using System.Text.Json;
using AllRecipes_API.Data;
using AllRecipes_API.Models;
using MongoDB.Driver;

namespace AllRecipes_API.Repositories
{
    public class PostgresRecipeRepository
    {
        private readonly PostgresDbContext _postgresDbContext;

        public PostgresRecipeRepository(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<RecipeSql?> GetRecipeByName(string? recipeName)
        {
            return _postgresDbContext.RecipesSql.FirstOrDefault(r=> r.Title == recipeName);
        }

        public async Task<InsertRecipesResult> InsertRecipesToPostgresDb(string jsonFile)
        {
            try
            {
                List<RecipeSql>? recipes = JsonSerializer.Deserialize<List<RecipeSql>>(jsonFile);
                List<string> recipesRejected = new List<string>();
                List<string> recipesAccepted = new List<string>();

                foreach (RecipeSql recipe in recipes!)
                {
                    var result = await GetRecipeByName(recipe.Title);

                    if (result == null)
                    {
                        Add(recipe);
                        recipesAccepted.Add(recipe.Title);
                    }
                    else
                    {
                        recipesRejected.Add(recipe.Title);
                    }
                }

                return new InsertRecipesResult
                {
                    RejectedRecipes = recipesRejected,
                    AcceptedRecipes = recipesAccepted,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool Add(RecipeSql recipe)
        {
            _postgresDbContext.Add(recipe);
            return Save();
        }
        
        public bool Save()
        {
            var saved = _postgresDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
