import notify from "devextreme/ui/notify";

import { http } from "../http";

export const getTemplates = async (
	options = {
		Filter: null,
		SearchQuery: null
	}
) => {
	return await http
		.get(process.env.REACT_APP_ROUTING_HTTP + "/v1/ItineraryTemplates", {
			params: {
				...options
			}
		})
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			return [];
		})
		.catch(() => {
			notify("Failed to get itinerary templates.", "error", 3000);
		});
};

export const createTemplate = (newTemplate) => {
	return http
		.post(process.env.REACT_APP_ROUTING_HTTP + "/v1/ItineraryTemplates", newTemplate)
		.then((response) => {
			if (response.status !== 201) notify("Failed to create template.", "error", 3000);
			return response.data;
		})
		.catch(() => {
			notify("Failed to create template.", "error", 3000);
		});
};

export const getItineraries = async (
	options = {
		Filter: null,
		SearchQuery: null
	}
) => {
	return await http
		.get(process.env.REACT_APP_ROUTING_HTTP + "/v1/Itineraries", {
			params: {
				...options
			}
		})
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			return [];
		})
		.catch(() => {
			notify("Failed to get itineraries.", "error", 3000);
		});
};

export const createItinerary = async (newItinerary) => {
	return http
		.post(process.env.REACT_APP_ROUTING_HTTP + "/v1/Itineraries", newItinerary)
		.then((response) => {
			if (response.status !== 201) notify("Failed to create Itinerary.", "error", 3000);
			return response.data;
		})
		.catch(() => {
			notify("Failed to create Itinerary.", "error", 3000);
		});
};

export const getStartEndLocations = async (
	options = {
		Filter: null,
		SearchQuery: null
	}
) => {
	return await http
		.get(process.env.REACT_APP_ROUTING_HTTP + "/v1/Locations", {
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
