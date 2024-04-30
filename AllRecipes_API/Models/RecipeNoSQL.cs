using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AllRecipes_API.Models;

public class RecipeNoSQL
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Id { get; set; }
  [BsonElement("Title")]
  public string Title { get; set; }
  [BsonElement("SubTitle")]
  public string SubTitle { get; set; }
  [BsonElement("Ingredients")]
  public List<string> Ingredients { get; set; }
  [BsonElement("Directions")]
  public List<String> Directions { get; set; }
}