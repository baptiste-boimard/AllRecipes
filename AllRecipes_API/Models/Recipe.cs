using Microsoft.VisualBasic.CompilerServices;

namespace AllRecipes_API.Models;

public class Recipe
{
  public ObjectType? _id { get; set; }
  public string? Title { get; set; }
  public string? SubTitle { get; set; }
  public List<string>? Ingredients { get; set; }
  public List<String>? Directions { get; set; }
}
