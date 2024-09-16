import { createSlice } from "@reduxjs/toolkit";

import { modalReducers } from "../reducers/modalReducer";

import { StateModalT } from "../../utils/types";

const initialState: StateModalT = {
	popupDetailsVisible: false,
	selectedMapItem: {},
	selectedMapItemHistory: []
};

export const modalSlice = createSlice({
	name: "modal",
	initialState: initialState,
	reducers: modalReducers
});

export const { setPopupDetails, hidePopupDetails, setSelectedMapItemHistory } = modalSlice.actions;

export const modalReducer = modalSlice.reducer;
