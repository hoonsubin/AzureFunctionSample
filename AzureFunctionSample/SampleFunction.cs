using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionSample
{
    public static class SampleFunction
    {
        // the function name which is the endpoint (ex: http://localhost:7071/api/SampleFunction)
        [FunctionName("SampleFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            // read the request parameter (http://localhost:7071/api/<function-name>?name=<value>)
            string name = req.Query["name"];

            // read the HTTP request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // convert the request body (JSON) into a C# object
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // set the name from either the request parameter or the body
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
