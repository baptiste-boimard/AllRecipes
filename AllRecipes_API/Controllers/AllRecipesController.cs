using System.Text.Json;
using System.Text.Json.Nodes;
using AllRecipes_API.Models;
using AllRecipes_API.Repositories;
using AllRecipes_API.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Xml.Formatting;

namespace AllRecipes_API.Controller;

public class AllRecipesController : Microsoft.AspNetCore.Mvc.Controller
{
  private readonly MongoRecipesRepository _mongoRecipesRepository;

  public AllRecipesController(MongoRecipesRepository mongoRecipesRepository)
  {
    _mongoRecipesRepository = mongoRecipesRepository;
  }
  
  /// <summary>
  /// Demande le scrapping vers une url
  /// </summary>
  [HttpGet]
  [Route($"/scrapping/{{url}}/")]
  [Produces("application/json")]
  // [ProducesResponseType(200, Type = typeof(StatusResponseOk))]
  // [ProducesResponseType(404, Type = typeof(StatusNotFoundError))]
  public async Task<IActionResult> Scrapping(string url)
  {
    try
    {
      var scrappingResult = await ScrapperService.Scrapping(url);

      var scrappingJson = JsonSerializer.Serialize(scrappingResult);

      Task<InsertRecipesResult> response = _mongoRecipesRepository.InsertRecipesToMongoDb(scrappingJson);
      
      string accepted = "Les recettes suivantes ont été ajoutées à la BDD";
      string rejected = "Les recettes suivantes n'ont pas été rajoutées à la BDD car elles y figurent dèjà";
      var jsonObjetResult = new
      {
        Accepted= accepted,
        response.Result.AcceptedRecipes,
        Rejected = rejected,
        response.Result.RejectedRecipes,
      };
      return Ok(jsonObjetResult);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw e;
    }
 


  }
}