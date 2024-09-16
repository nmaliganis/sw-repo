// Import React hooks
import React, { useEffect } from "react";

// Import Redux action creators
import { useDispatch } from "react-redux";
import { setLatestActivityData } from "../../redux/slices/activitySlice";

// Import Devextreme components
// import notify from "devextreme/ui/notify";

// Import custom tools
import useIsMounted from "../../utils/useIsMounted";
import { getEventHistoryByDevice } from "../../utils/apis/assets";

import "../../styles/WebSocket.scss";

// Item that holds current url for websocket connection
const wsClientUrl = process.env.REACT_APP_WEBSOCKET_HTTP as string;

// import { ServiceBusClient } from "@azure/service-bus";

// const connectionString = "Endpoint=sb://sw.servicebus.windows.net/;SharedAccessKeyName=sensoring-sas;SharedAccessKey=KcnZL3dX3klNjnTMLqcmpWbZAOpaY9uO93K+tcsXSQM=;EntityPath=sensoring";
// const queueName = "sensoring";

// const serviceBusClient = new ServiceBusClient(connectionString);

// const receiver = serviceBusClient.createReceiver(queueName);
// const sender = serviceBusClient.createSender(queueName);

function WebSocketClient() {
	// const [reloadConnection, setReloadConnection] = useState(false);
	// const [failedToConnectIndicator, setFailedToConnectIndicator] = useState(false);
	// const [connectionState, setConnectionState] = useState("CONNECTING");

	const dispatch = useDispatch();

	// Ref that checks if component is mounted
	const isComponentMounted = useIsMounted();

	// const onHiding = () => {
	// 	setFailedToConnectIndicator(false);
	// };

	// const onReloadConnectionClick = () => {
	// 	setReloadConnection((state) => !state);
	// };

	// initialize websocket and add event listener to update redux store accordingly
	useEffect(() => {
		let ws = new WebSocket(wsClientUrl);

		if (ws) {
			ws.onopen = () => {
				console.log(`%c ${wsClientUrl} returned OPEN.`, `color: white; font-weight: bold; background-color: green;`);
				if (isComponentMounted.current) {
					// isComponentMounted.current && setConnectionState("LIVE");
				}
			};
			ws.onmessage = async (message: any) => {
				// Check if component is mounted
				if (isComponentMounted.current) {
					if (message)
						if (message.data !== "Ack") {
							const data = JSON.parse(message.data);

							// TODO: The below logic needs to be refactored in accordance to the final version of the monitoring setting
							if (Array.isArray(data)) {
							} else if ("PinNumber" in data) {
								// if (data.pinNumber !== 0) {
								// 	dispatch(setLatestActivityData(data));
								// }
							} else {
								if ("Range" in data) {
									const deviceData = await getEventHistoryByDevice(data.Imei);

									if (deviceData) dispatch(setLatestActivityData(deviceData));
								} else {
									dispatch(setLatestActivityData(data));
								}
							}
						}
				}
			};
			ws.onerror = () => {
				console.log(`%c ${wsClientUrl} returned ERROR.`, `color: white; font-weight: bold; background-color: red;`);
				// Check if component is mounted
				if (isComponentMounted.current) {
					// notify("Failed to connect. Please reload page.", "error", 5000);
				}
			};
			ws.onclose = () => {
				console.log(`%c ${wsClientUrl} return CLOSED.`, `color: white; font-weight: bold; background-color: red;`);
				if (isComponentMounted.current) {
					// notify("Failed to connect. Please reload page.", "error", 5000);
				}
			};
		}

		return () => {
			ws.close();
		};
	}, [dispatch, isComponentMounted]);

	return null;

	// return (
	// 	<div className="indicator-container vertical-center-content" style={{ paddingLeft: 9 }}>
	// 		{connectionState === "LIVE" && <div className="indicator-live sidebar-icon" title="LIVE" />}
	// 		{connectionState === "CONNECTING" && <div className="indicator-connecting sidebar-icon" title="Connecting..." />}
	// 		{connectionState === "OFFLINE" && <div className="indicator-offline sidebar-icon" onClick={onReloadConnectionClick} title="Offline" />}
	// 		<div className="hidden-sidebar" style={{ marginLeft: "1rem" }}>
	// 			LIVE
	// 		</div>
	// 		<Toast visible={failedToConnectIndicator} message="Failed to connect. Please reload page." type="error" onHiding={onHiding} displayTime={2000} />
	// 	</div>
	// );
}

export default React.memo(WebSocketClient);
