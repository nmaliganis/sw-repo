import { PayloadAction } from "@reduxjs/toolkit";

import { latestActivityDigitalT, latestActivityGPST, latestActivityUltraSonicT, ContainerModelT } from "../../utils/types";

import { uniqueId } from "../../utils/apis/assets";

export const activityReducers = {
	setInitialActivityData: (state, { payload }: PayloadAction<any[]>) => {
		state.latestActivityData = payload;
	},
	setLatestActivityData: (state, { payload }: PayloadAction<latestActivityUltraSonicT & latestActivityGPST & latestActivityDigitalT & ContainerModelT>) => {
		state.latestActivityData.unshift({ ...payload, Id: uniqueId() });
	},
	deleteLatestActivityEntry: (state, { payload }: PayloadAction<any>) => {
		const newLatestActivityData = state.latestActivityData.filter((item) => item.Id !== payload);

		state.latestActivityData = newLatestActivityData;
	}
};
