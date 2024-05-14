using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AllRecipes_API.Data;
using AllRecipes_API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoRecipesRepository>();
builder.Services.AddScoped<PostgresRecipeRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
  {
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
  }    
);
  // permet de prendre en comptre lors de la sérialisation les références circulaires
builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
  });

// Configuration de MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings");
var connectionString = mongoDbSettings["ConnectionString"];
var databaseName = mongoDbSettings["DatabaseName"];

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
  return new MongoClient(connectionString);
});

builder.Services.AddSingleton(serviceProvider =>
{
  var client = serviceProvider.GetRequiredService<IMongoClient>();
  return client.GetDatabase(databaseName);
});


// Configuration de Postgres
builder.Services.AddDbContext<PostgresDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json","v1 ");
    options.RoutePrefix = string.Empty;
  });}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
