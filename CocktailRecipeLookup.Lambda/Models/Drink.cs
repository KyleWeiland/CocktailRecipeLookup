using System.Text.Json.Serialization;

namespace CocktailRecipeLookup.Lambda.Models
{
    public class Drink
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("ingredients")]
        public List<string>? Ingredients { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }
    }
}
