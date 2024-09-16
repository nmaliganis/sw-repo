import { createSlice } from "@reduxjs/toolkit";

import { loginReducers } from "../reducers/loginReducer";

import { StateLoginT } from "../../utils/types";

const initialState: StateLoginT = {
	isUserLoggedIn: false,
	userData: {
		UserName: "",
		Token: "",
		RefreshToken: "",
		UserParams: {
			AssetCategories: [],
			Companies: [],
			Roles: [],
			RefreshInterval: 60000
		}
	}
};

export const loginSlice = createSlice({
	name: "login",
	initialState: initialState,
	reducers: loginReducers
});

export const { setIsUserLoggedIn, UserVariableSetter, setCompanyUpdatedData, setRolesUpdatedData } = loginSlice.actions;

export const loginReducer = loginSlice.reducer;
