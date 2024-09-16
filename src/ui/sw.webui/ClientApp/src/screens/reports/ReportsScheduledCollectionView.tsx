// Import React hooks
import { useState, useEffect } from "react";

// import Fontawesome icons
import { faChartBar } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsScheduledCollectionChart, ReportsScheduledCollectionForm } from "../../components/reports";

// Import custom tools
import { getReportsScheduledCollection } from "../../utils/apis/report";

import "../../styles/reports/ReportsScheduledCollection.scss";

const initialState = {
	dateFrom: new Date().setDate(new Date().getDate() - 5),
	dateTo: new Date().getTime(),
	selectedZones: [1],
	selectedEventLimit: 1,
	selectedContainers: [],
	mustPickup: true
};

// Style for the Box component
const boxStyle = { gap: "5px" };

function ReportsScheduledCollectionView() {
	// State that handles scheduled collections settings
	const [scheduledCollectionParams, setScheduledCollectionParams] = useState<any>(initialState);

	const [scheduledCollectionReport, setScheduledCollectionReport] = useState<any>([]);

	const [isLoading, setIsLoading] = useState(false);

	// Generate report from scheduled collections settings
	useEffect(() => {
		(async () => {
			if (scheduledCollectionParams?.selectedContainers.length) {
				setIsLoading(true);

				const data = await getReportsScheduledCollection(scheduledCollectionParams);

				setScheduledCollectionReport(data);
				setIsLoading(false);
			}
		})();
	}, [scheduledCollectionParams]);

	// Reset state on unmount
	useEffect(() => {
		return () => {
			setScheduledCollectionParams(initialState);
			setScheduledCollectionReport([]);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="row" width="100%" height="100%" style={boxStyle}>
			<Item ratio={1}>
				<ReportsScheduledCollectionForm isLoading={isLoading} setScheduledCollectionParams={setScheduledCollectionParams} />
			</Item>
			<Item ratio={1}>
				{scheduledCollectionReport.length ? (
					<ReportsScheduledCollectionChart scheduledCollectionReport={scheduledCollectionReport} />
				) : (
					<div className="reports-initial-container">
						<div style={{ textAlign: "center" }}>
							<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faChartBar} />
							<h4>Select Containers to generate report</h4>
						</div>
					</div>
				)}
			</Item>
		</Box>
	);
}

export default ReportsScheduledCollectionView;
