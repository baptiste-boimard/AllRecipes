namespace AllRecipes_API.Models;

public class InsertRecipesResult
{
  public List<string>? RejectedRecipes { get; set; }
  public List<string>? AcceptedRecipes { get; set; }
}