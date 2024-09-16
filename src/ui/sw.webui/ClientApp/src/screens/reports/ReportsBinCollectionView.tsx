// Import React hooks
import { useState, useEffect } from "react";

// import Fontawesome icons
import { faTable } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsBinCollectionForm, ReportsBinCollectionTable } from "../../components/reports";

// Import custom tools
import { getReportsStatisticsBinCollection } from "../../utils/apis/report";

import "../../styles/reports/ReportsBinCollection.scss";

const initialState = {
	dateFrom: new Date().setDate(new Date().getDate() - 5),
	dateTo: new Date().getTime(),
	selectedZones: [],
	selectedStreams: []
};

// Style for the Box component
const boxStyle = { gap: "5px", margin: "auto" };

function ReportsBinCollectionView() {
	// State that handles bin collection data
	const [binCollectionParams, setBinCollectionParams] = useState(initialState);
	const [binCollectionReport, setBinCollectionReport] = useState<any>(null);

	const [isLoading, setIsLoading] = useState(false);

	// Load bin collection data
	useEffect(() => {
		(async () => {
			if (binCollectionParams?.selectedZones.length) {
				setIsLoading(true);

				const data = await getReportsStatisticsBinCollection(binCollectionParams);

				setBinCollectionReport(data);
				setIsLoading(false);
			}
		})();
	}, [binCollectionParams]);

	// Reset data on unmount
	useEffect(() => {
		return () => {
			setBinCollectionParams(initialState);
			setBinCollectionReport(null);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="col" width="70%" height="100%" style={boxStyle}>
			<Item baseSize="auto">
				<ReportsBinCollectionForm isLoading={isLoading} setBinCollectionParams={setBinCollectionParams} />
			</Item>
			<Item ratio={1}>
				{binCollectionReport?.statsData?.length ? (
					<ReportsBinCollectionTable binCollectionReport={binCollectionReport} />
				) : (
					<div className="reports-initial-container">
						<div style={{ textAlign: "center" }}>
							<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faTable} />
							<h4>Select Zones and Streams to generate report</h4>
						</div>
					</div>
				)}
			</Item>
		</Box>
	);
}

export default ReportsBinCollectionView;
