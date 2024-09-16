import { createSlice } from "@reduxjs/toolkit";

import { vehicleReducers } from "../reducers/vehicleReducer";

import { InitialStateVehicleT } from "../../utils/types";

const initialState: InitialStateVehicleT = {
	vehiclesData: [],
	selectedVehicle: {}
};

export const vehicleSlice = createSlice({
	name: "vehicles",
	initialState: initialState,
	reducers: vehicleReducers
});

export const { setVehiclesData, setSelectedVehicle } = vehicleSlice.actions;

export const vehicleReducer = vehicleSlice.reducer;
