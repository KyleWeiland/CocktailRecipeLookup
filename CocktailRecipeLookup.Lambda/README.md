# CocktailRecipeLookup.Lambda

AWS Lambda function for the Cocktail Recipe Lookup API - serverless replacement for the Elastic Beanstalk .NET API.

## Overview

This is a .NET 8 Lambda function that provides a REST API for searching cocktail recipes. It integrates with API Ninja's cocktail API and is designed to be deployed with AWS API Gateway.

## Architecture

- **Runtime**: .NET 8
- **Handler**: `CocktailRecipeLookup.Lambda::CocktailRecipeLookup.Lambda.Function::FunctionHandler`
- **Memory**: 256 MB recommended
- **Timeout**: 30 seconds
- **Integration**: API Gateway (REST API with Lambda Proxy Integration)

## API Endpoints

All endpoints are prefixed with `/api/drinks`:

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/drinks/Test` | Health check endpoint |
| GET | `/api/drinks/ByName/{name}` | Search cocktails by name |
| POST | `/api/drinks/ByIngredients` | Search cocktails by ingredients list |

### Example Requests

**Health Check:**
```bash
GET /api/drinks/Test
Response: {"message":"Success!"}
```

**Search by Name:**
```bash
GET /api/drinks/ByName/margarita
Response: [{"name":"Margarita","ingredients":[...],"instructions":"..."}]
```

**Search by Ingredients:**
```bash
POST /api/drinks/ByIngredients
Body: ["tequila", "lime"]
Response: [{"name":"Margarita","ingredients":[...],"instructions":"..."}]
```

## Features

- **CORS Support**: Enabled for all origins (can be restricted in production)
- **Error Handling**: Comprehensive error responses with logging
- **Data Sanitization**:
  - Converts centiliters (cl) to milliliters (ml)
  - Formats drink names to title case
- **API Key Management**: Securely retrieves API Ninja key from AWS Systems Manager Parameter Store
- **Performance Optimization**:
  - Caches API key to reduce SSM calls
  - Reuses HttpClient across invocations
  - Cold start optimization with `PublishReadyToRun`

## Prerequisites

- .NET 8 SDK
- AWS CLI configured
- Amazon.Lambda.Tools:
  ```bash
  dotnet tool install -g Amazon.Lambda.Tools
  ```

## Local Development

### Build
```bash
dotnet build
```

### Run Tests (if tests are added)
```bash
dotnet test
```

### Package for Deployment
```bash
dotnet lambda package -o publish/function.zip
```

## Deployment

See the [LAMBDA_MIGRATION_GUIDE.md](../LAMBDA_MIGRATION_GUIDE.md) in the root directory for complete deployment instructions.

Quick deploy:
```bash
# Build and package
dotnet lambda package -o publish/function.zip

# Deploy using AWS CLI
aws lambda update-function-code \
  --function-name CocktailRecipeApi \
  --zip-file fileb://publish/function.zip \
  --region us-east-1
```

## Environment Configuration

### Required AWS Permissions

The Lambda execution role needs:
- `AWSLambdaBasicExecutionRole` (for CloudWatch Logs)
- SSM Parameter Store read access for `api-ninja-key` parameter

### AWS Systems Manager Parameter

The function expects a parameter in SSM Parameter Store:
- **Name**: `api-ninja-key`
- **Type**: SecureString
- **Region**: us-east-1
- **Value**: Your API Ninja API key

## Project Structure

```
CocktailRecipeLookup.Lambda/
├── Function.cs                          # Main Lambda handler with routing logic
├── Models/
│   └── Drink.cs                        # Drink data model
├── Services/
│   └── DrinksService.cs                # Business logic for API calls
├── CocktailRecipeLookup.Lambda.csproj  # Project file
├── aws-lambda-tools-defaults.json      # Lambda deployment configuration
└── README.md                           # This file
```

## Dependencies

- **Amazon.Lambda.Core** (2.2.0): Core Lambda functionality
- **Amazon.Lambda.APIGatewayEvents** (2.7.0): API Gateway event handling
- **Amazon.Lambda.Serialization.SystemTextJson** (2.4.0): JSON serialization
- **AWSSDK.SimpleSystemsManagement** (3.7.202.4): SSM Parameter Store access

## Monitoring

### CloudWatch Logs

Lambda automatically logs to CloudWatch:
- Log group: `/aws/lambda/CocktailRecipeApi`
- Logs include request path, errors, and stack traces

### Metrics

Monitor these Lambda metrics:
- **Invocations**: Total number of requests
- **Duration**: Function execution time
- **Errors**: Failed invocations
- **Throttles**: Concurrent execution limit reached

## Troubleshooting

### Function returns 500 errors
1. Check CloudWatch Logs for error details
2. Verify SSM parameter `api-ninja-key` exists and is accessible
3. Ensure IAM role has correct permissions

### CORS errors in browser
- Verify Lambda returns CORS headers in all responses
- Check API Gateway CORS configuration
- Ensure OPTIONS method is configured

### Slow response times
- First request after idle period will have cold start (~1-2 seconds)
- Subsequent requests are fast (~100-300ms)
- Consider Provisioned Concurrency for production if needed (adds cost)

## License

See root project LICENSE file.

## Support

For issues or questions, refer to the main project README or create an issue in the repository.
