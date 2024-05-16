namespace AllRecipes_API.Models;

public class Ingredient
{
    public int? Id { get; set; }
    public int? RecipeId { get; set; }
    public RecipeSql? Recipe { get; set; }
    public int? QuantityId { get; set; }
    public Quantity Quantity { get; set; }
    public int? UnityId { get; init; }
    public Unity Unity { get; set; }
    public int? NameId { get; init; }
    public Name Name { get; set; }
}