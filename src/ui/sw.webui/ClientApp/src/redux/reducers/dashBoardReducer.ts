import { current, PayloadAction } from "@reduxjs/toolkit";

import { ContainerModelT } from "../../utils/types";

export const dashboardReducers = {
	setMapData: (state, { payload }: PayloadAction<any[]>) => {
		// Filter data in accordance of the selected Stream and Bin Status
		const mapData = payload?.filter((item: { WasteType: number }) => !state.selectedStreamFilters.includes(item.WasteType)).filter((item: { BinStatus: number }) => !state.mapDataFilter.includes(item.BinStatus));

		state.mapData = mapData;
	},
	setMapTotals: (state, { payload }) => {
		state.mapTotalTrash = payload?.Trash;
		state.mapTotalRecycle = payload?.Recycle;
		state.mapTotalCompost = payload?.Compost;
	},
	setSelectedMapItem: (state, { payload }: PayloadAction<ContainerModelT | null>) => {
		state.selectedMapItem = payload;
	},
	setSelectedZones: (state, { payload }) => {
		state.selectedZones = payload;
	},
	setSelectedStreamFilters: (state, { payload }) => {
		state.selectedStreamFilters = payload;
	},
	addSelectedStreamFilters: (state, { payload }: PayloadAction<number>) => {
		const filterArray = [...current(state.selectedStreamFilters), payload];

		state.selectedStreamFilters = filterArray;
	},
	removeSelectedStreamFilters: (state, { payload }: PayloadAction<number>) => {
		const filterArray = current(state.selectedStreamFilters).filter((item: number) => item !== payload);

		state.selectedStreamFilters = filterArray;
	},
	addToDataFilter: (state, { payload }: PayloadAction<string>) => {
		const filterArray = [...current(state.mapDataFilter), payload];

		state.mapDataFilter = filterArray;
	},
	removeToDataFilter: (state, { payload }: PayloadAction<string>) => {
		const filterArray = current(state.mapDataFilter).filter((item: string) => item !== payload);

		state.mapDataFilter = filterArray;
	}
};
