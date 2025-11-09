## Book Lending Application

A C#/.NET application for managing a book lending library, supporting both in-memory and AWS DynamoDB storage. 
The solution is layered for testability and extensibility, and includes comprehensive unit tests.

---

## API Endpoints

GET /api/Books - Get all books
GET /api/Books/{id} - Get a book by Id
POST /api/Books - Add a new book
POST /api/Books/{id}/checkout - Check out a book
POST /api/Books/{id}/return - Return a book

## Database Configuration

### Local Development uses In-Memory database   

### Production deployment uses AWS DynamoDB database

## Environment Variables

# Local Development (automatically uses in-memory DB)
ASPNETCORE_ENVIRONMENT=Development

# Production (configure for DynamoDB)
AWS.Profile: your-aws-profile-name
AWS.Region: your-aws-region

---

## How to Run the App Locally

### Prerequisites
- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download)
- (Optional) [AWS CLI](https://aws.amazon.com/cli/) and credentials if you want to use DynamoDB locally

### Steps

1. **Clone the repository from below path** 

    - https://github.com/pooja15131/BookLendingApplication.git

2. **Restore dependencies**
	- nuget packages will be restored automatically when building the solution in Visual Studio.

3. **Configuration**
    - By default, the app uses the in-memory repository. To use DynamoDB, update the DI configuration or appsettings as described in the code comments.

4. **Build the solution**
   
5. **Run the BookLendingApplication project**
    - API available at http://localhost:5000/swagger
        
6. **Run tests**

    

## How to Deploy to AWS

### Prerequisites
- AWS account and credentials
- [AWS CLI](https://aws.amazon.com/cli/)
- [AWS Toolkit for Visual Studio](https://aws.amazon.com/visualstudio/)

### Steps

1. **Configure AWS Credentials**
    - Set up your AWS credentials using the AWS CLI.
    - Configure correct AWS profile and region.

2. **Deploy infrastructure (using CloudFormation)**
    - Create Stack using AWS CloudFormation with the provided template 'BookLending.yml' in the `deployment` folder.
    
3. **Verify Deployment:**
    - Check the AWS Console for your service status.
    - Test the API endpoint or application URL, API available at https://qs3npkjlnk.execute-api.ap-south-1.amazonaws.com/Prod/
    
---

    
## Unit Tests Coverage
### Open BookLendingApplication.Tests/StrykerOutput/{latest-dated-report}/reports/mutation-report.html
Test Coverage: 80.77%
Total Tests: 30

### Coverage by Component
Controller Tests: API endpoint testing with mocked services
Service Tests: Business logic testing with mocked repositories
Repository Tests: In memory data access testing
Integration Tests: End-to-end API testing

---


## Technologies
### Backend
    Framework: ASP.NET Core 8.0
    Database: DynamoDB (AWS) / In-Memory (Local)
    Architecture: Clean Architecture with Repository Pattern
### AWS Services
    Compute: AWS Lambda
    API: API Gateway
    Database: DynamoDB
    Storage: S3 (deployment packages)
### Testing
    Framework: nUnit
    Mocking: fakeiteasy
### DevOps
    CI/CD: GitHub Actions
    Infrastructure: CloudFormation
    Packaging: AWS Lambda Tools
    Documentation: OpenAPI/Swagger

---


## API Documentation
### Swagger/OpenAPI
    JSON: deployment/swagger.json
    Interactive UI: Available at /swagger endpoint during development

---

## Design Decisions

- **Repository Pattern:**  
  The app uses the repository pattern to abstract data access, making it easy to switch between in-memory and DynamoDB storage. This improves testability and flexibility.

- **In-Memory vs. DynamoDB:**  
  - *In-Memory Repository* is used for local development and testing. It is not thread-safe by default; for production, a thread-safe collection is recommended.
  - *DynamoDB Repository* is used for production and cloud deployments. It requires AWS credentials and proper table setup.

- **Service Layer:**  
  Business logic is encapsulated in the service layer, keeping controllers and UI code clean.

- **Unit Testing:**  
  All business logic and data access are covered by unit tests using NUnit and FakeItEasy. This ensures reliability and supports refactoring.

- **Async Programming:**  
  All repository and service methods are asynchronous for scalability and responsiveness.

- **Configuration:**  
  The storage implementation can be swapped via dependency injection, making the application adaptable to different environments.

- **Trade-offs:**  
  - Using in-memory storage for local development is fast and simple, but not persistent or thread-safe.
  - DynamoDB integration adds cloud scalability but requires AWS setup.
  - The solution favors clarity and extensibility over micro-optimizations, as it is intended as a sample project.


---