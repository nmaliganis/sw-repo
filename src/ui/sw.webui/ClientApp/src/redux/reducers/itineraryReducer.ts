export const itineraryReducers = {
	setItineraryData: (state, { payload }) => {
		state.itineraryData = payload;
	},
	setSelectedItinerary: (state, { payload }) => {
		state.selectedItinerary = payload;
	},
	setSelectedTemplate: (state, { payload }) => {
		state.selectedTemplate = payload;
	},
	setRestrictionsData: (state, { payload }) => {
		state.restrictionsData = payload;
	},
	setFocusedRestriction: (state, { payload }) => {
		state.focusedRestriction = payload;
	},
	setStartEndPoints: (state, { payload }) => {
		state.startEndPoints = payload;
	},
	setZonesByCompany: (state, { payload }) => {
		state.zonesByCompany = payload;
	},
	setVehicles: (state, { payload }) => {
		state.vehicles = payload;
	},
	setDrivers: (state, { payload }) => {
		state.drivers = payload;
	}
};
