using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MyHikingAPI.Services;
using System.Collections.Generic;
using MyHikingAPI.Models;
using System;



namespace My.Functions
{
    public class MyHikingAPI
    {
        private readonly IMountainService _mountainService; // Domain service used to retrieve mountains 
        // private readonly HttpClient _client;
        // private readonly ILogger<MyHikingAPI> _log; // Class-level logger 

        // Constructor. Dependencies are injected by the functions DI container 

        public MyHikingAPI(IMountainService mountainService)
        {
            // Store dependencies for later use
            this._mountainService = mountainService; 
            // this._client = httpClientFactory.CreateClient();
           // _log = log; 
        }
    
        [FunctionName("MyHikingAPI")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            // Call the injected mountain service to retieve all available mountains & Log the number of the retrieved mountains 
            var mountains = _mountainService.GetAllMountains();
            // log.LogInformation($"Here are the list of mountains: {mountains.Select(m => m.Name).ToList()}");
            log.LogInformation($"Inserting {mountains.Count} mountains into the database.");

            try
            {
                await _mountainService.InsertMountainsDataToDbAsync(mountains);
                log.LogInformation("Inserted mountains data into the database.");
            }
            catch (Exception dbInsertEx)
            {
                log.LogError(dbInsertEx, "Error inserting mountains data into the database");
                return new ObjectResult(new {error = "Failed to insert mountains data into the database."}) 
                // default status code for ObjectResult is 200 OK which means the request was successful - this needs to be changed to indicate an error
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }; 
            }

            log.LogInformation("Retrieving mountains data from the database.");
            try
            {
                List<Mountain> mountainsDataFromDb = await _mountainService.GetAllMountainsFromDbAsync();
                log.LogInformation($"Retrieved {mountainsDataFromDb.Count} mountains from the database.");
            }
            catch (Exception dbRetrieveEx)
            {
                log.LogError(dbRetrieveEx, "Error retrieving mountains data from the database");
                return new ObjectResult(new {error = "Failed to retrieve mountains data from the database."})
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync(); // Asynchronous read of request body
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
        
    
    }
}
