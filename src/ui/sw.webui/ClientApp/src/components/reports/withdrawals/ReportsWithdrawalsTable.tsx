// Import Devextreme components
import { Column, DataGrid, MasterDetail, Scrolling } from "devextreme-react/data-grid";
import { Chart, CommonSeriesSettings, ArgumentAxis, Series, ValueAxis, Label, ConstantLine, Legend, Tooltip } from "devextreme-react/chart";

// Function that customizes tooltip and shows values with styling
function customizeTooltip(pointInfo) {
	return {
		html: `<div><div class="tooltip-header">${pointInfo.argumentText}</div><div class="tooltip-body"><div class="series-name"><span class='top-series-name'>${pointInfo.points[0].seriesName}</span>: </div><div class="value-text"><span class='top-series-value'>${pointInfo.points[0].valueText}</span></div><div class="series-name"><span class='bottom-series-name'>${pointInfo.points[1].seriesName}</span>: </div><div class="value-text"><span class='bottom-series-value'>${pointInfo.points[1].valueText}</span>% </div></div></div>`
	};
}

// Component that shows Chart as details for Table
const MasterDetailView = ({ data }) => {
	return (
		<Chart dataSource={data.data.Records} id="chart">
			<CommonSeriesSettings argumentField="recordedDate" />
			<Series name="Overflow volume (L)" valueField="count" axis="frequency" type="bar" color="#fac29a" />
			<Series name="Overflow Frequency" valueField="percentage" axis="percentage" type="spline" color="#6b71c3" />

			<ArgumentAxis>
				<Label overlappingBehavior="stagger" />
			</ArgumentAxis>

			<ValueAxis title="Overflow volume (L)" name="frequency" position="left" tickInterval={300} />
			<ValueAxis title="Overflow Frequency" name="percentage" position="right" tickInterval={20} showZero={true} valueMarginsEnabled={false}>
				<ConstantLine value={50} width={2} color="#fc3535" dashStyle="dash">
					<Label visible={false} />
				</ConstantLine>
			</ValueAxis>

			<Tooltip enabled={true} shared={true} customizeTooltip={customizeTooltip} />

			<Legend verticalAlignment="top" horizontalAlignment="center" />
		</Chart>
	);
};

function ReportsWithdrawalsTable({ withdrawalsReport }) {
	return (
		<DataGrid className="reports-data-grid" dataSource={withdrawalsReport} width="100%" height="100%" keyExpr="Id" showBorders={true} showColumnLines={true} hoverStateEnabled={true} allowColumnResizing={true} allowColumnReordering={true}>
			<Scrolling mode="virtual" rowRenderingMode="virtual" columnRenderingMode="virtual" />
			<MasterDetail enabled={true} component={MasterDetailView} autoExpandAll={true} />
			<Column dataField="Name" caption={"Name"} allowHeaderFiltering={false} />
			<Column dataField="TotalOverflowFrequency" caption={"Overflow Frequency"} allowHeaderFiltering={false} />
			<Column dataField="TotalOverflowVolume" caption={"Overflow Volume"} allowHeaderFiltering={false} />
		</DataGrid>
	);
}

export default ReportsWithdrawalsTable;
