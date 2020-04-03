using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Covid19Functions
{
    public static class GetPositionsFunction
    {
        [FunctionName("get-positions")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "cosmosdb-covid19", collectionName: "patients",
                ConnectionStringSetting = "CosmosDBConnectionString",
                SqlQuery = "select c.position, c.isSuspected from c")]
            IEnumerable<dynamic> positions,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request: {req.Path}");
            return new OkObjectResult(positions);
        }
    }
}
