const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` : env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:35360';
console.log(`proxy.conf.js - Proxy target: '${target}'`);
// If you add or remove any API here you should think about changing jwt.interceptor.ts
const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/GrowthwareAccount",
      "/GrowthwareAPI",
      "/GrowthwareFile",
      "/GrowthwareFunction",
      "/GrowthwareGroup",
      "/GrowthwareRole",
      "/swagger"
   ],
    target: target,
    secure: false,
    http2: true,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;