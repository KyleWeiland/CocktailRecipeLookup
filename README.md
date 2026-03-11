# Cocktail Recipe Lookup

A serverless cocktail recipe search application built with React and .NET, deployed on AWS.

## Architecture

```
User → CloudFront → S3 (React SPA)
                      ↓
                API Gateway → Lambda (.NET 8)
                                ↓
                          API Ninja (External API)
```

## Components

### Frontend
- **Technology**: ReactJS SPA
- **Hosting**: Amazon S3 + CloudFront
- **Function**: User interface for searching cocktail recipes by name or ingredients

### Backend
- **Technology**: .NET 8 Lambda Function
- **API Gateway**: REST API with Lambda proxy integration
- **External API**: API Ninja Cocktail API
- **Function**: Serverless API that fetches and processes cocktail data
- **Features**:
  - Search by cocktail name
  - Search by ingredients
  - Unit conversion (centiliters to milliliters)
  - Name formatting (title case)

## AWS Services

### Compute & API
- **AWS Lambda**: Serverless function hosting (.NET 8 runtime)
- **API Gateway**: REST API endpoint management

### Storage & Delivery
- **Amazon S3**: Static website hosting for React frontend
- **Amazon CloudFront**: Global CDN for frontend delivery

### Configuration & DNS
- **AWS Systems Manager (SSM)**: Secure parameter storage for API keys
- **Route 53**: DNS management

## CI/CD Pipeline

### GitHub Actions Workflows

**Frontend Deployment** (`.github/workflows/frontend-build-deploy.yml`)
- Triggers on push to `main` when `cocktail-reciple-lookup-ui/` changes
- Builds React app
- Deploys to S3
- Invalidates CloudFront cache

**Backend Deployment** (`.github/workflows/backend-build-deploy.yml`)
- Triggers on push to `main` when `CocktailRecipeLookup.Lambda/` changes
- Builds .NET 8 Lambda function
- Packages deployment zip
- Updates Lambda function code

## Documentation

- **[CocktailRecipeLookup.Lambda/README.md](./CocktailRecipeLookup.Lambda/README.md)**: Lambda function documentation

## Local Development

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- AWS CLI (for deployment)
- AWS Lambda Tools: `dotnet tool install -g Amazon.Lambda.Tools`

### Frontend
```bash
cd cocktail-reciple-lookup-ui
npm install
npm start
```

### Backend (Lambda)
```bash
cd CocktailRecipeLookup.Lambda
dotnet build
dotnet lambda package -o publish/function.zip
```

## Environment Variables

### AWS Systems Manager Parameters
- **api-ninja-key**: API Ninja API key (SecureString in us-east-1)

### GitHub Secrets (for CI/CD)
- **AWS_ACCESS_KEY_ID**: AWS access key
- **AWS_SECRET_ACCESS_KEY**: AWS secret key

## API Endpoints

Base URL: `https://p7d8ipi548.execute-api.us-east-1.amazonaws.com/prod/api/`

- **GET** `/Drinks/Test` - Health check
- **GET** `/Drinks/ByName/{name}` - Search by cocktail name
- **POST** `/Drinks/ByIngredients` - Search by ingredients (body: `["ingredient1", "ingredient2"]`)

## Live Application

- **Frontend**: https://kyle-weiland.com
- **API**: https://p7d8ipi548.execute-api.us-east-1.amazonaws.com/prod/api/

## License

See LICENSE file for details.
