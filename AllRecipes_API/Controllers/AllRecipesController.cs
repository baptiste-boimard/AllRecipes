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
          if (sql)
            {
                var scrappingResultSql = await ScrapperService.ScrappingSQL(url);
                var scrappingJsonSql = JsonSerializer.Serialize(scrappingResultSql);
                Task<InsertRecipesResult> responseSql = _mongoRecipesRepository.InsertRecipesToPostgresDb(scrappingJsonSql);
                
            }

          if (nosql)
            {
                var scrappingResult = await ScrapperService.Scrapping(url);
                var scrappingJson = JsonSerializer.Serialize(scrappingResult);
                Task<InsertRecipesResult> responseNoSql = _mongoRecipesRepository.InsertRecipesToMongoDb(scrappingJson);

            }
          
            string acceptedSql = "Les recettes suivantes ont été ajoutées à la BDD SQL";
            string rejectedSql = "Les recettes suivantes n'ont pas été rajoutées à la BDD SQL car elles y figurent dèjà";
            string acceptedNoSql = "Les recettes suivantes ont été ajoutées à la BDD NoSQL";
            string rejectedNoSql = "Les recettes suivantes n'ont pas été rajoutées à la BDD NoSQL car elles y figurent dèjà";
            var jsonObjetResult = new
            {
                acceptedSql = acceptedSql,
                responseSql.Result.AcceptedRecipes,
                rejectedSql = rejectedSql,
                responseSql.Result.RejectedRecipes,
                acceptedNoSql = acceptedNoSql,
                responseNoSql.Result.AcceptedRecipes,
                rejectedNoSql = rejectedNoSql,
                responseNoSql.Result.RejectedRecipes,
            };
            return Ok(jsonObjetResult);
          
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
#pragma warning disable CA2200 // Lever à nouveau une exception pour conserver les détails de la pile
            throw e;
#pragma warning restore CA2200 // Lever à nouveau une exception pour conserver les détails de la pile
     }
 


  }
}