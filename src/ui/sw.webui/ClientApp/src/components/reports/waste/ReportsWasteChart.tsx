// Import React hooks
import { useRef } from "react";

// Import Devextreme components
import { Chart, CommonSeriesSettings, Grid, Label, Legend, Series, ValueAxis, ConstantLine } from "devextreme-react/chart";

function ReportsWasteChart({ productionWasteReport }) {
	const chartRef = useRef<Chart | any>();

	return (
		<Chart id="chart" ref={chartRef} className="reports-chart" style={{ paddingTop: 15 }} dataSource={productionWasteReport.wasteData}>
			<CommonSeriesSettings type="bar" hoverMode="allArgumentPoints" selectionMode="allArgumentPoints"></CommonSeriesSettings>
			<ValueAxis name="FillLevel" position="left" title="Waste Generation (L)">
				<ConstantLine value={productionWasteReport.fillLevelLimit} width={2} color="#fc3535" dashStyle="dash">
					<Label visible={false} />
				</ConstantLine>
				<Grid visible={true} />
			</ValueAxis>
			<Series valueField="FillLevel" argumentField="Time" name="Waste Generation" />
			<Legend visible={true} verticalAlignment="bottom" horizontalAlignment="center" />
		</Chart>
	);
}

export default ReportsWasteChart;
