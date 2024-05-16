using System.ComponentModel;
using AllRecipes_API.Models;
using HtmlAgilityPack;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace AllRecipes_API.Services;

public class ScrapperService
{
  public static async Task<List<RecipeNoSQL>> Scrapping(string url)
  {
        string decodedUrl = HttpUtility.UrlDecode(url);

        // string mainUrl = "https://www.allrecipes.com/recipes/17561/lunch/";
        // string mainUrl = "https://www.allrecipes.com/recipes/723/world-cuisine/european/italian/";
        string[] recipeLinks = await GetRecipeLinks(decodedUrl);
    
    List<RecipeNoSQL> recipes = new List<RecipeNoSQL>();
    foreach (var link in recipeLinks)
    {
      RecipeNoSQL recipe = await GetRecipeDetails(link);
     
      recipes.Add(recipe!);
      
    }
    return recipes;
  }
    public static async Task<List<RecipeSql>> ScrappingSQL(string url)
    {
        string decodedUrl = HttpUtility.UrlDecode(url);

        string[] recipeLinks = await GetRecipeLinks(decodedUrl);

        List<RecipeSql> recipes = new List<RecipeSql>();
        
        foreach (var link in recipeLinks)
        {
            RecipeSql recipe = await GetRecipeDetailsForSQL(link);
            // RecipeSql recipe = await GetRecipeDetailsForSQL(link);

            recipes.Add(recipe);

        }
        return recipes;
    }

    public static async Task<string[]> GetRecipeLinks(string url)
  {
    HttpClient httpClient = new HttpClient();
    List<string> recipeLinks = new List<string>();
    string html = await httpClient.GetStringAsync(url);
    HtmlDocument htmlDocument = new HtmlDocument();
    htmlDocument.LoadHtml(html);

    // Prend toutes les recettes de la pages
    // var links = htmlDocument.DocumentNode.SelectNodes("//div[@class='comp tax-sc__recirc-list card-list mntl-universal-card-list mntl-document-card-list mntl-card-list mntl-block']//a[@href]");
    
    // Prend uniquement la 1er partie des recettes de la page
    var links = htmlDocument.DocumentNode.SelectNodes("//div[@id='tax-sc__recirc-list_1-0']//a[@href]");
   
    foreach (var link in links)
    {
      string href = link.GetAttributeValue("href", string.Empty);
      
      // On s'assure que le link contient bien une recette
      Regex recipeRegex = new Regex(@"^https:\/\/www\.allrecipes\.com\/.*?recipe");
      if (recipeRegex.IsMatch(href))
      {
        recipeLinks.Add(href);
      }
    }
    
    return recipeLinks.Distinct().ToArray(); // Éliminer les doublons
  }
  
  public static async Task<RecipeNoSQL> GetRecipeDetails(string url)
  {
    HttpClient httpClient = new HttpClient();
    string html = await httpClient.GetStringAsync(url);
    HtmlDocument htmlDocument = new HtmlDocument();
    htmlDocument.LoadHtml(html);

    RecipeNoSQL recipe = new RecipeNoSQL();

    // Exemple de récupération de titre et d'ingrédients, ajustez selon la structure exacte du site
    var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='article-heading type--lion']");
    var subheadingNode = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='article-subheading type--dog']");
    var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='mntl-structured-ingredients__list']/li");
    var directionNodes = htmlDocument.DocumentNode.SelectNodes("//ol[@class='comp mntl-sc-block mntl-sc-block-startgroup mntl-sc-block-group--OL']/li");

    if (titleNode != null)
    {
      recipe.Title = titleNode.InnerText.Trim();
      recipe.SubTitle = subheadingNode.InnerText.Trim();
      recipe.Ingredients = new List<string>();
      recipe.Directions = new List<string>();

      if (ingredientsNodes != null)
      {
        foreach (var node in ingredientsNodes)
        {
          recipe.Ingredients.Add(node.InnerText.Trim());
        }
      }
      
      if (directionNodes != null)
      {
        foreach (var node in directionNodes)
        {
          recipe.Directions.Add(node.InnerText.Trim());
        }
      }

      return recipe;
    }
        return null;
  }

    public static async Task<RecipeSql> GetRecipeDetailsForSQL(string url)
    {
        HttpClient client = new HttpClient();
        string html = await client.GetStringAsync(url);
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        RecipeSql recipe = new RecipeSql();

        // Exemple de récupération de titre et d'ingrédients, ajustez selon la structure exacte du site
        var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='article-heading type--lion']");
        var subheadingNode = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='article-subheading type--dog']");
        var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='mntl-structured-ingredients__list']/li");
        var directionNodes = htmlDocument.DocumentNode.SelectNodes("//ol[@class='comp mntl-sc-block mntl-sc-block-startgroup mntl-sc-block-group--OL']/li");


        if (titleNode != null)
        {
            recipe.Title = titleNode.InnerText.Trim();
            recipe.SubTitle = subheadingNode.InnerText.Trim();
            // recipe.Ingredients = new List<Ingredient>();
            recipe.Directions = new string("");

            if (ingredientsNodes != null)
            {
              foreach (var ingredientsNode in ingredientsNodes)
              {
                if (ingredientsNode == null) continue;
                
                // Imprimer les valeurs des nœuds
                HtmlNode? quantity = ingredientsNode.SelectSingleNode(".//span[@data-ingredient-quantity='true']");
                HtmlNode? unity = ingredientsNode.SelectSingleNode(".//span[@data-ingredient-unit='true']");
                HtmlNode? name = ingredientsNode.SelectSingleNode(".//span[@data-ingredient-name='true']");
                
                recipe.Ingredients!.Add(new Ingredient
                {
                  Quantity = new Quantity(description: quantity !=null ? quantity.InnerText?.Trim() : ""),
                  Unity = new Unity(description: unity != null ? unity.InnerText?.Trim() : ""),
                  Name = new Name(description: name != null ? name.InnerText?.Trim() : ""),
                });
              }
            }

            if (directionNodes != null)
            {
                for (int i = 0; i < directionNodes.Count; i++)
                {
                    recipe.Directions += (directionNodes[i].InnerText.Trim());
                };
            }
        }
        return recipe;
    }
}