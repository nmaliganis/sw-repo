import { configureStore } from "@reduxjs/toolkit";

// import logger from "redux-logger";

import { activityReducer } from "./slices/activitySlice";
import { containerReducer } from "./slices/containerSlice";
import { dashboardReducer } from "./slices/dashBoardSlice";
import { itineraryReducer } from "./slices/itinerarySlice";
import { loginReducer } from "./slices/loginSlice";
import { modalReducer } from "./slices/modalSlice";
import { navBarReducer } from "./slices/navBarSlice";
import { reportsReducer } from "./slices/reportsSlice";
import { vehicleReducer } from "./slices/vehicleSlice";

// const middleware = [...getDefaultMiddleware(), logger];

export const store = configureStore({
	reducer: {
		login: loginReducer,
		modal: modalReducer,
		navbar: navBarReducer,
		activity: activityReducer,
		dashboard: dashboardReducer,
		container: containerReducer,
		itinerary: itineraryReducer,
		vehicles: vehicleReducer,
		reports: reportsReducer
	},
	// middleware: middleware,
	devTools: process.env.NODE_ENV !== "production"
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;
