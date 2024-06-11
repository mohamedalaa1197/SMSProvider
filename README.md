# SMS Microservice Application

## Overview
This Microservice Application enables the sending of SMS messages using different SMS providers based on specific requirements.

## Getting Started

### Prerequisites
- **.NET SDK**: Ensure you have the .NET SDK installed on your machine.
- **SQL Server**: Set up a local MS SQL database.
- **SMS PRoviders User Secrets**

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://your-repo-url.git
   cd your-repo-directory
2. Configure the Database Connection
3. - Update the connection string in the appsettings.json file with your local MS SQL database connection string.
   - ```bash
       "ConnectionStrings": {
        "DefaultConnection": "your_local_db_connection_string"
      }
4. -Run the Application
   - Use the following command to run the project. The necessary tables will be created automatically, and seeded SMS providers will be added.
   - ```bash
       dotnet run
5. -Testing the APIs
   -Use Swagger to test the various API endpoints provided by the service.

