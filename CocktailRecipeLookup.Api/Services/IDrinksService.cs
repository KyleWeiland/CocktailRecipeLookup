using CocktailRecipeLookup.Api.Models;

namespace CocktailRecipeLookup.Api.Services
{
    public interface IDrinksService
    {
        Task<List<Drink>> GetDrinksByNameAsync(string name);
        Task<List<Drink>> GetDrinksByIngredientsAsync(List<string> ingredients);
    }
}