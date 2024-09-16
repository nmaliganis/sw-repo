// Import Devextreme components
import DataGrid, { Column, ColumnChooser, MasterDetail, Scrolling, Selection } from "devextreme-react/data-grid";

// const onCellPrepared = (e) => {
// 	if (e.rowType === "data" && e.rowIndex > 0) {
// 		setTimeout(() => {
// 			let previousCellValue = e.component.cellValue(e.rowIndex - 1, e.column.dataField);
// 			if (e.value === previousCellValue) {
// 				let previousCellElement = e.component.getCellElement(e.rowIndex - 1, e.column.dataField);
// 				let rowspan = 2;
// 				while (previousCellElement.style.display === "none") {
// 					rowspan++;
// 					previousCellElement = e.component.getCellElement(e.rowIndex - rowspan + 1, e.column.dataField);
// 				}
// 				previousCellElement.setAttribute("rowspan", rowspan);
// 				e.cellElement.style.display = "none";
// 			}
// 		});
// 	}
// };

// Function that selects first row and open details when data loads
const oncContentReady = (e) => {
	if (!e.component.getSelectedRowKeys().length) {
		e.component.selectRowsByIndexes(0);
	}
};

// Function that opens details when row is selected
const onSelectionChanged = (e) => {
	e.component.collapseAll(-1);
	e.component.expandRow(e.currentSelectedRowKeys[0]);
};

const reportDetails = ({ data }) => {
	return (
		<DataGrid dataSource={[data]} width="100%" showBorders={true} columnAutoWidth={true} showColumnLines={true}>
			<Column caption="From the 1st to the last withdrawal">
				<Column dataField="DistanceLastCollect" caption="Distance" width={80} alignment="center" />
				<Column dataField="DurationLastCollect" caption="Duration" width={80} alignment="center" />
				<Column dataField="MedianWeight" caption="M. Weight" width={90} alignment="center" />
				<Column dataField="TotalWeight" caption="Total Weight" width={100} alignment="center" />
			</Column>
			<Column caption="From last departure until arrival at KDAU/SMA">
				<Column dataField="DistanceLastArrival" caption="Distance" width={120} alignment="center" />
				<Column dataField="DurationLastArrival" caption="Duration" width={120} alignment="center" />
			</Column>
			<Column caption="KDAU/SMA (Stance)">
				<Column dataField="DurationKDAY" caption="Duration" width={150} alignment="center" />
			</Column>
			<Column caption="From KDAU/SMA to the train station">
				<Column dataField="DistanceTillKDAY" caption="Distance" width={100} alignment="center" />
				<Column dataField="DurationTillKDAY" caption="Duration" width={100} alignment="center" />
			</Column>
		</DataGrid>
	);
};

function ReportsItinerariesTable({ itinerariesReport }) {
	return (
		<DataGrid dataSource={itinerariesReport} width="100%" height="100%" keyExpr="Id" showBorders={true} showColumnLines={true} hoverStateEnabled={true} allowColumnResizing={true} allowColumnReordering={true} onContentReady={oncContentReady} onSelectionChanged={onSelectionChanged}>
			<Selection mode="single" />
			<ColumnChooser enabled={true} />
			<Scrolling mode="virtual" rowRenderingMode="virtual" columnRenderingMode="virtual" />
			<Column dataField="Department" width={95} />
			<Column dataField="Name" caption="Program" width={150} />
			<Column dataField="ScheduledDate" caption="Date" width={90} />
			<Column dataField="Vehicle" width={90} />
			<MasterDetail enabled={false} render={reportDetails} />
			<Column caption="Itinerary (Departure to return from the station)">
				<Column dataField="StartTimeDepart" caption="Start Time" width={100} alignment="center" />
				<Column dataField="EndTimeDepart" caption="End Time" width={100} alignment="center" />
				<Column dataField="TotalLastCollect" caption="Collection Total" width={120} alignment="center" />
				<Column dataField="DistanceDepart" caption="Distance" width={80} alignment="center" />
				<Column dataField="DurationDepart" caption="Duration" width={80} alignment="center" />
			</Column>
			<Column caption="From the train station to the 1st pick-up point">
				<Column dataField="DistanceFirstCollect" caption="Distance" width={150} alignment="center" />
				<Column dataField="DurationFirstCollect" caption="Duration" width={150} alignment="center" />
			</Column>
		</DataGrid>
	);
}

export default ReportsItinerariesTable;
