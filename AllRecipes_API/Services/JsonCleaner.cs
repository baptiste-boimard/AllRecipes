using System.Text.Json;
using System.Text.Json.Nodes;
using AllRecipes_API.DTO;

namespace AllRecipes_API.Services;

public class JsonCleaner
{
  public static string RemoveIdProperties(string json)
  {
    // Parse the JSON string into a JsonNode
    var jsonObject = JsonNode.Parse(json);

    // Remove all properties named "$id"
    RemoveIdPropertiesRecursively(jsonObject);

    // Serialize the modified JsonNode back to a JSON string
    return jsonObject.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
  }
  
  private static void RemoveIdPropertiesRecursively(JsonNode node)
  {
    if (node is JsonObject jsonObject)
    {
      var propertiesToRemove = new List<string>();

      foreach (var property in jsonObject)
      {
        if (property.Key == "$id")
        {
          propertiesToRemove.Add(property.Key);
        }
        else
        {
          RemoveIdPropertiesRecursively(property.Value);
        }
      }

      foreach (var property in propertiesToRemove)
      {
        jsonObject.Remove(property);
      }
    }
    else if (node is JsonArray jsonArray)
    {
      foreach (var item in jsonArray)
      {
        RemoveIdPropertiesRecursively(item);
      }
    }
  }
}