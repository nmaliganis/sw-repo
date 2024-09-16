// Import Devextreme components
import { Chart, CommonSeriesSettings, Grid, Label, Format, Legend, Series, ValueAxis } from "devextreme-react/chart";

function ReportsOccupancyChart({ occupancyReport }) {
	return (
		<Chart id="chart" className="reports-chart" dataSource={occupancyReport} width="100%" height="100%">
			<CommonSeriesSettings type="bar" hoverMode="allArgumentPoints" selectionMode="allArgumentPoints">
				<Label visible={true}>
					<Format type="fixedPoint" precision={0} />
				</Label>
			</CommonSeriesSettings>

			<Series valueField="AvgFillingPercent" argumentField="Name" name="Bins" />
			<ValueAxis>
				<Grid visible={true} />
			</ValueAxis>
			<ValueAxis name="duration" position="right">
				<Grid visible={true} />
			</ValueAxis>
			<Legend verticalAlignment="bottom" horizontalAlignment="center" />
		</Chart>
	);
}

export default ReportsOccupancyChart;
