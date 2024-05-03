using System.Text.Json;
using System.Text.Json.Nodes;
using AllRecipes_API.Models;
using AllRecipes_API.Repositories;
using AllRecipes_API.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Operations;
using static System.Xml.Formatting;

namespace AllRecipes_API.Controller;

public class AllRecipesController : Microsoft.AspNetCore.Mvc.Controller
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
        responseSql = _mongoRecipesRepository.InsertRecipesToPostgresDb(scrappingJsonSql);
      }

      if (nosql)
      {
        var scrappingResult = await ScrapperService.Scrapping(url);
        var scrappingJson = JsonSerializer.Serialize(scrappingResult);
        responseNoSql = _mongoRecipesRepository.InsertRecipesToMongoDb(scrappingJson);
      }
      
      // Création de la réponse vers swagger
      string acceptedSql = "Les recettes suivantes ont été ajoutées à la BDD SQL";
      string rejectedSql = "Les recettes suivantes n'ont pas été rajoutées à la BDD SQL car elles y figurent dèjà";
      string acceptedNoSql = "Les recettes suivantes ont été ajoutées à la BDD NoSQL";
      string rejectedNoSql = "Les recettes suivantes n'ont pas été rajoutées à la BDD NoSQL car elles y figurent dèjà";

      JsonObject jsonObjectResult = new JsonObject();

      if (responseSql != null)
      {
        jsonObjectResult.Add("acceptedSql", $"{acceptedSql}");
        jsonObjectResult.Add("acceptedSql", $"{responseSql.Result.AcceptedRecipes}");
        jsonObjectResult.Add("rejectedSql", $"{rejectedSql}");
        jsonObjectResult.Add("rejectedSql", $"{responseSql.Result.RejectedRecipes}");
      }
      

      if (responseNoSql != null)
      {
        jsonObjectResult.Add("acceptedNoSql", $"{acceptedNoSql}");
        jsonObjectResult.Add("acceptedNoSql", $"{responseNoSql.Result.AcceptedRecipes}");
        jsonObjectResult.Add("rejectedNoSql", $"{rejectedNoSql}");
        jsonObjectResult.Add("rejectedNoSql", $"{responseNoSql.Result.RejectedRecipes}");
      }
      
      return Ok(jsonObjectResult);
          
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
            throw;
    }
  }
}