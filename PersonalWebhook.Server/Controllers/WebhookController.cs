using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PersonalWebhook.Server
{
    [ApiController]
    [Route("")]
    public class WebhookController(
        IWebhookReceiver webhookService
        ) : Controller
    {
        [Route("~/")]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost("{sessionUUID}")]
        public async Task<IActionResult> ProcessPostRequest(string sessionUUID) {
            object response = await webhookService.ProcessPostRequest(HttpContext.Request);
            return Ok(response);
        }

        [HttpGet("{sessionUUID}")]
        public async Task<IActionResult> ProcessGetRequest(string sessionUUID)
        {
            object response = await webhookService.ProcessGetRequest(HttpContext.Request);
            return Ok(response);
        }
    }
}
