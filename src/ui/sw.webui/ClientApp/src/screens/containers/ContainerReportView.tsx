// Import React hooks
import React from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import ContainerReportTabsView from "./ContainerReportTabsView";
import { ContainerReportInfo, ContainerMap } from "../../components/containers";

function ContainerReportView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedContainer } = useSelector((state: any) => state.container);

	if (selectedContainer?.Id)
		return (
			<Box direction="col" width="100%" height="100%">
				<Item baseSize={180}>
					<ContainerReportInfo selectedContainer={selectedContainer} />
				</Item>
				<Item ratio={1}>
					<ContainerReportTabsView selectedContainer={selectedContainer} />
				</Item>
			</Box>
		);

	return <ContainerMap />;
}

export default React.memo(ContainerReportView);
