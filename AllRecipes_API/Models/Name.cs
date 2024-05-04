namespace AllRecipes_API.Models;

public class Name
{
    public int? Id { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public string? Description { get; set; }
}