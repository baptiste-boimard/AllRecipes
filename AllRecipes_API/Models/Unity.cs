namespace AllRecipes_API.Models;

public class Unity
{
    public int? Id { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public string? Description { get; set; }
}