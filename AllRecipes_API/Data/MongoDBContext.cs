using AllRecipes_API.Models;
using MongoDB.Driver;

namespace AllRecipes_API.Data;

public class MongoDBContext
{
  private readonly IMongoDatabase _database;

  public MongoDBContext(string connectionString, string databaseName)
  {
    var client = new MongoClient(connectionString);
    _database = client.GetDatabase(databaseName);
  }

  public IMongoCollection<RecipeNoSQL> Users => _database.GetCollection<RecipeNoSQL>("recipes");
  
  
  
}