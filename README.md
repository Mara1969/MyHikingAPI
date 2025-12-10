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
<ItemGroup>
    <None Update="Data\mountains.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
</ItemGroup>

- The FluentAssertions package was added to the test project to write expressive and readable assertions

### Running Tests 
- Use the Testing icon on the left-hand side (it looks like a flask)


## Running the app
*To be added*
