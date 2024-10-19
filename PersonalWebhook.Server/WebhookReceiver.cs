using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net;

namespace PersonalWebhook.Server
{
    public interface IWebhookReceiver
    {
        Task<object> ProcessPostRequest(HttpRequest request);
        Task<object> ProcessGetRequest(HttpRequest request);
        string GetSessionId();
        string GetBaseUrl();
    }
    public class WebhookReceiver(
        IHttpContextAccessor httpContextAccessor,
        ILogger<WebhookReceiver> logger,
        IHubContext<SignalRHub> signalRHub,
        IConfiguration configuration
    ) : IWebhookReceiver
    {
        private string SessionId = Guid.NewGuid().ToString();
        public string GetBaseUrl() => configuration["BaseUrl"] ?? string.Empty;
        public string GetSessionId() => SessionId;
        /// <summary>
        /// Writes the POST request body to the console and returns JSON
        /// </summary>
        public async Task<object> ProcessPostRequest(HttpRequest request)
        {
            await ProcessRequestDetails(request);

            return new { Message = "CUSTOM POST RESPONSE" };
        }
  
        public async Task<object> ProcessGetRequest(HttpRequest request)
        {
            await ProcessRequestDetails(request);

            return new { Message = "CUSTOM GET RESPONSE" };
        }
        /// <summary>
        /// Format the request and send to the client.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task ProcessRequestDetails(HttpRequest request)
        {
            Dictionary<string, object?> requestDetails = GetRequestDetails(request);
            string requestDetailsJson = JsonConvert.SerializeObject(requestDetails, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            string requestBody = await GetRequestBody(request);
            object requestJson = new
            {
                SessionId,
                RequestDetails = requestDetails,
                RequestHeaders = request.Headers,
                RequestBody = requestBody
            };
            string requestJsonString = JsonConvert.SerializeObject(requestJson, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }); 

            logger.LogDebug("===REQUEST DETAILS==={NewLine}{requestDetailsJson}",Environment.NewLine, requestDetailsJson);
            logger.LogDebug("===REQUEST HEADERS==={NewLine}{requestHeaders}", Environment.NewLine, GetRequestHeadersJson(request));
            logger.LogDebug("===REQUEST JSON==={NewLine}{requestJsonString}", Environment.NewLine, requestJsonString);

            IncomingRequestBase incomingRequest = new(SessionId)
            {
                Method = request.Method,
                RequestBody = requestBody,
                AbsoluteUri = request.GetDisplayUrl(),
                QueryString = request.QueryString.ToString(),
                RemoteIpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                RemotePort = httpContextAccessor.HttpContext.Connection.RemotePort
            };
            foreach(var header in request.Headers)
            {
                incomingRequest.Headers.Add(new {
                    header.Key,
                    Value = header.Value.ToString()
                });
            }
            await signalRHub.Clients.All.SendAsync("ReceivedRequest",incomingRequest);
        }

        private static async Task<string> GetRequestBody(HttpRequest request)
        {
            using StreamReader stream = new(request.Body);
            return await stream.ReadToEndAsync();
        }
        private static string GetRequestHeadersJson(HttpRequest request) => JsonConvert.SerializeObject(request.Headers, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        });
        private static Dictionary<string,string> GetRequestHeadersDictionary(HttpRequest request)
        {
            Dictionary<string, string> headers = [];
            foreach(var header in request.Headers)
            {
                headers.TryAdd(header.Key, header.Value.ToString());
            }
            return headers;
        }
        private Dictionary<string, object?> GetRequestDetails(HttpRequest request)
        {
            Dictionary<string, object?> requestDetails = new()
            {
                [nameof(request.ContentType)] = request.ContentType,
                [nameof(request.ContentLength)] = request.ContentLength,
                [nameof(request.Protocol)] = request.Protocol,
                [nameof(request.Method)] = request.Method,
                ["AbsoluteUri"] = request.GetDisplayUrl(),
                [nameof(request.Scheme)] = request.Scheme,
                [nameof(request.HttpContext.Request.Host)] = request.HttpContext.Request.Host.Value,
                [nameof(request.Path)] = request.Path.ToString(),
                [nameof(request.QueryString)] = request.QueryString.ToString(),
                [nameof(httpContextAccessor.HttpContext.Connection.RemoteIpAddress)] = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                [nameof(httpContextAccessor.HttpContext.Connection.RemotePort)] = httpContextAccessor.HttpContext.Connection.RemotePort
            };
            return requestDetails;
        }

    }
}