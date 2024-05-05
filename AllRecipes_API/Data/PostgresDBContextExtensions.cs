using AllRecipes_API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AllRecipes_API.Data;

public class PostgresDbContextExtensions
{
  private readonly PostgresDbContext _postgresDbContext;

  public PostgresDbContextExtensions(PostgresDbContext postgresDbContext)
  {
    _postgresDbContext = postgresDbContext;
  }
  
  public void AddOrUpdateUnity(Unity? unity)
  {
    var existingUnity = _postgresDbContext.Unities
      .AsNoTracking()
      .FirstOrDefault(r=> r!.Description == unity!.Description);

    if (unity == null)
    {
      _postgresDbContext.Unities.Add(unity);
    }
    else
    {
      unity.Id = existingUnity.Id;
      _postgresDbContext.Unities.Update(unity);
    }
  }
}