import React from "react";

import DataGrid, { Column, Scrolling } from "devextreme-react/data-grid";

function ReportsOccupancyTable({ occupancyReport }) {
	return (
		<DataGrid dataSource={occupancyReport} width="100%" height="100%" keyExpr="Id" showBorders={true} showColumnLines={true} hoverStateEnabled={true} allowColumnResizing={true} allowColumnReordering={true}>
			<Scrolling mode="virtual" rowRenderingMode="virtual" columnRenderingMode="virtual" />
			<Column dataField="Name" caption={"Name"} width={120} allowHeaderFiltering={false} selectedFilterOperation="contains" />

			<Column caption="Average Filling Frequency">
				<Column dataField="AvgFillingLongerDuration" caption="Longer duration" width={120} alignment="center" />
				<Column dataField="AvgFillShorterDuration" caption="Shorter duration" width={120} alignment="center" />
			</Column>
			<Column caption="Average collection frequency">
				<Column dataField="AvgCollLongerDuration" caption="Longer duration" width={120} alignment="center" />
				<Column dataField="AvgCollShorterDuration" caption="Shorter duration" width={120} alignment="center" />
			</Column>
			<Column dataField="AvgFillingPercent" caption="Average filling time" width={100} />
		</DataGrid>
	);
}

export default ReportsOccupancyTable;
