// Import React hooks
import { useState } from "react";

// Import DevExtreme components
import Button from "devextreme-react/button";

// import Fontawesome icons
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalendarTimes, faRecycle, faRoad, faRoute, faTrash, faTrashRestore, faTruckLoading, IconDefinition } from "@fortawesome/free-solid-svg-icons";

// Import custom components
import ReportsWasteView from "./ReportsWasteView";
import ReportsOccupancyView from "./ReportsOccupancyView";
import ReportsItinerariesView from "./ReportsItinerariesView";
import ReportsWithdrawalsView from "./ReportsWithdrawalsView";
import ReportsBinCollectionView from "./ReportsBinCollectionView";
import ReportsRouteIndicatorsView from "./ReportsRouteIndicatorsView";
import ReportsScheduledCollectionView from "./ReportsScheduledCollectionView";

import "../../styles/reports/Reports.scss";

// Array that handles visualization for report selection
const reports = [
	{
		Id: 1,
		Name: "Itineraries - Kilometers and pickups",
		Icon: faRoad
	},
	{
		Id: 2,
		Name: "Collection statistics per bin",
		Icon: faTrash
	},
	{
		Id: 3,
		Name: "Bin occupancy statistics",
		Icon: faTrashRestore
	},
	{
		Id: 4,
		Name: "Route indicators",
		Icon: faRoute
	},
	{
		Id: 5,
		Name: "Waste generation",
		Icon: faRecycle
	},
	{
		Id: 6,
		Name: "Non-service of scheduled collection",
		Icon: faCalendarTimes
	},
	{
		Id: 7,
		Name: "Early or late withdrawals",
		Icon: faTruckLoading
	}
];

// Component that handles report selection visualization
const reportsViewController = (currentReport) => {
	switch (currentReport) {
		case 1:
			return <ReportsItinerariesView />;
		case 2:
			return <ReportsBinCollectionView />;
		case 3:
			return <ReportsOccupancyView />;
		case 4:
			return <ReportsRouteIndicatorsView />;
		case 5:
			return <ReportsWasteView />;
		case 6:
			return <ReportsScheduledCollectionView />;
		case 7:
			return <ReportsWithdrawalsView />;

		default:
			return <></>;
	}
};

function ReportsView() {
	// State that handles report selection
	const [selectedReport, setSelectedReport] = useState<{
		Id: number;
		Name: string;
		Icon: IconDefinition;
	} | null>(null);

	return (
		<div className="reports-container">
			{selectedReport ? (
				<div className="reports-selected">
					<div className="reports-selected-navbar">
						<Button
							height={49}
							type="normal"
							stylingMode="text"
							onClick={() => {
								setSelectedReport(null);
							}}
						>
							<i className="dx-icon-arrowleft reports-selected-icon"></i>
						</Button>
						<h3 className="reports-selected-title">{selectedReport.Name}</h3>
						<i className="dx-icon-help reports-selected-icon"></i>
					</div>
					<div style={{ flex: 1 }}>{reportsViewController(selectedReport.Id)}</div>
				</div>
			) : (
				<>
					<h3>Select what report you wish to view/export:</h3>
					<div className="reports-buttons-container">
						{reports.map((report, index) => (
							<Button
								key={report.Id}
								className="reports-button"
								type="normal"
								stylingMode="outlined"
								onClick={() => {
									setSelectedReport(report);
								}}
								style={{ borderRadius: "1em" }}
							>
								<div className="reports-button-inner">
									{typeof report.Icon === "string" ? <i className={`dx-icon-${report.Icon}`} style={{ fontSize: "6em" }}></i> : <FontAwesomeIcon style={{ fontSize: "6em" }} icon={report.Icon} />}
									<h3 style={{ margin: 0 }}>{report.Name}</h3>
								</div>
							</Button>
						))}
					</div>
				</>
			)}
		</div>
	);
}

export default ReportsView;
