using System.Text.Json;
using System.Text.Json.Nodes;
using AllRecipes_API.DTO;
using AllRecipes_API.Models;
using AllRecipes_API.Repositories;
using AllRecipes_API.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace AllRecipes_API.Controllers;

public class AllRecipesController : Controller
{
  private readonly MongoRecipesRepository _mongoRecipesRepository;
  private readonly PostgresRecipeRepository _postgresRecipeRepository;

  public AllRecipesController(MongoRecipesRepository mongoRecipesRepository, PostgresRecipeRepository postgresRecipeRepository)
  {
    _mongoRecipesRepository = mongoRecipesRepository;
    _postgresRecipeRepository = postgresRecipeRepository;
  }
  
  /// <summary>
  /// Demande le scrapping vers une url en choissant sa BDD
  /// </summary>
  [HttpPost]
  [Route($"/scrapping/{{url}}/{{sql}}/{{nosql}}")]
  [Produces("application/json")]
  [SwaggerOperation(Tags = ["AllRecipes Scrapping"])]
    // [ProducesResponseType(200, Type = typeof(StatusResponseOk))]
    // [ProducesResponseType(404, Type = typeof(StatusNotFoundError))]
    public async Task<IActionResult> Scrapping(string url, bool sql, bool nosql)
  {
    try
    {
      
      // Création des Tasks pour la demande de scrapping vers un type de BDD
      Task<InsertRecipesResult> responseSql = Task.FromResult(new InsertRecipesResult());
      Task<InsertRecipesResult> responseNoSql = Task.FromResult(new InsertRecipesResult());
      
      if (sql)
      {
        var scrappingResultSql = await ScrapperService.ScrappingSQL(url);
        var scrappingJsonSql = JsonSerializer.Serialize(scrappingResultSql);
        responseSql = _postgresRecipeRepository.InsertRecipesToPostgresDb(scrappingJsonSql);
      }

      if (nosql)
      {
        var scrappingResultNoSql = await ScrapperService.Scrapping(url);
        var scrappingJsonNoSql = JsonSerializer.Serialize(scrappingResultNoSql);
        responseNoSql = _mongoRecipesRepository.InsertRecipesToMongoDb(scrappingJsonNoSql);
      }
      
      // Création de la réponse vers swagger
      string acceptedSql = "Les recettes suivantes ont été ajoutées à la BDD SQL";
      string rejectedSql = "Les recettes suivantes n'ont pas été rajoutées à la BDD SQL car elles y figurent dèjà";
      string acceptedNoSql = "Les recettes suivantes ont été ajoutées à la BDD NoSQL";
      string rejectedNoSql = "Les recettes suivantes n'ont pas été rajoutées à la BDD NoSQL car elles y figurent dèjà";

      JsonObject jsonObjectResult = new JsonObject();

      if (responseSql.Result.RejectedRecipes != null || responseSql.Result.AcceptedRecipes != null)
      {
        jsonObjectResult.Add("acceptSql", $"{acceptedSql}");
        jsonObjectResult.Add("acceptedSql",$"{JsonSerializer.Serialize(responseSql.Result.AcceptedRecipes)}");
        jsonObjectResult.Add("rejectSql", $"{rejectedSql}");
        jsonObjectResult.Add("rejectedSql",$"{JsonSerializer.Serialize(responseSql.Result.RejectedRecipes)}");
      }
      

      if (responseNoSql.Result.RejectedRecipes != null || responseNoSql.Result.AcceptedRecipes != null)
      {
        jsonObjectResult.Add("acceptNoSql", $"{acceptedNoSql}");
        jsonObjectResult.Add("acceptedNoSql", $"{JsonSerializer.Serialize(responseNoSql.Result.AcceptedRecipes)}");
        jsonObjectResult.Add("rejectNoSql", $"{rejectedNoSql}");
        jsonObjectResult.Add("rejectedNoSql", $"{JsonSerializer.Serialize(responseNoSql.Result.RejectedRecipes)}");
      }
      
      return Ok(jsonObjectResult);
          
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
            throw;
    }
  }

  /// <summary>
  /// Demande toutes les recette de la BDD SQL
  /// </summary>
  [HttpGet]
  [Route($"/GetAllSql/")]
  [Produces("application/json")]
  [SwaggerOperation(Tags = ["SQL Database"])]
  public async Task<IActionResult> GetAllRecipesSql()
  {
    try
    {
      var response = _postgresRecipeRepository.GetAll();

      return Ok(response);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

    /// <summary>
    /// Demande une recette de la base de données SQL
    /// </summary>
    [HttpGet]
    [Route($"/GetOneRecipeSql/{{id}}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["SQL Database"])]
    public async Task<IActionResult> GetOneRecipeSql(int id)
    {
      try
      {
        var response = _postgresRecipeRepository.GetOneById(id);
      
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    
    }

    /// <summary>
    /// Demande les recettes qui contiennent cet ingredient
    /// </summary>
    [HttpGet]
    [Route($"/GetRecipesByIngredientSql/{{ingredient}}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["SQL Database"])]
    public async Task<IActionResult> GetRecipesByIngredientSql(string ingredient)
    {
      try
      {
        var response = _postgresRecipeRepository.GetRecipesByIngredient(ingredient);
        
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
    
    /// <summary>
    /// Recherche une recette par son nom
    /// </summary>
    [HttpGet]
    [Route($"/GetRecipeByTitleSql/{{title}}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["SQL Database"])]
    public async Task<IActionResult> GetRecipesByTitleSql(string title)
    {
      try
      {
        var response = _postgresRecipeRepository.GetRecipesByTitle(title);
        
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    /// <summary>
    /// Demande toutes les recette de la BDD NoSQL
    /// </summary>
    [HttpGet]
    [Route($"/GetAllNoSql/")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["NoSQL Database"])]
    public async Task<IActionResult> GetAllRecipesNoSql()
    {
      try
      {
        var response = await _mongoRecipesRepository.GellAll();

        // var responseJson = response.Result.ToJson();
        
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    /// <summary>
    /// Demande une Recette par son id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet]
    [Route($"/GetOneNoSql/{{id}}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["NoSQL Database"])]
    public async Task<IActionResult> GetOneRecipeNoSql(string id)
    {
      try
      {
        var response = await _mongoRecipesRepository.GellOneById(id);
        
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
    
    /// <summary>
    /// Demande les Recettes qui contiennent cet ingredient
    /// </summary>
    /// <param name="ingredient"></param>
    [HttpGet]
    [Route($"/GetRecipesByIngredientNoSql/{{ingredient}}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["NoSQL Database"])]
    public async Task<IActionResult> GetRecipesByIngredientNoSql(string ingredient)
    {
      try
      {
        var response = await _mongoRecipesRepository.GellRecipesByIngredient(ingredient);
        
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
    /// <summary>
    /// Demande une Recette par le nom de la recette
    /// </summary>
    /// <param name="title"></param>
    [HttpGet]
    [Route($"/GetRecipeByTitleNoSql/{{title}}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["NoSQL Database"])]
    public async Task<IActionResult> GetRecipeByTitleNoSql(string title)
    {
      try
      {
        var response = await _mongoRecipesRepository.GellRecipeByTitle(title);
        
        return Ok(response);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
}

// A finir quand demande de title recette qui ne sont pas en BDD sql => erreur, il faut gérer la nullité du result
// => pareil pour l'id

// En NO SQL pas d'erreur mais un code 204 non documenté

// probleme avec le scrapping vers SQL qui trouve des champs null, bizare
