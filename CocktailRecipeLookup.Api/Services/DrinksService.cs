using CocktailRecipeLookup.Api.Models;
using Newtonsoft.Json;

namespace CocktailRecipeLookup.Api.Services
{
    public class DrinksService : IDrinksService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public DrinksService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["CocktailsApiKey"];
        }

        public async Task<List<Drink>> GetDrinksByNameAsync(string name)
        {
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
            var response = await _httpClient.GetStringAsync($"https://api.api-ninjas.com/v1/cocktail?name={name}");
            var drinks = JsonConvert.DeserializeObject<List<Drink>>(response);
            return drinks;
        }
    }
}