// Import Redux selector
import { useSelector } from "react-redux";

// Import Devextreme components
import DataGrid, { Column, ColumnChooser, SearchPanel, Sorting, FilterRow, Scrolling, Lookup, HeaderFilter } from "devextreme-react/data-grid";
import { dateRenderer, fillLevelRenderer, lastUpdatedRenderer, binStatusRenderer } from "../../utils/containerUtils";
import { BucketStatus, CapacityType, MaterialType, streamType } from "../../utils/consts";

const nameOps = ["contains"];

function ItinerariesContainersTable() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedItinerary } = useSelector((state: any) => state.itinerary);
	const { userData } = useSelector((state: any) => state.login);

	return (
		<DataGrid
			className="table-background"
			style={{ marginTop: "0px !important" }}
			width="100%"
			height="100%"
			dataSource={selectedItinerary?.AssignedContainers || []}
			showBorders={true}
			showColumnLines={true}
			allowColumnResizing={true}
			rowAlternationEnabled={true}
			allowColumnReordering={true}
			hoverStateEnabled={true}
			autoNavigateToFocusedRow={false}
		>
			<Sorting mode="single" />
			<FilterRow visible={true} applyFilter="auto" />
			<HeaderFilter allowSearch={true} visible={true} />
			<Scrolling mode="virtual" rowRenderingMode="virtual" />
			<ColumnChooser enabled={true} allowSearch={true} mode="select" />
			<SearchPanel visible={true} width={240} placeholder="Search..." />
			<Column dataField="BinStatus" caption={"State"} cellRender={binStatusRenderer} width={80} alignment="center" allowFiltering={false} allowResizing={false} />
			<Column dataField="Name" caption={"Name"} filterOperations={nameOps} />
			<Column dataField="Level" caption={"Fill Level"} allowEditing={false} cellRender={fillLevelRenderer} width={120} />
			<Column dataField="LastServicedDate" caption={"Last Serviced"} allowEditing={false} cellRender={dateRenderer} width={120} allowFiltering={false} allowResizing={false} />
			<Column dataField="LastUpdated" caption={"Last Updated"} allowEditing={false} cellRender={lastUpdatedRenderer} width={130} allowFiltering={false} allowResizing={false} />
			{/* Boilerplate columns that are used only inside the Data Grid Form */}
			<Column dataField="CompanyId" caption={"Company"}>
				<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="AssetCategoryId" caption={"Asset Category"}>
				<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="Status" caption={"Status"} visible={false}>
				<Lookup dataSource={BucketStatus} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="Material" caption={"Material"} visible={false}>
				<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="WasteType" caption={"Waste Type"} visible={false}>
				<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="Capacity" caption={"Capacity"} visible={false}>
				<Lookup dataSource={CapacityType} valueExpr="Id" displayExpr="Name" />
			</Column>
		</DataGrid>
	);
}

export default ItinerariesContainersTable;
