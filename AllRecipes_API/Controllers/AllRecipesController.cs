using System.Text.Json;
using System.Text.Json.Nodes;
using AllRecipes_API.DTO;
using AllRecipes_API.Models;
using AllRecipes_API.Repositories;
using AllRecipes_API.Services;
using Microsoft.AspNetCore.Mvc;

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
  /// Demande le scrapping vers une url
  /// </summary>
  [HttpGet]
  [Route($"/scrapping/{{url}}/{{sql}}/{{nosql}}")]
  [Produces("application/json")]
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
}