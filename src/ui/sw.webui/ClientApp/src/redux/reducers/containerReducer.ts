import { PayloadAction, current } from "@reduxjs/toolkit";

export const containerReducers = {
	setSelectedContainer: (state, { payload }) => {
		state.selectedContainer = payload;
	},
	setContainersData: (state, { payload }) => {
		state.containersData = payload.ContainersData;
	},
	setTotals: (state, { payload }) => {
		state.totalTrash = payload?.Trash;
		state.totalRecycle = payload?.Recycle;
		state.totalCompost = payload?.Compost;
	},
	setAvailableZones: (state, { payload }) => {
		state.availableZones = payload;
	},
	setSelectedZones: (state, { payload }) => {
		state.selectedZones = payload;
	},
	setSelectedFilterBinStatus: (state, { payload }) => {
		state.selectedFilterBinStatus = payload;
	},
	addSelectedStreamFilters: (state, { payload }: PayloadAction<number>) => {
		const filterArray = [...current(state.selectedStreamFilters), payload];

		state.selectedStreamFilters = filterArray;
	},
	removeSelectedStreamFilters: (state, { payload }: PayloadAction<number>) => {
		const filterArray = current(state.selectedStreamFilters).filter((item: number) => item !== payload);

		state.selectedStreamFilters = filterArray;
	}
};
