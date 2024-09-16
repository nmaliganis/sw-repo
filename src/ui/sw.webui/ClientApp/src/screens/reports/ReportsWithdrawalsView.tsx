// Import React hooks
import { useState, useEffect } from "react";

// import Fontawesome icons
import { faChartBar } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { ReportsWithdrawalsForm, ReportsWithdrawalsTable } from "../../components/reports";

// Import custom tools
import { getReportsWithdrawals } from "../../utils/apis/report";

const initialState = {
	dateFrom: new Date().setDate(new Date().getDate() - 5),
	dateTo: new Date().getTime(),
	selectedZones: [0],
	earlyCollectionLimit: 0,
	delayedCollectionLimit: 1,
	selectedContainers: []
};

// Style for the Box component
const boxStyle = { gap: "5px" };

function ReportsWithdrawalsView() {
	// State that handles withdrawals settings
	const [withdrawalsParams, setWithdrawalsParams] = useState<any>(initialState);

	const [withdrawalsReport, setWithdrawalsReport] = useState<any>([]);

	const [isLoading, setIsLoading] = useState(false);

	// Generate report from withdrawal settings
	useEffect(() => {
		(async () => {
			if (withdrawalsParams?.selectedContainers.length) {
				setIsLoading(true);

				const data = await getReportsWithdrawals(withdrawalsParams);

				setWithdrawalsReport(data);
				setIsLoading(false);
			}
		})();
	}, [withdrawalsParams]);

	// Reset state on unmount
	useEffect(() => {
		return () => {
			setWithdrawalsParams(initialState);
			setWithdrawalsReport([]);
			setIsLoading(false);
		};
	}, []);

	return (
		<Box direction="row" width="100%" height="100%" style={boxStyle}>
			<Item ratio={1}>
				<ReportsWithdrawalsForm isLoading={isLoading} setWithdrawalsParams={setWithdrawalsParams} />
			</Item>
			<Item ratio={1}>
				{withdrawalsReport.length ? (
					<ReportsWithdrawalsTable withdrawalsReport={withdrawalsReport} />
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

export default ReportsWithdrawalsView;
