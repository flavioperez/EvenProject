using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NaturalEven
{
    public static class Function1
    {
        [FunctionName("CheckIfEven")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string param = req.Query["natural"];

            int? natural = StringToNullableNatural(param);

            if (natural == null)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                natural = natural ?? data?.int1;
            }

            if (natural == null)
            {
                log.LogInformation("Returning bad request for natural.");
                return new BadRequestObjectResult("Please pass a natural number (positve integer) natural on the query string or in the request body");
            }

            bool isEven = natural % 2 == 0;
            log.LogInformation($"Returning Ok with the value of {isEven} for input {natural}");

            return new OkObjectResult($"Is Even: {isEven}");
        }

        private static int? StringToNullableNatural(string input)
        {
            int outValue;
            int? value = null;

            
            if (int.TryParse(input, out outValue))
            {
                value = outValue < 1 ? (int?)null : outValue;
            }

            return value;
        }
    }
}
