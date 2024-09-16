import { createSlice } from "@reduxjs/toolkit";

import { dashboardReducers } from "../reducers/dashBoardReducer";

import { StateDashboardT } from "../../utils/types";

const initialState: StateDashboardT = {
	mapData: [],
	mapTotalTrash: 0,
	mapTotalRecycle: 0,
	mapTotalCompost: 0,
	mapDataFilter: [],
	selectedStreamFilters: [],
	selectedZones: [],
	selectedMapItem: null
};

export const dashboardSlice = createSlice({
	name: "dashboard",
	initialState: initialState,
	reducers: dashboardReducers
});

export const { setMapData, setMapTotals, setSelectedMapItem, setSelectedZones, addSelectedStreamFilters, removeSelectedStreamFilters, addToDataFilter, removeToDataFilter } = dashboardSlice.actions;

export const dashboardReducer = dashboardSlice.reducer;
