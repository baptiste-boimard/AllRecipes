namespace AllRecipes_API.Models;

public class CustomError : Exception
{
    public new string? Message { get; set; }
}