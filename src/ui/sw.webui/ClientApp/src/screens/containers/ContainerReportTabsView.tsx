// Import React hooks
import React, { useState, Suspense } from "react";

// Import Redux action creators
import { useSelector } from "react-redux";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";
import Tabs from "devextreme-react/tabs";
import MultiView, { Item as MultiItem } from "devextreme-react/multi-view";

// Import custom components
import { ContainerLog, ContainerTimeline } from "../../components/containers";
import LoadingPage from "../../utils/LoadingPage";

import "../../styles/containers/ContainerReportTabs.scss";

// Dynamically import the ContainerMapView component
const ContainerMapView = React.lazy(() => import("../../components/containers/tabs/ContainerMap"));

// Array of objects for the tabs menu options
const tabs = [
	{
		id: 0,
		text: "map",
		icon: "map"
	},
	{
		id: 1,
		text: "timeline",
		icon: "verticalalignbottom"
	},
	{
		id: 2,
		text: "service log",
		icon: "chart"
	}
];

// Function to customize each tab button
const tabsRenderer = (data: { icon: string | undefined; text: string | undefined }) => {
	return (
		<span>
			<i className={`dx-icon-${data.icon}`}></i>
			<span style={{ paddingLeft: 5, fontSize: 14 }}>{data.text}</span>
		</span>
	);
};

function ContainerReportTabsView({ selectedContainer }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedMapItemHistory } = useSelector((state: any) => state.modal);

	// State that handles current selected tab
	const [selectedIndex, setSelectedIndex] = useState(0);

	// Function that handles the changes of the index on tab change
	const onTabsSelectionChanged = (args: any) => {
		if (args.name === "selectedIndex") {
			setSelectedIndex(args.value);
		}
	};

	return (
		<Box direction="col" width="100%" height="100%">
			<Item baseSize="auto">
				<div style={{ display: "flex", background: "#ededf0" }}>
					<Tabs dataSource={tabs} width="80%" style={{ marginRight: "auto" }} selectedIndex={selectedIndex} onOptionChanged={onTabsSelectionChanged} itemRender={tabsRenderer} />
				</div>
			</Item>
			<Item ratio={1}>
				<MultiView selectedIndex={selectedIndex} style={{ flexGrow: 1 }} width="100%" height="100%" loop={false} swipeEnabled={false} animationEnabled={false} deferRendering={false}>
					<MultiItem visible={true}>
						<Suspense fallback={<LoadingPage />}>
							<ContainerMapView selectedContainer={selectedContainer} />
						</Suspense>
					</MultiItem>
					<MultiItem visible={true}>
						<Suspense fallback={<LoadingPage />}>
							<ContainerTimeline selectedContainer={selectedContainer} selectedMapItemHistory={selectedMapItemHistory} />
						</Suspense>
					</MultiItem>
					<MultiItem visible={true}>
						<div style={{ width: "100%", height: "100%", padding: 10 }}>
							<ContainerLog selectedMapItemHistory={selectedMapItemHistory} />
						</div>
					</MultiItem>
				</MultiView>
			</Item>
		</Box>
	);
}

export default React.memo(ContainerReportTabsView);
