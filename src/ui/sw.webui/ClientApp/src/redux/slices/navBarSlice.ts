import { createSlice } from "@reduxjs/toolkit";

import { navBarReducers } from "../reducers/navBarReducer";

import { StateNavBarT } from "../../utils/types";

const initialState: StateNavBarT = {
	selectedItemKey: "0"
};

export const navBarSlice = createSlice({
	name: "nav-bar",
	initialState: initialState,
	reducers: navBarReducers
});

export const { setSelectedItemKey } = navBarSlice.actions;

export const navBarReducer = navBarSlice.reducer;
