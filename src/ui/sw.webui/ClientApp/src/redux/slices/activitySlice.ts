import { createSlice } from "@reduxjs/toolkit";

import { StateActivityT } from "../../utils/types";

import { activityReducers } from "../reducers/activityReducer";

const initialState: StateActivityT = {
	latestActivityData: []
};

const activitySlice = createSlice({
	name: "activity",
	initialState: initialState,
	reducers: activityReducers
});

export const { setInitialActivityData, setLatestActivityData, deleteLatestActivityEntry } = activitySlice.actions;

export const activityReducer = activitySlice.reducer;
