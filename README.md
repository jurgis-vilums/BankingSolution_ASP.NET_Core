# Banking Solution REST API

## Overview

This project is a simple REST API for a banking application that allows users to perform basic banking operations such as creating accounts, making deposits, and transferring funds.

## Technology Stack

- **Language:** C#
- **Framework:** .NET 8.0
- **Database:** MongoDB Atlas
- **Design Patterns:** Service Repository Controller
- **Testing:** Moq, FluentAssertions

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- MongoDB Atlas account

### Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/jurgis-vilums/BankingSolution_Homework
   cd banking-solution
   ```

2. **Configure MongoDB Atlas:**

   - Recieve database credentials from the author.
   - Update the connection string in `appsettings.json`:

     ```json
     {
       "MongoDbSettings": {
         "ConnectionString": "your-mongodb-connection-string",
         "DatabaseName": "BankingDb"
       }
     }
     ```

3. **Restore the dependencies:**

   ```bash
   dotnet restore
   ```

4. **Build the project:**

   ```bash
   dotnet build
   ```

5. **Run the application:**

   ```bash
   dotnet run
   ```

6. **API Documentation:**
   - Once the application is running, navigate to `http://localhost:5000/` to view the API documentation and test the endpoints.

## Design Choices

- **Service Repository Controller Pattern:** This design pattern helps in organizing the code in a clean and maintainable way. The Controller handles the HTTP requests, the Service contains the business logic, and the Repository handles data access.
- **MongoDB Atlas:** A highly scalable and flexible NoSQL database that allows for easy setup and maintenance.
- **Testing with Moq and FluentAssertions:** Ensures that the code is robust and reliable by providing mocking capabilities and fluent syntax for assertions.
