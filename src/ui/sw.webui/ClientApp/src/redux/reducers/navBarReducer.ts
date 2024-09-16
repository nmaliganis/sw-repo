import { PayloadAction } from "@reduxjs/toolkit";

export const navBarReducers = {
	setSelectedItemKey: (state, { payload }: PayloadAction<string>) => {
		state.selectedItemKey = payload;
	}
};
