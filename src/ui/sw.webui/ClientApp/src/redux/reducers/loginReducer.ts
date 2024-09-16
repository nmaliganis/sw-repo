import { PayloadAction } from "@reduxjs/toolkit";

import { loginDataObjectT } from "../../utils/types";

export const loginReducers = {
	setIsUserLoggedIn: (state, { payload }: PayloadAction<boolean>) => {
		state.isUserLoggedIn = payload;
	},
	UserVariableSetter: (state, { payload }: PayloadAction<loginDataObjectT>) => {
		state.userData = payload;
	},
	setCompanyUpdatedData: (state, { payload }: PayloadAction<any[]>) => {
		state.userData.UserParams.Companies = payload;
	},
	setRolesUpdatedData: (state, { payload }: PayloadAction<any[]>) => {
		state.userData.UserParams.Roles = payload;
	}
};
