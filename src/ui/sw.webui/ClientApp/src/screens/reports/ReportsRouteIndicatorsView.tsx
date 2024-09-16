// Import React hooks
import { useState, useEffect } from "react";

// import Fontawesome icons
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTable } from "@fortawesome/free-solid-svg-icons";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsRouteIndicatorsForm, ReportsRouteIndicatorsTable } from "../../components/reports";

// Import custom tools
import { getReportsRouteIndicators } from "../../utils/apis/report";

import "../../styles/reports/ReportsRouteIndicators.scss";

const initialState = {
	dateFrom: new Date().setDate(new Date().getDate() - 5),
	dateTo: new Date().getTime(),
	selectedStreams: []
};

// Style for the Box component
const boxStyle = { gap: "5px", margin: "auto" };

function ReportsRouteIndicatorsView() {
	// State that handles settings for indicator reports
	const [routeIndicatorsParams, setRouteIndicatorsParams] = useState(initialState);

	const [routeIndicatorsReport, setRouteIndicatorsReport] = useState<any>(null);

	const [isLoading, setIsLoading] = useState(false);

	// Generate report from indicators settings
	useEffect(() => {
		(async () => {
			if (routeIndicatorsParams?.selectedStreams.length) {
				setIsLoading(true);

				const data = await getReportsRouteIndicators(routeIndicatorsParams);

				setRouteIndicatorsReport(data);
				setIsLoading(false);
			}
		})();
	}, [routeIndicatorsParams]);

	// Reset state on unmount
	useEffect(() => {
		return () => {
			setRouteIndicatorsParams(initialState);
			setRouteIndicatorsReport(null);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="col" width="60%" height="100%" style={boxStyle}>
			<Item baseSize="auto">
				<ReportsRouteIndicatorsForm isLoading={isLoading} setRouteIndicatorsParams={setRouteIndicatorsParams} />
			</Item>
			<Item ratio={1}>
				{routeIndicatorsReport ? (
					<ReportsRouteIndicatorsTable routeIndicatorsReport={routeIndicatorsReport} />
				) : (
					<div className="reports-initial-container">
						<div style={{ textAlign: "center" }}>
							<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faTable} />
							<h4>Select Streams to generate report</h4>
						</div>
					</div>
				)}
			</Item>
		</Box>
	);
}

export default ReportsRouteIndicatorsView;
