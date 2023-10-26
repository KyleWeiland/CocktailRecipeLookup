using CocktailRecipeLookup.Api.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
            ConvertInstructions(drinks);
            return drinks;
        }

        public async Task<List<Drink>> GetDrinksByIngredientsAsync(List<string> ingredients)
        {
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
            var combined = "";
            foreach (var i in ingredients)
            {
                combined += i + ",";
            }
            combined = combined.Substring(0, combined.Length - 1);
            var response = await _httpClient.GetStringAsync($"https://api.api-ninjas.com/v1/cocktail?ingredients={combined}");
            var drinks = JsonConvert.DeserializeObject<List<Drink>>(response);
            return drinks;
        }

        private void ConvertInstructions(List<Drink> drinks)
        {
            // Skip if none of the ingredients have cl
            if (!drinks.Any(d => d.Ingredients.Any(i => i.Contains(" cl ")))) return;

            foreach (var d in drinks)
            {
                for (int j = 0; j < d.Ingredients.Count; j++)
                {
                    d.Ingredients[j] = ConvertClToMl(d.Ingredients[j]);
                }
            }
        }

        private string ConvertClToMl(string ingredient)
        {
            // Use regex to match the pattern of a number followed by "cl"
            var regex = new Regex(@"(\d+(\.\d+)?)\s*cl");

            // Use MatchEvaluator to convert the matched value to ml
            return regex.Replace(ingredient, m =>
            {
                // Parse the number and multiply by 10 to convert to ml
                double valueInCl = double.Parse(m.Groups[1].Value);
                double valueInMl = valueInCl * 10;

                // Return the converted value with "ml"
                return $"{valueInMl} ml";
            });
        }
    }
}