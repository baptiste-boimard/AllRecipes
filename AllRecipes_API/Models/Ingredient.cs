using System.Text.Json.Serialization;

namespace AllRecipes_API.Models;

public class Ingredient
{
    [JsonIgnore]
    public int? Id { get; set; }
    
    [JsonIgnore]
    public int? RecipeId { get; set; }
    
    [JsonIgnore]
    public RecipeSql? Recipe { get; set; }
    
    [JsonIgnore]
    public int? QuantityId { get; set; }
    public Quantity? Quantity { get; set; }
    
    [JsonIgnore]
    public int? UnityId { get; set; }
    public Unity? Unity { get; set; }
    
    [JsonIgnore]
    public int? NameId { get; set; }
    public Name? Name { get; set; }
}