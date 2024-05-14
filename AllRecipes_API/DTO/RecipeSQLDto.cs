namespace AllRecipes_API.DTO;

public class RecipeSQLDto
{
  public int? Id { get; set; }
  public string? Title { get; set; }
  public string? SubTitle { get; set; }
  public string? Directions { get; set; }
  public IEnumerable<IngredientDto> Ingredients { get; set; }
}