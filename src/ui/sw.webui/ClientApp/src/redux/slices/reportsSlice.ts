import { createSlice } from "@reduxjs/toolkit";

import { reportsReducers } from "../reducers/reportsReducer";

const initialState = {
	selectedReportItineraries: []
};

export const reportsSlice = createSlice({
	name: "reports",
	initialState: initialState,
	reducers: reportsReducers
});

export const { setSelectedReportItineraries } = reportsSlice.actions;

export const reportsReducer = reportsSlice.reducer;
