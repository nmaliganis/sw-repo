import { createSlice } from "@reduxjs/toolkit";

import { itineraryReducers } from "../reducers/itineraryReducer";

const initialState = {
	itineraryData: [],
	selectedItinerary: null,
	selectedTemplate: null,
	restrictionsData: [],
	focusedRestriction: null,
	startEndPoints: [],
	zonesByCompany: [],
	vehicles: [],
	drivers: []
};

export const itinerarySlice = createSlice({
	name: "itinerary",
	initialState: initialState,
	reducers: itineraryReducers
});

export const { setItineraryData, setSelectedItinerary, setSelectedTemplate, setRestrictionsData, setFocusedRestriction, setStartEndPoints, setZonesByCompany, setVehicles, setDrivers } = itinerarySlice.actions;

export const itineraryReducer = itinerarySlice.reducer;
