// Import React hooks
import { useEffect, useState } from "react";

// import Fontawesome icons
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChartBar, faTable } from "@fortawesome/free-solid-svg-icons";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsItinerariesForm, ReportsItinerariesChart, ReportsItinerariesTable } from "../../components/reports";

// Import custom tools
import { getReportsItineraries } from "../../utils/apis/report";

const boxStyle = { gap: "5px" };

function ReportsItinerariesView() {
	// State that handles itineraries data
	const [selectedItineraries, setSelectedItineraries] = useState([]);
	const [itinerariesReport, setItinerariesReport] = useState<any>([]);

	const [isLoading, setIsLoading] = useState(false);

	// Generate report in accordance to selected data
	useEffect(() => {
		(async () => {
			if (selectedItineraries.length) {
				setIsLoading(true);

				const data = await getReportsItineraries({ selectedItineraries: selectedItineraries });

				setItinerariesReport(data);
				setIsLoading(false);
			}
		})();
	}, [selectedItineraries]);

	// Reset data on unmount
	useEffect(() => {
		return () => {
			setSelectedItineraries([]);
			setItinerariesReport([]);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="row" width="100%" height="100%" style={boxStyle}>
			<Item ratio={1}>
				<ReportsItinerariesForm isLoading={isLoading} setSelectedItineraries={setSelectedItineraries} />
			</Item>
			<Item ratio={2}>
				{itinerariesReport.length ? (
					<Box direction="col" height="100%" style={boxStyle}>
						<Item ratio={1}>
							<ReportsItinerariesTable itinerariesReport={itinerariesReport} />
						</Item>
						<Item ratio={1}>
							<ReportsItinerariesChart itinerariesReport={itinerariesReport} />
						</Item>
					</Box>
				) : (
					<div className="reports-initial-container">
						<div style={{ textAlign: "center" }}>
							<h4>Select itineraries to generate reports</h4>
							<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faTable} />
						</div>

						<FontAwesomeIcon style={{ fontSize: "12em" }} color="#d3d3d3" icon={faChartBar} />
					</div>
				)}
			</Item>
		</Box>
	);
}

export default ReportsItinerariesView;
