// Import React hooks
import React, { useEffect, useCallback, useRef } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { addSelectedStreamFilters, addToDataFilter, removeSelectedStreamFilters, removeToDataFilter, setSelectedMapItem } from "../../redux/slices/dashBoardSlice";
import { deleteLatestActivityEntry, setInitialActivityData } from "../../redux/slices/activitySlice";

// Import DevExtreme components
import List from "devextreme-react/list";
import Box, { Item } from "devextreme-react/box";

// Import custom tools
import ContainersTotalIndicators from "../../components/containers/ContainersTotalIndicators";
import { latestActivityItem, SummaryPieChart } from "../../utils/dashboardUtils";
import { getEventHistoryRecords } from "../../utils/apis/assets";

import "../../styles/dashboard/Summary.scss";

function SummaryView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { mapData, mapTotalTrash, mapTotalRecycle, mapTotalCompost, selectedStreamFilters } = useSelector((state: any) => state.dashboard);

	const { latestActivityData } = useSelector((state: any) => state.activity);

	const scrollPositionRef = useRef(0);

	const dispatch = useDispatch();

	// Function that handles bin status selection filtering
	const onLegendClick = ({ points }: { points: any[] }) => {
		// TODO: set up logic to add remove the icons in the same category on legend click

		if (points[0].isVisible()) {
			dispatch(addToDataFilter(points[0].data.BinStatus));

			points[0].hide();
		} else {
			dispatch(removeToDataFilter(points[0].data.BinStatus));
			points[0].show();
		}
	};

	// Function that handles selection on list click
	const onSummaryItemClick = (e) => {
		if ("AssetCategoryId" in e.itemData) {
			dispatch(setSelectedMapItem(e.itemData));
		}
	};

	// Function that removes item on delete
	const onLatestActivityItemDeleted = (e) => {
		dispatch(deleteLatestActivityEntry(e.itemData.Id));
	};

	// Function that saves scroll position while data loads
	const onListScroll = useCallback((e: { component: { scrollTop: () => any } }) => {
		if (e.component.scrollTop() > 0) scrollPositionRef.current = e.component.scrollTop();
	}, []);

	// Function that keeps scroll position to were it was currently
	const onListContentReady = useCallback((e: { component: { scrollTo: (arg0: number) => void } }) => {
		e.component.scrollTo(scrollPositionRef.current);
	}, []);

	// On init get event history data
	useEffect(() => {
		(async () => {
			const data = await getEventHistoryRecords();

			dispatch(setInitialActivityData(data));
		})();
	}, [dispatch]);

	// Filter data in accordance of the selected Stream
	const data = mapData?.filter((item: { WasteType: number }) => !selectedStreamFilters.includes(item.WasteType));

	return (
		<Box direction="col" width={380} height="100%">
			<Item baseSize="auto">
				<h3 style={{ paddingLeft: 10 }}>Waste Types</h3>
				<div style={{ margin: "0.5rem" }}>
					<ContainersTotalIndicators totalTrash={mapTotalTrash} totalRecycle={mapTotalRecycle} totalCompost={mapTotalCompost} addStreamFilters={addSelectedStreamFilters} removeStreamFilters={removeSelectedStreamFilters} />
				</div>
				<hr />
			</Item>
			<Item baseSize="auto">
				<h3 style={{ paddingLeft: 10 }}>Containers</h3>
				<SummaryPieChart data={data} onLegendClick={onLegendClick} />
				<hr />
			</Item>
			<Item ratio={1} visible={false}>
				<h3 style={{ paddingLeft: 10 }}>Latest Activity</h3>
				<div className="activity-list-container">
					<List
						height="100%"
						style={{ flexGrow: 1 }}
						itemDeleteMode="static"
						onScroll={onListScroll}
						allowItemDeleting={true}
						repaintChangesOnly={true}
						itemRender={latestActivityItem}
						dataSource={latestActivityData}
						onItemClick={onSummaryItemClick}
						onContentReady={onListContentReady}
						onItemDeleting={onLatestActivityItemDeleted}
					/>
				</div>
			</Item>
		</Box>
	);
}

export default React.memo(SummaryView);
