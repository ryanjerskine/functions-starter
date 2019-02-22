using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using FunctionsStarter.Infrastructure.IoC;

namespace FunctionsStarter.Features.SetIndividualInfo
{
    public static class Function
    {
        [FunctionName("SetIndividualInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/set-individual-info")] HttpRequest req,
            [Inject]IMediator mediatr,
            [Inject] CommandValidator commandValidator,
            ILogger log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var uicommand = JsonConvert.DeserializeObject<UICommand>(body as string);
            var command = uicommand.ToCommand();
            await mediatr.Send(uicommand.ToCommand());
            return new OkResult();
        }
    }
}