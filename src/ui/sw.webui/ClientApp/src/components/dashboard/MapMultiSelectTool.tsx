// Import React hooks
import { useState } from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import Leaflet and map tools
import L from "leaflet";
import { useMapEvents } from "react-leaflet";

// Import Devextreme components
import Popup from "devextreme-react/popup";
import DataGrid, { Column, Scrolling, Sorting, ColumnChooser, Lookup, Export, Toolbar, Item as ToolbarItem } from "devextreme-react/data-grid";

// Import custom tools
import { BucketStatus, CapacityType, MaterialType, streamType } from "../../utils/consts";
import { dateRenderer, binStatusRenderer } from "../../utils/containerUtils";

function MapMultiSelectTool({ mapData }: { mapData: any[] }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	// States that handles marker selection
	const [startPoint, setStartPoint] = useState(null);
	const [selectedItems, setSelectedItems] = useState<any[]>([]);

	// Object that handles state on mouse actions
	useMapEvents({
		mousedown: (e: any) => {
			if (e.originalEvent.shiftKey) {
				// check if the shift key been pressed
				setStartPoint(e.latlng);
			}
		},
		mouseup: (e) => {
			if (e.originalEvent.shiftKey && startPoint) {
				const bounds = new L.LatLngBounds(startPoint, e.latlng); // creates a class which will help to identify if an element is within the selection boundaries
				const selectedIds = [];

				for (let i = 0; i < mapData?.length; i++) {
					const point = new L.LatLng(mapData[i].Latitude, mapData[i].Longitude);

					bounds.contains(point) && selectedIds.push(mapData[i] as never);
				}
				setSelectedItems(selectedIds);
				setStartPoint(null);
			}
		}
	});

	const onPopupHiding = () => {
		setSelectedItems([]);
	};

	return (
		<Popup visible={selectedItems.length ? true : false} onHiding={onPopupHiding} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={true} showTitle={true} title="Selected Containers" container=".dx-viewport" width="60%" height="80%">
			<DataGrid width="100%" height="100%" dataSource={selectedItems} showBorders={true} showColumnLines={true} allowColumnResizing={true} rowAlternationEnabled={true} allowColumnReordering={true} hoverStateEnabled={true} autoNavigateToFocusedRow={false}>
				<Toolbar>
					<ToolbarItem name="columnChooserButton" location="before" />
					<ToolbarItem name="exportButton" location="after" />
				</Toolbar>
				<Sorting mode="single" />
				<Scrolling mode="virtual" rowRenderingMode="virtual" />
				<Export enabled={true} allowExportSelectedData={false} />
				<ColumnChooser enabled={true} allowSearch={true} mode="select" />
				<Column dataField="BinStatus" caption={"State"} cellRender={binStatusRenderer} width={80} alignment="center" allowFiltering={false} allowResizing={false} />
				<Column dataField="Name" caption={"Name"} />
				<Column dataField="LastServicedDate" caption={"Last updated"} cellRender={dateRenderer} width={120} allowFiltering={false} allowResizing={false} />
				<Column dataField="CompanyId" caption={"Company"}>
					<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="AssetCategoryId" caption={"Asset Category"}>
					<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="Status">
					<Lookup dataSource={BucketStatus} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="Material">
					<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="WasteType">
					<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="Capacity">
					<Lookup dataSource={CapacityType} valueExpr="Id" displayExpr="Name" />
				</Column>
			</DataGrid>
		</Popup>
	);
}

export default MapMultiSelectTool;
