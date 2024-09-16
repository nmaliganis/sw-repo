// Import Devextreme components
import TextBox from "devextreme-react/text-box";
import DataGrid, { Column, Scrolling, Summary, TotalItem } from "devextreme-react/data-grid";

function ReportsBinCollectionTable({ binCollectionReport }) {
	return (
		<>
			<div className="dx-fieldset" style={{ width: "40%", margin: "1rem 0" }}>
				<div className="dx-field">
					<div className="dx-field-label" style={{ width: "25%" }}>
						Itineraries:
					</div>
					<div className="dx-field-value" style={{ width: "35%" }}>
						<TextBox readOnly stylingMode="outlined" value={binCollectionReport?.itinerariesCount} />
					</div>
				</div>
				<div className="dx-field">
					<div className="dx-field-label" style={{ width: "25%" }}>
						Containers:
					</div>
					<div className="dx-field-value" style={{ width: "35%" }}>
						<TextBox readOnly stylingMode="outlined" value={binCollectionReport?.containersCount} />
					</div>
				</div>
			</div>
			<DataGrid dataSource={binCollectionReport?.statsData} width="100%" height="100%" keyExpr="Id" showBorders={true} showColumnLines={true} hoverStateEnabled={true} allowColumnResizing={true} allowColumnReordering={true} noDataText="No data. Select Zones, and Streams.">
				<Scrolling mode="virtual" rowRenderingMode="virtual" columnRenderingMode="virtual" />
				<Column dataField="Name" caption="Value" width={250} />
				<Column dataField="Collections" caption="Collections" width={150} />
				<Column dataField="CollectionFrequency" caption="Collection Frequency" width={180} />
				<Column dataField="MaxNumberOfCollections" caption="Max number of non-consecutive withdrawals" />
				<Summary>
					<TotalItem column="Name" summaryType="count" displayFormat="Total: {0}" />
				</Summary>
			</DataGrid>
		</>
	);
}

export default ReportsBinCollectionTable;
