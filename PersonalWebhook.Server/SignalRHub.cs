using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace PersonalWebhook.Server
{
    public class SignalRHub(
        ILogger<SignalRHub> logger
        ) : Hub
    {
        public async Task SendMessage(IncomingRequestBase message)
        {
            logger.LogDebug(JsonConvert.SerializeObject(message));
            await Clients.All.SendAsync("ReceivedRequest", message);
        }
    }
}
