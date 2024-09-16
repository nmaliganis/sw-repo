// Import Devextreme components
import { Chart, CommonSeriesSettings, Grid, Label, Format, Legend, Series, ValueAxis } from "devextreme-react/chart";

function ReportsItinerariesChart({ itinerariesReport }) {
	return (
		<Chart id="chart" className="reports-chart" dataSource={itinerariesReport} width="100%" height="100%">
			<CommonSeriesSettings type="bar" hoverMode="allArgumentPoints" selectionMode="allArgumentPoints">
				<Label visible={true}>
					<Format type="fixedPoint" precision={0} />
				</Label>
			</CommonSeriesSettings>

			<Series valueField="DistanceDepart" argumentField="Name" name="Distance" />
			<Series valueField="TotalLastCollect" argumentField="Name" name="Collection Total" />
			<Series valueField="DurationDepart" argumentField="Name" name="Duration" axis="duration" />
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

export default ReportsItinerariesChart;
