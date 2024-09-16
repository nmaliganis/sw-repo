// Import React hooks
import { useEffect, useRef } from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import Devextreme components

import Box, { Item } from "devextreme-react/box";

import DataGrid, { Column, ColumnChooser, Selection, Sorting, RemoteOperations, Scrolling, Lookup, Summary, TotalItem } from "devextreme-react/data-grid";

// Import custom tools

import { BucketStatus, CapacityType, MaterialType, streamType } from "../../utils/consts";
import { dateRenderer, binStatusRenderer, fillLevelRenderer, lastUpdatedRenderer } from "../../utils/containerUtils";

function TemplateContainersTable() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);
	const { selectedTemplate } = useSelector((state: any) => state.itinerary);

	const dataGridContainerRef = useRef<any>();

	// Update data when websocket returns new data
	useEffect(() => {
		if (dataGridContainerRef.current) dataGridContainerRef.current.instance.refresh();
	}, [selectedTemplate]);

	return (
		<Box className="table-background" direction="col" width="100%" height="100%">
			<Item ratio={1}>
				<DataGrid
					ref={dataGridContainerRef}
					width="100%"
					height="100%"
					keyExpr="Id"
					dataSource={selectedTemplate?.AssignedContainers || []}
					showBorders={true}
					showColumnLines={true}
					allowColumnResizing={true}
					rowAlternationEnabled={true}
					allowColumnReordering={true}
					focusedRowEnabled={true}
					hoverStateEnabled={true}
					autoNavigateToFocusedRow={false}
				>
					<Sorting mode="single" />
					<Scrolling mode="virtual" rowRenderingMode="virtual" />
					<ColumnChooser enabled={true} allowSearch={true} mode="select" />
					<RemoteOperations sorting={false} filtering={false} paging={false} />
					<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
					<Column dataField="BinStatus" caption={"State"} cellRender={binStatusRenderer} width={80} alignment="center" allowFiltering={false} allowResizing={false} />
					<Column dataField="Name" caption={"Name"} />
					<Column dataField="Level" caption={"Fill Level"} allowEditing={false} cellRender={fillLevelRenderer} width={120} />
					<Column dataField="LastServicedDate" caption={"Last Serviced"} allowEditing={false} cellRender={dateRenderer} width={120} allowFiltering={false} allowResizing={false} />
					<Column dataField="LastUpdated" caption={"Last Updated"} allowEditing={false} cellRender={lastUpdatedRenderer} width={130} allowFiltering={false} allowResizing={false} />
					<Column dataField="CompanyId" caption={"Company"}>
						<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="AssetCategoryId" caption={"Asset Category"}>
						<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="Status" visible={false}>
						<Lookup dataSource={BucketStatus} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="Material" visible={false}>
						<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="WasteType" visible={false}>
						<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="Capacity" visible={false}>
						<Lookup dataSource={CapacityType} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Summary>
						<TotalItem column="BinStatus" summaryType="count" displayFormat="Total: {0}" />
					</Summary>
				</DataGrid>
			</Item>
		</Box>
	);
}

export default TemplateContainersTable;
