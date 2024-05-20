using System.Text.Json;
using AllRecipes_API.Data;
using AllRecipes_API.DTO;
using AllRecipes_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


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
                List<string?> recipesRejected = new List<string?>();
                List<string?> recipesAccepted = new List<string?>();

                foreach (RecipeSql recipe in recipes!)
                {
                    // Enrengistrement de la recipe en BDD ou non
                    var existingRecipe = await GetRecipeByName(recipe.Title);
                    if (existingRecipe != null)
                    {
                        recipe.Id = existingRecipe.Id;
                        recipesRejected.Add(recipe.Title);
                    }
                    else
                    {
                        var newRecipe = AddRecipe(recipe);
                        recipe.Id = newRecipe.Id;
                        recipesAccepted.Add(newRecipe.Title);
                    

                        foreach (var ingredient in recipe.Ingredients!)
                        {
                            // Enrengistrement de la Quantity en BDD
                            var existingQuantity =
                                _postgresDbContext.Quantities.AsNoTracking().FirstOrDefault(q =>
                                    q.Description == ingredient.Quantity!.Description);

                            if (existingQuantity != null)
                            {
                                ingredient.Quantity!.Id = existingQuantity.Id;
                            }
                            else
                            {
                                var newQuantity = AddQuantity(ingredient.Quantity);
                                ingredient.Quantity!.Id = newQuantity.Id;
                            }

                            var existingUnity =
                                _postgresDbContext.Unities.AsNoTracking().FirstOrDefault(u =>
                                    u.Description == ingredient.Unity!.Description);

                            if (existingUnity != null)
                            {
                                ingredient.Unity!.Id = existingUnity.Id;
                            }
                            else
                            {
                                var newUnity = AddUnity(ingredient.Unity);
                                ingredient.Unity!.Id = newUnity.Id;
                            }

                            var existingName =
                                _postgresDbContext.Names.AsNoTracking().FirstOrDefault(n =>
                                    n.Description == ingredient.Name!.Description);

                            if (existingName != null)
                            {
                                ingredient.Name!.Id = existingName.Id;
                            }
                            else
                            {
                                var newName = AddName(ingredient.Name);
                                ingredient.Name!.Id = newName.Id;

                            }

                            var existingIngredient = _postgresDbContext.Ingredients
                                .FirstOrDefault(i =>
                                    i.QuantityId == ingredient.Quantity!.Id &&
                                    i.UnityId == ingredient.Unity!.Id &&
                                    i.NameId == ingredient.Name!.Id &&
                                    i.RecipeId == recipe.Id
                                );
                            if (existingIngredient == null)
                            {
                                AddIngredient(ingredient, recipe);
                            }
                        }
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

        private Quantity AddQuantity(Quantity? quantity)
        {
            var newQuantity = new Quantity
            {
                Id = quantity!.Id,
                Description = quantity.Description,
            };
            _postgresDbContext.Quantities.Add(newQuantity);
            _postgresDbContext.SaveChanges();
            return newQuantity;
        }

        private Unity AddUnity(Unity? unity)
        {
            var newUnity = new Unity
            {
                Id = unity!.Id,
                Description = unity.Description,
            };
            _postgresDbContext.Unities.Add(newUnity);
            _postgresDbContext.SaveChanges();
            return newUnity;
        }

        private Name AddName(Name? name)
        {
            var newName = new Name
            {
                Id = name!.Id,
                Description = name.Description,
            };
            _postgresDbContext.Names.Add(newName);
            _postgresDbContext.SaveChanges();
            return newName;
        }

        private void AddIngredient(Ingredient ingredient, RecipeSql recipe)
        {
            var newIngredient = new Ingredient
            {
                RecipeId = recipe.Id,
                QuantityId = ingredient.Quantity!.Id,
                UnityId = ingredient.Unity!.Id,
                NameId = ingredient.Name!.Id,
            };
            _postgresDbContext.Ingredients.Add(newIngredient);
            _postgresDbContext.SaveChanges();
        }

        private RecipeSql AddRecipe(RecipeSql recipe)
        {
            var newRecipe = new RecipeSql
            {
                Title = recipe.Title,
                SubTitle = recipe.SubTitle,
                Directions = recipe.Directions,
            };
            _postgresDbContext.RecipesSql.Add(newRecipe);
            _postgresDbContext.SaveChanges();
            return newRecipe;
        }

        public List<RecipeSQLDto> GetAll()
        {
           
            var recipes = _postgresDbContext.RecipesSql
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Quantity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Unity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Name)
                .ToList();

            if (recipes == null)
            {
                throw new CustomError
                {
                    Message = "La Bdd ne contient aucunes informations"
                };
            }
            
            // var recipes = _postgresDbContext.RecipesSql
            //     .Include(r => r.Ingredients)!
            //     .ThenInclude(i => i.Quantity)
            //     .Include(r => r.Ingredients)!
            //     .ThenInclude(i => i.Unity)
            //     .Include(r => r.Ingredients)!
            //     .ThenInclude(i => i.Name)
            //     .ToList();

            var recipeDtos = recipes.Select(r => new RecipeSQLDto
            {
                Id = r.Id,
                Title = r.Title,
                SubTitle = r.SubTitle,
                Directions = r.Directions,
                Ingredients = r.Ingredients!.Select(i => new IngredientDto
                {
                    Quantity = i.Quantity!.Description,
                    Unity = i.Unity!.Description,
                    Name = i.Name!.Description
                    
                    
                }).ToList()
            }).ToList();
            
            
            
            return recipeDtos;
        }

        public RecipeSQLDto GetOneById(int id)
        {
            var recipe = _postgresDbContext.RecipesSql
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Quantity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Unity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Name)
                .FirstOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                throw new CustomError
                {
                    Message = "Cette id de recette n'existe pas en BDD"
                };
            }

            var recipeDto = new RecipeSQLDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                SubTitle = recipe.SubTitle,
                Directions = recipe.Directions,
                Ingredients = recipe.Ingredients.Select(i => new IngredientDto
                {
                    Quantity = i.Quantity.Description,
                    Unity = i.Unity.Description,
                    Name = i.Name.Description,
                })
            };
            return recipeDto;
        }

        public List<RecipeSQLDto> GetRecipesByIngredient(string ingredient)
        {
            var recipes = _postgresDbContext.RecipesSql
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Quantity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Unity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Name)
                .Where(r => r.Ingredients != null &&
                       r.Ingredients.Any(i => EF.Functions.Like(i.Name!.Description, $"%{ingredient}%")))
                .ToList();

            if (recipes == null)
            {
                throw new CustomError
                {
                    Message = "Aucune recette en Bdd ne contient cet ingredient"
                };
            }

            var recipeDtos = recipes.Select(r => new RecipeSQLDto
            {
                Id = r.Id,
                Title = r.Title,
                SubTitle = r.SubTitle,
                Directions = r.Directions,
                Ingredients = r.Ingredients!.Select(i => new IngredientDto
                {
                    Quantity = i.Quantity!.Description,
                    Unity = i.Unity!.Description,
                    Name = i.Name!.Description
                }).ToList()
            }).ToList();

            return recipeDtos;
        }
        
        public RecipeSQLDto GetRecipesByTitle(string title)
        {
            var recipe = _postgresDbContext.RecipesSql
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Quantity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Unity)
                .Include(r => r.Ingredients)!
                .ThenInclude(i => i.Name)
                .FirstOrDefault(r => r.Title == title );

            if (recipe == null)
            {
                throw new CustomError
                {
                    Message = "Aucune recette ne porte ce nom"
                };
            }

            var recipeDto = new RecipeSQLDto
            {
                Id = recipe!.Id,
                Title = recipe.Title,
                SubTitle = recipe.SubTitle,
                Directions = recipe.Directions,
                Ingredients = recipe.Ingredients!.Select(i => new IngredientDto
                {
                    Quantity = i.Quantity!.Description,
                    Unity = i.Unity!.Description,
                    Name = i.Name!.Description,
                })
            };
            return recipeDto;
        }
    }
}
