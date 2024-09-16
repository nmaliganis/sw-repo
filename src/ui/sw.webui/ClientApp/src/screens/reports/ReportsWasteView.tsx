// Import React hooks
import { useState, useEffect } from "react";

// import Fontawesome icons
import { faChartBar } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsWasteChart, ReportsWasteForm } from "../../components/reports";

// Import custom tools
import { getReportsWasteGeneration } from "../../utils/apis/report";

const initialState = {
	dateFrom: new Date().setDate(new Date().getDate() - 5),
	dateTo: new Date().getTime(),
	selectedZones: [],
	fillLevelLimit: 0,
	selectedTimeResult: 0,
	selectedContainers: []
};

// Style for the Box component
const boxStyle = { gap: "5px" };

function ReportsWasteView() {
	// State that handles waste settings
	const [wasteParams, setWasteParams] = useState(initialState);

	const [productionWasteReport, setProductionWasteReport] = useState<any>({ wasteData: [], fillLevelLimit: null });

	const [isLoading, setIsLoading] = useState(false);

	// Generate report from waste settings
	useEffect(() => {
		(async () => {
			if (wasteParams?.selectedContainers.length) {
				setIsLoading(true);

				const data = await getReportsWasteGeneration(wasteParams);

				setProductionWasteReport({
					wasteData: data,
					fillLevelLimit: wasteParams.fillLevelLimit
				});
				setIsLoading(false);
			}
		})();
	}, [wasteParams]);

	// Reset state on unmount
	useEffect(() => {
		return () => {
			setWasteParams(initialState);
			setProductionWasteReport([]);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="row" width="100%" height="100%" style={boxStyle}>
			<Item ratio={1}>
				<ReportsWasteForm isLoading={isLoading} setWasteParams={setWasteParams} />
			</Item>
			<Item ratio={1}>
				{productionWasteReport.wasteData.length ? (
					<ReportsWasteChart productionWasteReport={productionWasteReport} />
				) : (
					<div className="reports-initial-container">
						<div style={{ textAlign: "center" }}>
							<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faChartBar} />
							<h4>Select containers to generate report</h4>
						</div>
					</div>
				)}
			</Item>
		</Box>
	);
}

export default ReportsWasteView;
