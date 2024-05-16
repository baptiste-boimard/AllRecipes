namespace AllRecipes_API.Models;

public class Quantity
{
    public Quantity()
    {
    }

    public Quantity(string? description)
    {
        Description = description;
    }  
    public int? Id { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public string? Description { get; set; }
}