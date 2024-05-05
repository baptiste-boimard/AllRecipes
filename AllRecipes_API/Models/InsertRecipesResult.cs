namespace AllRecipes_API.Models;

public class InsertRecipesResult
{
  public List<string?> RejectedRecipes { get; set; } = null;
  public List<string?> AcceptedRecipes { get; set; } = null;
}