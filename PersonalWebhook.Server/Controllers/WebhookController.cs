﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PersonalWebhook.Server
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class WebhookController(
        IWebhookReceiver webhookService
        ) : Controller
    {
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

        [HttpGet]
        [Route("[action]")]
        public string GetWebhookDetails() => webhookService.GetSessionId();

        [HttpGet]
        [Route("[action]")]
        public string GetBaseUrl() => webhookService.GetBaseUrl();
    }
}
