// Import React hooks
import React, { useState } from "react";

// import Fontawesome icons
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

// Import DevExtreme components
import TabPanel, { Item } from "devextreme-react/tab-panel";

// Import custom tools
import VehiclesListView from "./VehiclesListView";
import UsersListView from "./UsersListView";

import "../../styles/Notifications.scss";

// Function to display title and icon for each tab item
const itemTitleRender = (item: { icon: any; title: boolean | React.ReactChild | React.ReactFragment | React.ReactPortal | null | undefined }) => {
	return (
		<>
			{typeof item.icon === "string" ? <i className={`dx-icon dx-icon-${item.icon}`}></i> : <FontAwesomeIcon className="dx-icon" icon={item.icon} />}
			<span>{item.title}</span>
		</>
	);
};

function NotificationsView() {
	// State that keeps track of the currently selected tab
	const [selectedNotificationTab, setSelectedNotificationTab] = useState();

	// Function that handle changes in the selected tab
	const onSelectionChanged = (args: any) => {
		if (args.name === "selectedIndex") {
			setSelectedNotificationTab(args.value);
		}
	};

	return (
		<div style={{ height: "100%", minWidth: 900, padding: 10, display: "flex", justifyContent: "center" }}>
			<TabPanel style={{ border: "1px solid #d7d7d7", maxWidth: 800 }} height="100%" selectedIndex={selectedNotificationTab} onOptionChanged={onSelectionChanged} itemTitleRender={itemTitleRender} loop={false} animationEnabled={false} swipeEnabled={false}>
				<Item title="Vehicles" icon="car">
					<VehiclesListView />
				</Item>
				<Item title="Users" icon="group">
					<UsersListView />
				</Item>
			</TabPanel>
		</div>
	);
}

export default NotificationsView;
