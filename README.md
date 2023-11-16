# Cocktail Recipe Lookup

## Frontend
- ReactJS SPA
- Connected to: Amazon CloudFront
- Function: User interface for cocktail recipe search.

## Backend
- .NET 6 REST API
- Connected to: API Ninja (External API)
- Connected from: AWS Elastic Beanstalk
- Function: Processes requests and interacts with external API for data.

## AWS Services
### Amazon S3
- Stores: ReactJS SPA
- Connected to: Amazon CloudFront
### Amazon CloudFront
- Delivers: ReactJS SPA
- Connected from: Amazon S3
### AWS Elastic Beanstalk
- Contains: EC2, Load Balancer
- Hosts: .NET 6 REST API

## Version/Source Control and CI/CD Pipeline
### GitHub Repository
- Contains: Codebase for both frontend and backend.
- Connected to: GitHub Actions
### GitHub Actions
- Functions: Builds and publishes frontend to Amazon S3, and backend to AWS Elastic Beanstalk.
