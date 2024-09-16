import notify from "devextreme/ui/notify";

import { http } from "../http";

export const getReportsItineraries = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/Itineraries", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return [];
		})
		.catch(() => {
			notify("Failed to get reports itineraries.", "error", 3000);
			return [];
		});
};

export const getReportsStatisticsBinCollection = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/StatisticsBinCollection", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return {};
		})
		.catch(() => {
			notify("Failed to get reports statistics per bin.", "error", 3000);

			return {};
		});
};

export const getReportsStatisticsOccupancy = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/StatisticsOccupancy", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return [];
		})
		.catch(() => {
			notify("Failed to get reports statistics occupancy.", "error", 3000);

			return [];
		});
};

export const getReportsRouteIndicators = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/RouteIndicators", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return {};
		})
		.catch(() => {
			notify("Failed to get reports route indicators.", "error", 3000);
			return {};
		});
};

export const getReportsWasteGeneration = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/WasteGeneration", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return [];
		})
		.catch(() => {
			notify("Failed to get reports waste generation.", "error", 3000);
			return [];
		});
};

export const getReportsScheduledCollection = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/ScheduledCollection", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return [];
		})
		.catch(() => {
			notify("Failed to get reports scheduled collection.", "error", 3000);
			return [];
		});
};

export const getReportsWithdrawals = async (params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/Withdrawals", { params: params })
		.then((response) => {
			if (response.status === 200) {
				return response.data.Value;
			}
			notify("No report was generated.", "error", 3000);
			return [];
		})
		.catch(() => {
			notify("Failed to get reports withdrawals.", "error", 3000);
			return [];
		});
};

export const getReportDocument = async (type, params) => {
	return await http
		.get(process.env.REACT_APP_REPORT_HTTP + "/v1/report-export", { params: { ...params, type: type } })
		.then((response) => {})
		.catch(() => {
			notify("Failed to generate report.", "error", 3000);
		});
};
