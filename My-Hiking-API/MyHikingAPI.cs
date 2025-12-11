using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MyHikingAPI.Models;
using MyHikingAPI.Services;
using System;
using System.Linq;
using System.Net.Http;

namespace My.Functions
{
    public class MyHikingAPI
    {
        private readonly IMountainService _mountainService; // Domain service used to retrieve mountains 
        // private readonly HttpClient _client;
        // private readonly ILogger<MyHikingAPI> _log; // Class-level logger 

        // Constructor. Dependencies are injected by the functions DI container 

        public MyHikingAPI(IMountainService mountainService 
,
            IHttpClientFactory httpClientFactory,
            ILogger<MyHikingAPI> log)
        {
            // Stoe dependencies for later use
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
            
            // Call the injected mountain service to retieve all available mountains & Log the name of the retrieved mountains 
            var mountains = _mountainService.GetAllMountains();
            log.LogInformation($"Here are the list of mountains: {mountains.Select(m => m.Name).ToList()}");

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
