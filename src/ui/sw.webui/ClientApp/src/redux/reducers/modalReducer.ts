import { PayloadAction } from "@reduxjs/toolkit";

import { ContainerModelT } from "../../utils/types";

export const modalReducers = {
	setPopupDetails: (state, { payload }: PayloadAction<{ visible: boolean; selectedMapItem: ContainerModelT | any; selectedMapItemHistory: any[] }>) => {
		state.selectedMapItemHistory = payload.selectedMapItemHistory;
		state.selectedMapItem = payload.selectedMapItem;
		state.popupDetailsVisible = payload.visible;
	},
	hidePopupDetails: (state) => {
		state.popupDetailsVisible = false;
	},
	setSelectedMapItemHistory: (state, { payload }: PayloadAction<any[]>) => {
		state.selectedMapItemHistory = payload;
	}
};
