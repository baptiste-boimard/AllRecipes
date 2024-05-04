using System.Linq.Expressions;
using System.Text.Json;
using AllRecipes_API.Models;
using MongoDB.Driver;

namespace AllRecipes_API.Repositories;

public class MongoRecipesRepository
{
  // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
  private readonly IMongoDatabase _mongoDatabase;
  private readonly IMongoCollection<RecipeNoSQL> _recipesCollection;

  public MongoRecipesRepository(IMongoDatabase mongoDatabase)
  {
    _mongoDatabase = mongoDatabase;
    _recipesCollection = _mongoDatabase.GetCollection<RecipeNoSQL>("recipes");

  }

  public async Task<InsertRecipesResult> InsertRecipesToMongoDb(string jsonFile)
  {
    try
    {
      List<RecipeNoSQL>? recipes = JsonSerializer.Deserialize<List<RecipeNoSQL>>(jsonFile);
      List<string> recipesRejected = new List<string>();
      List<string> recipesAccepted = new List<string>();

      foreach (RecipeNoSQL recipe in recipes!)
      {
        // var filter = Builders<RecipeNoSQL>.Filter.Eq("Title", $"{recipe.Title}");
        var result = await _recipesCollection.Find( t => t.Title == recipe.Title).FirstOrDefaultAsync();

        if (result == null)
        {
          await _recipesCollection.InsertOneAsync(recipe);
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
    catch (MongoException e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
}

