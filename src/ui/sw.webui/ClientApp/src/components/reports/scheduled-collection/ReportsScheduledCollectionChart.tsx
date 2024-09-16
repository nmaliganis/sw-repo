// Import Devextreme components
import { Chart, CommonSeriesSettings, Grid, Label, Format, Legend, Series, ValueAxis } from "devextreme-react/chart";

// Function that handles custom coloring for for each chart point
const customizePoint = (arg: any) => {
	if (arg.value > 7) {
		return { color: "#ff7070", hoverStyle: { color: "#ff7070" } };
	} else if (arg.value < 3) {
		return { color: "#73df70", hoverStyle: { color: "#73df70" } };
	}
	return { color: "#fbd75a", hoverStyle: { color: "#fbd75a" } };
};

function ReportsScheduledCollectionChart({ scheduledCollectionReport }) {
	return (
		<Chart id="chart" className="reports-chart" title="How often do you drive to an empty bin?" dataSource={scheduledCollectionReport} width="100%" height="100%" customizePoint={customizePoint}>
			<CommonSeriesSettings type="bar" hoverMode="allArgumentPoints" selectionMode="allArgumentPoints">
				<Label visible={true}>
					<Format type="fixedPoint" precision={0} />
				</Label>
			</CommonSeriesSettings>
			<Series valueField="AvgTime" argumentField="Name" />
			<ValueAxis>
				<Grid visible={true} />
			</ValueAxis>
			<ValueAxis name="duration" position="right">
				<Grid visible={true} />
			</ValueAxis>
			<Legend visible={false} verticalAlignment="bottom" horizontalAlignment="center" />
		</Chart>
	);
}

export default ReportsScheduledCollectionChart;
