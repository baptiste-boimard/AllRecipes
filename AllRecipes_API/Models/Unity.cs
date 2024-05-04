namespace AllRecipes_API.Models;

public class Unity
{
    public Unity()
    {
    }

    public Unity(string? description)
    {
        Description = description;
    }

    public int? Id { get; init; }
    public List<Ingredient>? Ingredients { get; init; } = new List<Ingredient>();
    public string? Description { get; set; } = "";
}