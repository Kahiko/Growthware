/* eslint-disable linebreak-style */
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://127.0.0.1:${env.ASPNETCORE_HTTPS_PORT}` : env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://127.0.0.1:5001';
console.log(`proxy.conf.js - Proxy target: '${target}'`);
// If you add or remove any API here you should think about changing jwt.interceptor.ts (JwtInterceptor)
const PROXY_CONFIG = [
	{
		context: [
			'/GrowthwareAccount',
			'/GrowthwareAPI',
			'/GrowthwareCalendar',
			'/GrowthwareFeedback',
			'/GrowthwareFile',
			'/GrowthwareFunction',
			'/GrowthwareGroup',
			'/GrowthwareMessage',
			'/GrowthwareNameValuePair',
			'/GrowthwareRole',
			'/GrowthwareSecurityEntity',
			'/GrowthwareState',
			'/swagger'
		],
		target: target,
		secure: false,
		http2: true,
		headers: {
			Connection: 'Keep-Alive'
		}
	}
];

module.exports = PROXY_CONFIG;
