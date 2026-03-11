using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using CocktailRecipeLookup.Lambda.Models;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CocktailRecipeLookup.Lambda.Services
{
    public class DrinksService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string? _cachedApiKey = null;

        public async Task<List<Drink>> GetDrinksByNameAsync(string name)
        {
            var apiKey = await GetApiKeyAsync();

            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.api-ninjas.com/v1/cocktail?name={name}");
            request.Headers.Add("X-Api-Key", apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var drinks = JsonSerializer.Deserialize<List<Drink>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Drink>();

            SanitizeResponse(drinks);
            return drinks;
        }

        public async Task<List<Drink>> GetDrinksByIngredientsAsync(List<string> ingredients)
        {
            var apiKey = await GetApiKeyAsync();
            var combined = string.Join(",", ingredients);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.api-ninjas.com/v1/cocktail?ingredients={combined}");
            request.Headers.Add("X-Api-Key", apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var drinks = JsonSerializer.Deserialize<List<Drink>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Drink>();

            SanitizeResponse(drinks);
            return drinks;
        }

        private async Task<string> GetApiKeyAsync()
        {
            // Cache the API key to avoid repeated SSM calls (Lambda container reuse)
            if (_cachedApiKey != null)
                return _cachedApiKey;

            using var ssmClient = new AmazonSimpleSystemsManagementClient(Amazon.RegionEndpoint.USEast1);
            var request = new GetParameterRequest
            {
                Name = "api-ninja-key",
                WithDecryption = true
            };
            var response = await ssmClient.GetParameterAsync(request);
            _cachedApiKey = response.Parameter.Value;
            return _cachedApiKey;
        }

        private void SanitizeResponse(List<Drink> drinks)
        {
            ConvertInstructions(drinks);
            ConvertDrinkNamesToTitleCase(drinks);
        }

        private void ConvertInstructions(List<Drink> drinks)
        {
            var regexCheck = new Regex(@"\d+(\.\d+)?\s*[cC][lL]");
            if (!drinks.Any(d => d.Ingredients != null && d.Ingredients.Any(i => regexCheck.IsMatch(i))))
                return;

            foreach (var drink in drinks)
            {
                if (drink.Ingredients == null) continue;

                for (int j = 0; j < drink.Ingredients.Count; j++)
                {
                    drink.Ingredients[j] = ConvertClToMl(drink.Ingredients[j]);
                }
            }
        }

        private string ConvertClToMl(string ingredient)
        {
            var regex = new Regex(@"(\d+(\.\d+)?)\s*[cC][lL]");
            return regex.Replace(ingredient, m =>
            {
                double valueInCl = double.Parse(m.Groups[1].Value);
                double valueInMl = valueInCl * 10;
                return $"{valueInMl} ml";
            });
        }

        private void ConvertDrinkNamesToTitleCase(List<Drink> drinks)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            foreach (var drink in drinks)
            {
                if (drink.Name != null)
                    drink.Name = textInfo.ToTitleCase(drink.Name.ToLower());
            }
        }
    }
}
