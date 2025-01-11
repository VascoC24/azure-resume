using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Company.Function
{
    public class GetResumeCounter
    {
        private readonly ILogger<GetResumeCounter> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public GetResumeCounter(ILogger<GetResumeCounter> logger)
        {
            _logger = logger;
            string connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
            _cosmosClient = new CosmosClient(connectionString);
            _container = _cosmosClient.GetContainer("AzureResume", "Counter");
        }

        [Function("GetResumeCounter")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            try
            {
                // Retrieve the document with id "1"
                var response = await _container.ReadItemAsync<JObject>("1", new PartitionKey("1"));
                var currentItem = response.Resource;

                // Extract and increment the count
                int count = (int)currentItem["count"];
                count++;

                // Update the count in the document
                currentItem["count"] = count;

                // Save the updated document back to Cosmos DB
                await _container.ReplaceItemAsync(currentItem, "1", new PartitionKey("1"));

                // Return the updated count
                return new OkObjectResult(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update counter: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
