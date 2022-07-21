
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace BooksReader.Function
{
    public static class Functions
    {
        [FunctionName("Negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "favoriteBooksHub")] SignalRConnectionInfo connectionInfo, ILogger log)
        {
            log.LogInformation("Returning connection:" + connectionInfo.Url + "" + connectionInfo.AccessToken);
            return connectionInfo;
        }

    }
}
