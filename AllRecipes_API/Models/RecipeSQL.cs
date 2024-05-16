using System.Text.Json.Serialization;

namespace AllRecipes_API.Models;

public class RecipeSql
{
  public int? Id { get; set; }
  public List<Ingredient>? Ingredients { get; set; } = new List<Ingredient>();
   public string? Title { get; set; }
  public string? SubTitle { get; set; }
  public string? Directions { get; set; }
}