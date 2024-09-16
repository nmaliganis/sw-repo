const { createProxyMiddleware } = require("http-proxy-middleware");

// TODO: Remove when endpoint becomes one
// Setup to allow the React app to use multiple proxies in development

module.exports = function (app) {
	app.use(
		createProxyMiddleware("/asset", {
			target: "https://sw-waste-asset-dev.azurewebsites.net/api", // API endpoint 1
			changeOrigin: true,
			pathRewrite: {
				"^/asset": ""
			},
			headers: {
				Connection: "keep-alive"
			}
		})
	);
	app.use(
		createProxyMiddleware("/auth", {
			target: "http://sw.com:6200/api", // API endpoint 2
			changeOrigin: true,
			pathRewrite: {
				"^/auth": ""
			},
			headers: {
				Connection: "keep-alive"
			}
		})
	);
	app.use(
		createProxyMiddleware("/routing", {
			target: "https://sw-waste-routing-dev.azurewebsites.net/api", // API endpoint 3
			changeOrigin: true,
			pathRewrite: {
				"^/routing": ""
			},
			headers: {
				Connection: "keep-alive"
			}
		})
	);
	app.use(
		createProxyMiddleware("/report", {
			target: "https://sw-waste-report-dev.azurewebsites.net/api", // API endpoint 4
			changeOrigin: true,
			pathRewrite: {
				"^/report": ""
			},
			headers: {
				Connection: "keep-alive"
			}
		})
	);
	// app.use(
	// 	createProxyMiddleware("/base", {
	// 		target: "ws://20.4.232.113:6001", // API endpoint 3
	// 		changeOrigin: true,
	// 		pathRewrite: {
	// 			"^/websocket": ""
	// 		},
	// 		headers: {
	// 			Connection: "keep-alive"
	// 		}
	// 	})
	// );
	// app.use(
	// 	createProxyMiddleware("/websocket", {
	// 		target: "https://sw-messaging-dev.service.signalr.net", // API endpoint 3
	// 		changeOrigin: true,
	// 		pathRewrite: {
	// 			"^/websocket": ""
	// 		},
	// 		headers: {
	// 			Connection: "keep-alive"
	// 		}
	// 	})
	// );
};
