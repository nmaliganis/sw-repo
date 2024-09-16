// Import React hooks
import { useState } from "react";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";
import TabPanel, { Item as TabItem } from "devextreme-react/tab-panel";

// Import custom components
import { ItinerariesTable, ItinerariesMap, ItinerariesContainersTable } from "../../components/itineraries";

function ItinerariesView() {
	// State that handles the selected visible tab from the selected itineraries
	const [selectedTab, setSelectedTab] = useState(0);

	// Function that handles the state change on tab switching
	const onSelectionChanged = (args: any) => {
		if (args.name === "selectedIndex") {
			setSelectedTab(args.value);
		}
	};

	return (
		<Box direction="row" width="100%" height="100%">
			<Item ratio={1}>
				<ItinerariesTable />
			</Item>
			<Item ratio={1}>
				<TabPanel style={{ border: "1px solid #e0e0e0" }} width="100%" height="100%" selectedIndex={selectedTab} onOptionChanged={onSelectionChanged} loop={false} animationEnabled={false} swipeEnabled={false}>
					<TabItem title="Map" icon="map">
						<ItinerariesMap />
					</TabItem>
					<TabItem title="Containers" icon="box">
						<ItinerariesContainersTable />
					</TabItem>
				</TabPanel>
			</Item>
		</Box>
	);
}

export default ItinerariesView;
