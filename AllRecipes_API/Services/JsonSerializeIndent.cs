using System.Text.Json;

namespace AllRecipes_API.Services;

public class JsonSerializeIndent
{
    public static string GetJsonSerializeIndent(List<String> recipes)
    {
        string acceptedRecipesJson = JsonSerializer.Serialize(recipes);
        JsonDocument document = JsonDocument.Parse(acceptedRecipesJson);
        string indentedJson = document.RootElement.ToString();
        return indentedJson;
    }
}