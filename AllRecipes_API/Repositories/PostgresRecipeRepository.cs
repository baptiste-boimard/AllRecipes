using AllRecipes_API.Data;

namespace AllRecipes_API.Repositories
{
    public class PostgresRecipeRepository
    {
        private readonly PostgresDbContext _postgresDbContext;

        public PostgresRecipeRepository(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }
    }
    
    
}
