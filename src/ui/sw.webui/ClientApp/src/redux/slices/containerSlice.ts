import { createSlice } from "@reduxjs/toolkit";
import { containerReducers } from "../reducers/containerReducer";

import { StateContainerT } from "../../utils/types";

const initialState: StateContainerT = {
	containersData: [],
	// totalContainers: 0,
	// totalVolume: 0,
	// timeToFull: "",
	totalTrash: 0,
	totalRecycle: 0,
	totalCompost: 0,
	selectedContainer: null,
	availableZones: [],
	selectedZones: [],
	selectedFilterBinStatus: [],
	selectedStreamFilters: []
};

export const containerSlice = createSlice({
	name: "container",
	initialState: initialState,
	reducers: containerReducers
});

export const { setSelectedContainer, setContainersData, setTotals, setAvailableZones, setSelectedZones, setSelectedFilterBinStatus, addSelectedStreamFilters, removeSelectedStreamFilters } = containerSlice.actions;

export const containerReducer = containerSlice.reducer;
