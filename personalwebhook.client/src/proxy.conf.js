const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7019';

const PROXY_CONFIG = [
  {
    context: [
      '/Webhook',
      '/webhookHub'
    ],
    target,
    secure: false,
    ws: true //Make SignalR work
  }
]

module.exports = PROXY_CONFIG;
