// Redux
import { useSelector } from "react-redux";

import Box, { Item } from "devextreme-react/box";
import Drawer from "devextreme-react/drawer";

import VehiclesTableView from "./VehiclesTableView";
import VehiclesMapView from "./VehiclesMapView";
import VehicleSummaryView from "./VehicleSummaryView";

import "./Vehicles.scss";
import VehicleTimelineView from "./VehicleTimelineView";

function VehicleView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedVehicle } = useSelector((state: any) => state.vehicles);

	return (
		<Box direction="row" width="100%" height="100%">
			<Item baseSize={550}>
				<Drawer maxSize={425} width="100%" height="100%" style={{ padding: 10 }} opened={selectedVehicle?.Name ? true : false} openedStateMode="shrink" position="bottom" component={VehicleSummaryView} revealMode="expand">
					<VehiclesTableView />
				</Drawer>
			</Item>
			<Item ratio={1}>
				<Drawer maxSize={265} width="100%" height="100%" opened={selectedVehicle?.Name ? true : false} openedStateMode="shrink" position="right" component={VehicleTimelineView} revealMode="expand">
					<VehiclesMapView />
				</Drawer>
			</Item>
		</Box>
	);
}

export default VehicleView;
