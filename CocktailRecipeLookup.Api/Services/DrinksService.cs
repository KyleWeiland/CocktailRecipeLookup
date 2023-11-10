using Amazon.SimpleSystemsManagement.Model;
using Amazon.SimpleSystemsManagement;
using CocktailRecipeLookup.Api.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CocktailRecipeLookup.Api.Services
{
    public class DrinksService : IDrinksService
    {
        private readonly HttpClient _httpClient;

        public DrinksService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Drink>> GetDrinksByNameAsync(string name)
        {
            var apiKey = await GetApiKeyAsync();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            var response = await _httpClient.GetStringAsync($"https://api.api-ninjas.com/v1/cocktail?name={name}");
            var drinks = JsonConvert.DeserializeObject<List<Drink>>(response);
            ConvertInstructions(drinks);
            return drinks;
        }

        public async Task<List<Drink>> GetDrinksByIngredientsAsync(List<string> ingredients)
        {
            var apiKey = await GetApiKeyAsync();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
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

        private async Task<string >GetApiKeyAsync()
        {
            using var ssmClient = new AmazonSimpleSystemsManagementClient(Amazon.RegionEndpoint.USEast1);
            var request = new GetParameterRequest { Name = "api-ninja-key", WithDecryption = true };
            var response = await ssmClient.GetParameterAsync(request);
            return response.Parameter.Value;
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