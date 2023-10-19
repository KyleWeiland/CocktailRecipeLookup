using CocktailRecipeLookup.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CocktailRecipeLookup.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public DrinksController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchDrinks(string query)
        {
            var apiUrl = $"YOUR_NINJAS_COCKTAIL_API_ENDPOINT_HERE?search={query}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Error fetching drinks.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var drinks = JsonConvert.DeserializeObject<List<Drink>>(content); // Adjust based on actual API response structure

            return Ok(drinks);
        }
    }
}

