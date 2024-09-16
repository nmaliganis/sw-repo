// Import React hooks
import { useEffect } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setMapData, setMapTotals } from "../../redux/slices/dashBoardSlice";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom tools
import { getContainersByZone, getContainersByZoneTotal } from "../../utils/apis/assets";

// Import custom components
import MapView from "./MapView";
import SummaryView from "./SummaryView";

function DashboardView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedZones } = useSelector((state: any) => state.dashboard);
	const { latestActivityData } = useSelector((state: any) => state.activity);

	const dispatch = useDispatch();

	// Update mapData when latestActivityData state changes
	useEffect(() => {
		(async () => {
			if (selectedZones.length) {
				const data = await getContainersByZone(selectedZones);

				const dataTotal = await getContainersByZoneTotal(selectedZones);

				dispatch(setMapTotals(dataTotal.Counts));

				dispatch(setMapData(data));
			}
		})();
	}, [dispatch, selectedZones, latestActivityData]);

	return (
		<>
			<Box direction="row" width="100%" height="100%">
				<Item ratio={1}>
					<MapView />
				</Item>
				<Item baseSize="auto">
					<SummaryView />
				</Item>
			</Box>
		</>
	);
}

export default DashboardView;
