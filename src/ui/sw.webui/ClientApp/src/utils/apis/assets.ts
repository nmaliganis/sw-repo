// Import Devextreme components
import notify from "devextreme/ui/notify";

// Import custom tools
import { http } from "../http";

// Function to generate unique ID
export const uniqueId = (length = 16) => {
	return parseInt(
		Math.ceil(Math.random() * Date.now())
			.toPrecision(length)
			.toString()
			.replace(".", "")
	);
};

export const getAssetCategories = async () => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/AssetCategories").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getEventHistoryData = async ({ selectedItem, startDate = new Date(new Date().setDate(new Date().getDate() - 1)), endDate = new Date() }) => {
	const start = startDate.toLocaleDateString("en-CA");
	const end = endDate.toLocaleDateString("en-CA");

	return await http
		.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/EventHistory/container/${selectedItem}/start_date/${start}/end_date/${end}`)
		.then((response) => {
			if (response.data.Value) {
				// Export data from JSON, join and add unique id on each one
				const eventData = response.data.Value.map((item) => ({ ...item, ...JSON.parse(item.EventValueJson) }));

				return eventData.sort((a, b) => new Date(a.Recorded).getTime() - new Date(b.Recorded).getTime());
			}

			return [];
		})
		.catch(() => {
			notify("Failed to get event history. Please try again.", "error", 2000);
			return [];
		});
};

export const getEventHistoryRecords = async () => {
	return await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/EventHistory/records/5`).then(async (response) => {
		if (response.status === 200) {
			const data = await Promise.all(
				response.data.Value.map(async (item) => {
					let valueJson = JSON.parse(item.EventValueJson);

					if (Array.isArray(valueJson)) valueJson = valueJson[0];

					if ("Range" in valueJson) {
						const deviceData = await getEventHistoryByDevice(valueJson.Imei);
						if (deviceData) return deviceData;
						return valueJson;
					}
					return valueJson;
				})
			);
			return data;
		}
		return [];
	});
};

export const getEventHistoryByDevice = async (Imei) => {
	return await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Containers/device/${Imei}`).then((response) => {
		if (response.status === 200) {
			return response.data.Model;
		}
		return null;
	});
};

export const getContainers = async (
	options = {
		Filter: null,
		SearchQuery: null
	}
) => {
	return await http
		.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Containers", {
			params: {
				...options
			}
		})
		.then((response) => {
			if (response.status === 200) {
				// Change shape of each object in order to handle changes inside the Form dynamically
				// data.Value = data.Value.map((item: { Level: number }) => ({
				// 	...item,
				// 	stateLevel: Math.ceil(item.Level / 33)
				// }));
				return response.data.Value;
			}
			return [];
		});
};

export const getSelectedContainer = async (selectedKey) => {
	return await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Containers/${selectedKey}`).then((response) => {
		if (response.status === 200) {
			return response.data.Model;
		}
		return [];
	});
};

export const getPolygon = async () => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Polygon").then((response) => {
		// TODO: Change to accept only array
		if (response.status === 200) {
			return response.data;
		}
		return [];
	});
};

export const getZonesByCompany = async (
	options = {
		Filter: null,
		SearchQuery: null
	}
) => {
	return await http
		.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Companies/zones", {
			params: {
				...options
			}
		})
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			return [];
		});
};

export const getContainersByZone = async (
	zones,
	options = {
		Filter: null,
		SearchQuery: null
	}
) => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Containers/zones/?" + zones.map((item) => `zones=${item.Id}`).join("&")).then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getContainersByZoneTotal = async (zones) => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Containers/count-total?" + zones.map((item) => `zones=${item.Id}`).join("&")).then((response) => {
		if (response.status === 200) {
			return response.data.Model;
		}
		return [];
	});
};

export const searchContainersByCriteria = async (criteria, zones) => {
	return await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Containers/search-by-criteria/criteria/${criteria}?${zones.map((item) => `zones=${item.Id}`).join("&")}`).then((response) => {
		if (response.status === 200) {
			return response.data.Model;
		}
		return [];
	});
};

export const searchContainersByVolume = async (range, zones) => {
	return await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Containers/search-by-volume/lower-level/${range[0]}/upper-level/${range[1]}?${zones.map((item) => `zones=${item.Id}`).join("&")}`).then((response) => {
		if (response.status === 200) {
			return response.data.Model;
		}
		return [];
	});
};

export const getVehicles = async () => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Vehicles").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getSensorTypes = async () => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/SensorTypes").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getDeviceModels = async () => {
	return await http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/DeviceModels").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};
