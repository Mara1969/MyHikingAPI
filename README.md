# My-Hiking-API

An Azure Functions project that provides endpoints to retrieve data from JSON files. This project demonstrates how to structure an Azure Functions app using models, services, and static data.

## Getting Started 

### Prerequisites
Before running this project, ensure you have:
- **.NET SDK 8.0**
    https://dotnet.microsoft.com/download
- **Azure Functions Core Tools** 
    The Azure Functions extension for Visual Studio Code 
- **C# Dev Kit**
    C# extension for Visual Studio Code
Download the below NuGet packages or ensure you have them: 
- **Newtonsoft.Json**
    For JSON serialization/deserialization
- **Microsoft.Azure.WebJobs.Extensions.Http**
    for HTTP triggers 
- **Microsoft.AspNetCore.Mvc** 
    For returning HTTP responses 
- **Microsoft.NET.sdk.Functions**
    for core Azure functions 
- **Microsoft.Extensions.Http**
    for IHttpClientFactory (used in DI)
- **Microsoft.Extensions.DependencyInjection**
    for DI 
- **Microsoft.Data.SqlClient**
    for using Dapper to query SQL server 

## Features 
- HTTP-triggered Azure Function
- Organised into 'Models', 'Services', and 'Data' folders
- 'JsonFilesData.cs' contains a static class and a static method, 'GetData', which reads a JSON file and returns it as a list of 'T' object e.g. if the filename specified is 'mountains.json' then this method will return a list of mountains 
- 'MountainServices.cs' contains a public class of mountains with a method which uses the 'GetData' method to return a list of mountains 
- The main entry point for the function app is: 'MyHikingAPI.cs'

## Dependency Injection 
This project uses DI to register and resolve services in Azure Functions. DI makes the app easier to maintain and extend (e.g., adding a database)

### DI Setup 
- Services are registered in `Startup.cs`. Azure Functions (in‑process model) uses `FunctionsStartup`. The `Configure` method registers services and enables `IHttpClientFactory`. 

### How DI is Used Inside the Function 
- Constructor injection provides IMountainService and IHttpClientFactory.
- The function’s Run method is instance (not static) so it can access injected fields.
- HttpClient is created via IHttpClientFactory to avoid socket exhaustion and enable handler reuse.

### Service Lifetimes 
- Singleton: One instance for the entire app lifetime (good for stateless services)
- Scoped: One instance per function invocation (good for per-request state like DbContext)
- Transient: New instance every time requested (lightweight helpers)

## Testing 

### Setting Up Tests 
- A test project was created (MyHikingAPI.Tests), and the My-Hiking-API project was added as a reference project. This allows the test project to point to the main project
- To ensure the tests run successfully, the My-Hiking-API.csproj file was updated to include an ItemGroup that specifies the location of the mountains.json file. Without this update, the tests would fail. Below is an example of the ItemGroup added to the .csproj file:
```
<ItemGroup>
    <None Update="Data\mountains.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

- The FluentAssertions package was added to the test project to write expressive and readable assertions

### Running Tests 
- Use the Testing icon on the left-hand side (it looks like a flask)

## Asynchronous Programming in Azure Functions 
This project uses async/await for non-blocking I/O operations, which is a best practice in Azure Functions.

### What is an I/O operation?
- I/O (input/output) refers to reading or writing data to external resources like files, network streams, or databases.
- These operations are slower than CPU work because they depend on external systems.

### Why async/await?
- Non-blocking I/O: Reading the HTTP request body is an I/O operation. Using `await` means the function doesn't block while waititing for the data.
- Improved scalability: Serverless environments benefit from freeing threads during I/O, enabling more concurrent executions.
- Cleaner code: Async/await avoids complex callbacks and makes asynchronous code easier to read.

## Creating a Database Project and Connecting to a Database 
A SQL database project is a local representation of SQL objects that comprise the schema for a single database, such as tables, stored procedures, or functions. 

Dapper ORM (object relational mapping) is used for database access, mapping queries to objects.

### Prerequisites 
- Install SQL Database Projects extension for VS Code 
- Install SQL Server Management Studio 
    https://learn.microsoft.com/en-us/ssms/install/install

### Connecting with SSMS (Database Engine)
Follow the istructions in this doc to connect to the database engine: 
    https://learn.microsoft.com/en-us/sql/relational-databases/lesson-1-connecting-to-the-database-engine?view=sql-server-ver17&source=recommendations

### SQL Server Connectivity 
If SQL Server fails to start or connect (e.g., Error 802: insufficient memory, Error 1069: logon failure, or SSL certificate issues), follow these steps in command prompt (run as admin):
- Check if the SQL server is running: 
    sc query MSSQLSERVER
- If it is not running, try switching to LocalSystem (ignore this step if it is already set up to localsystem): 
    sc.exe config MSSQLSERVER obj= "LocalSystem" password= ""
- Start the server, if this starts successfully you should see The SQL Server (MSSQLSERVER) service was started successfully: 
    net start MSSQLSERVER
    If LocalSystem still fails with 1069: 
    sc.exe config MSSQLSERVER obj= "NT AUTHORITY\NetworkService" password= ""
- Grant this account access to the SQL folders: 
    icacls "C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA" /grant "NT AUTHORITY\NETWORK SERVICE:(OI)(CI)M"
    icacls "C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\Log"  /grant "NT AUTHORITY\NETWORK SERVICE:(OI)(CI)M"
    net start MSSQLSERVER
- Connect with sqlcmd and trust the server certificate: 
    sqlcmd -S lpc:localhost -E -C -d master -Q "SELECT @@SERVERNAME;"
- Start SQL server in minimal configuration (if memory error occurs): 
    sqlcmd -S lpc:localhost -E -C -d master -Q "EXEC sp_configure 'show advanced options',1; RECONFIGURE WITH OVERRIDE; EXEC sp_configure 'max server memory (MB)',1024; EXEC sp_configure 'min server memory (MB)',0; RECONFIGURE WITH OVERRIDE;"

### SQL Database Project
To create a new project, add objects to the project, build and deploy the project follow the instructions in this doc:
    https://learn.microsoft.com/en-us/sql/tools/sql-database-projects/get-started?view=sql-server-ver17&pivots=sq1-visual-studio-code 

### Connecting to the Database via Dapper 
For creating the MyOptions class, local.settings.json file and registering the SqlDatabaseOptions with DI follow the instructions in this doc:
    https://docs.azure.cn/en-us/azure-functions/functions-dotnet-dependency-injection

- Create a `Configuration` folder within models, and a `SqlDatabaseOptions` class 
- Create a local.settings.json file on the same level as `My-Hiking-API.csproj` 
- In `Startup.cs` use the doc above to register the SqlDatabaseOption with DI to inject `IOptions<SqlDatabaseOptions>` anywhere to access settings like the database connection string
- In the services folder create a `DatabaseService` class (in the doc this class is HttpTrigger) and a `IDatabaseService` interface 

### Database Seeding and Data Flow (JSON -> SQL -> API)
This project uses two distinct services to separate concerns between reading data and persisting data. Understanding this separation is key to understanding how data flows through the application.

- JsonReader -> Reads static JSON files from disk
    No Database interaction occurs at this stage
- MountainService -> Provides domain data (mountains) from JSON
    No Database interaction occurs at this stage. The service simply returns a list of Mountain objects 
- DatabaseService -> Persists and retrieves data from SQL Server
    The database connection is scoped to a `using` block to ensure it is safely disposed. The service simply accepts domain objects and persists them
- Azure Function -> Orchestrates the flow between services

## Running the app
*To be added*
