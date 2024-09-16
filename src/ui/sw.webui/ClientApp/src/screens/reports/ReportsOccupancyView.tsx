// Import React hooks
import { useState, useEffect } from "react";

// import Fontawesome icons
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChartBar, faTable } from "@fortawesome/free-solid-svg-icons";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsOccupancyForm, ReportsOccupancyTable, ReportsOccupancyChart } from "../../components/reports";

// Import custom tools
import { getReportsStatisticsOccupancy } from "../../utils/apis/report";

const initialState = {
	dateFrom: new Date().setDate(new Date().getDate() - 5),
	dateTo: new Date().getTime(),
	selectedZones: [],
	selectedStreams: [],
	fillLevelLimit: 0.5
};

const boxStyle = { gap: "5px" };

function ReportsOccupancyView() {
	// State that handles occupancy data
	const [occupancyParams, setOccupancyParams] = useState<any>(initialState);
	const [occupancyReport, setOccupancyReport] = useState<any>([]);

	const [isLoading, setIsLoading] = useState(false);

	// Generate report from occupancy settings
	useEffect(() => {
		(async () => {
			if (occupancyParams?.selectedStreams.length) {
				setIsLoading(true);

				const data = await getReportsStatisticsOccupancy(occupancyParams);

				setOccupancyReport(data);
				setIsLoading(false);
			}
		})();
	}, [occupancyParams]);

	// Reset data on unmount
	useEffect(() => {
		return () => {
			setOccupancyParams(initialState);
			setOccupancyReport([]);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="col" width="100%" height="100%" style={boxStyle}>
			<Item baseSize="auto">
				<ReportsOccupancyForm isLoading={isLoading} setOccupancyReport={setOccupancyParams} />
			</Item>
			<Item ratio={1}>
				{occupancyReport?.length ? (
					<Box direction="row" width="100%" height="100%" style={boxStyle}>
						<Item ratio={1}>
							<ReportsOccupancyTable occupancyReport={occupancyReport} />
						</Item>
						<Item ratio={1}>
							<ReportsOccupancyChart occupancyReport={occupancyReport} />
						</Item>
					</Box>
				) : (
					<div className="reports-initial-container">
						<div style={{ textAlign: "center" }}>
							<FontAwesomeIcon style={{ fontSize: "12em", marginRight: "10rem" }} color="#d3d3d3" icon={faTable} />
							<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faChartBar} />
							<h4>Select Zones and Streams to generate report</h4>
						</div>
					</div>
				)}
			</Item>
		</Box>
	);
}

export default ReportsOccupancyView;
