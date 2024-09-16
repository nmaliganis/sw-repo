import { PayloadAction } from "@reduxjs/toolkit";
import { SelectedVehicleT } from "../../utils/types";

export const vehicleReducers = {
	setVehiclesData: (state, { payload }: PayloadAction<any>) => {
		state.vehiclesData = payload;
	},
	setSelectedVehicle: (state, { payload }: PayloadAction<SelectedVehicleT>) => {
		state.selectedVehicle = payload;
	}
};
