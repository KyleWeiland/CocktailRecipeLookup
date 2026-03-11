using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CocktailRecipeLookup.Lambda.Services;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CocktailRecipeLookup.Lambda;

public class Function
{
    private static readonly DrinksService _drinksService = new DrinksService();

    /// <summary>
    /// Lambda function handler for API Gateway proxy integration
    /// </summary>
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Request: {request.HttpMethod} {request.Path}");

            // CORS headers
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Allow-Headers", "Content-Type,X-Api-Key" },
                { "Access-Control-Allow-Methods", "GET,POST,OPTIONS" }
            };

            // Handle OPTIONS requests for CORS preflight
            if (request.HttpMethod == "OPTIONS")
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Headers = headers,
                    Body = string.Empty
                };
            }

            // Route based on path and method
            var path = request.Path?.TrimEnd('/') ?? "";
            var method = request.HttpMethod;

            // GET /api/drinks/Test
            if (method == "GET" && path.EndsWith("/Test", StringComparison.OrdinalIgnoreCase))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Headers = headers,
                    Body = JsonSerializer.Serialize(new { message = "Success!" })
                };
            }

            // GET /api/drinks/ByName/{name}
            if (method == "GET" && path.Contains("/ByName/", StringComparison.OrdinalIgnoreCase))
            {
                var name = path.Split("/ByName/", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                if (string.IsNullOrEmpty(name))
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Headers = headers,
                        Body = JsonSerializer.Serialize(new { error = "Name parameter is required" })
                    };
                }

                var drinks = await _drinksService.GetDrinksByNameAsync(name);
                if (drinks.Count == 0)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 404,
                        Headers = headers,
                        Body = JsonSerializer.Serialize(new { message = "No drinks found" })
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Headers = headers,
                    Body = JsonSerializer.Serialize(drinks)
                };
            }

            // POST /api/drinks/ByIngredients
            if (method == "POST" && path.EndsWith("/ByIngredients", StringComparison.OrdinalIgnoreCase))
            {
                List<string>? ingredients = null;

                if (!string.IsNullOrEmpty(request.Body))
                {
                    ingredients = JsonSerializer.Deserialize<List<string>>(request.Body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                if (ingredients == null || ingredients.Count == 0)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Headers = headers,
                        Body = JsonSerializer.Serialize(new { error = "Ingredients list is required" })
                    };
                }

                var drinks = await _drinksService.GetDrinksByIngredientsAsync(ingredients);
                if (drinks.Count == 0)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 404,
                        Headers = headers,
                        Body = JsonSerializer.Serialize(new { message = "No drinks found" })
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Headers = headers,
                    Body = JsonSerializer.Serialize(drinks)
                };
            }

            // Route not found
            return new APIGatewayProxyResponse
            {
                StatusCode = 404,
                Headers = headers,
                Body = JsonSerializer.Serialize(new { error = "Route not found" })
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error: {ex.Message}");
            context.Logger.LogError($"Stack trace: {ex.StackTrace}");

            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                },
                Body = JsonSerializer.Serialize(new
                {
                    error = "Internal server error",
                    message = ex.Message
                })
            };
        }
    }
}
