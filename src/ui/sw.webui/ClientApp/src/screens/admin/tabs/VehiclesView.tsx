// Import React hooks
import { useState, useCallback, useMemo } from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, Lookup, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, RemoteOperations, Toolbar, Item as ToolbarItem, Popup, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom tools
import { onRowUpdating } from "../../../utils/containerUtils";
import { dateRender } from "../../../utils/adminUtils";

import { http } from "../../../utils/http";

// Import custom components
import VehiclesForm, { editPictureRenderer } from "../../../components/admin/VehiclesForm";

function cellRender(data: any) {
	return <img src={data.value} alt={data.value} />;
}

// Function that sets initial values for Create Window
const onInitNewRow = (e: {
	data: {
		NumPlate: string;
		Brand: string;
		RegisteredDate: Date;
		Type: number;
		Status: number;
		Gas: number;
		Height: number;
		Width: number;
		Axels: number;
		MinTurnRadius: number;
		Length: number;
		CompanyId: number;
		AssetCategoryId: number;
		Name: string;
		CodeErp: string;
		Image: string;
		Description: string;
	};
}) => {
	e.data = {
		NumPlate: "",
		Brand: "",
		RegisteredDate: new Date(),
		Type: 1,
		Status: 1,
		Gas: 1,
		Height: 1,
		Width: 1,
		Axels: 1,
		MinTurnRadius: 1,
		Length: 1,
		CompanyId: 1,
		AssetCategoryId: 2,
		Name: "",
		CodeErp: "",
		Image: "img",
		Description: ""
	};
};

function VehiclesView() {
	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Vehicles").then((response) => {
					if (response.status === 200) {
						return {
							data: response.data.Value,
							totalCount: response.data.Value.length
						};
					}

					return {
						data: [],
						totalCount: 0
					};
				});
			},
			insert: (values) => {
				return http
					.post(process.env.REACT_APP_ASSET_HTTP + "/v1/Vehicles", values)
					.then((response) => {
						notify(`${values.Name} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create Vehicle.", "error", 2500);
					});
			},
			update: (key, values) => {
				return http
					.put(process.env.REACT_APP_ASSET_HTTP + `/v1/Vehicles/${key}`, values)
					.then(({ data }) => {
						notify(`${values.Name} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update Vehicle.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_ASSET_HTTP + `/v1/Vehicles/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
					if (response.status === 200) {
						notify(`Vehicle ${key} was deleted successfully.`, "success", 2500);
					} else {
						notify(`Failed to delete ${key} Vehicle.`, "error", 2500);
					}
				});
			}
		});
	}, []);

	// Function that updates state whenever a cell changes
	const onEditingChange = useCallback((changes) => {
		setGridChanges(changes);
	}, []);

	return (
		<DataGrid className="data-grid-common" dataSource={dataSource} width="100%" height="100%" showBorders={true} showRowLines={true} showColumnLines={true} repaintChangesOnly={true} rowAlternationEnabled={true} onInitNewRow={onInitNewRow} onRowUpdating={onRowUpdating}>
			<Editing mode="popup" changes={gridChanges} onChangesChange={onEditingChange} allowAdding={true} allowUpdating={true} allowDeleting={true} selectTextOnEditStart={false} startEditAction="click" useIcons={true}>
				<Popup title="Vehicles" showTitle={true} width="60%" height="90%" animation={undefined} dragEnabled={false} resizeEnabled={false} />
				{VehiclesForm()}
			</Editing>
			<RemoteOperations sorting={false} filtering={false} paging={false} />
			<Scrolling mode="virtual" showScrollbar="always" />
			<ColumnFixing enabled={true} />
			<Sorting mode="single" />
			<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
			<HeaderFilter allowSearch={true} visible={true} />
			<FilterRow visible={true} applyFilter="Immediately" />
			<ColumnChooser enabled={true} allowSearch={true} mode="select" />
			<Toolbar>
				<ToolbarItem location="after" name="addRowButton" />
				<ToolbarItem location="after" name="saveButton" />
				<ToolbarItem location="after" name="revertButton" />
				<ToolbarItem location="before" name="columnChooserButton" />
			</Toolbar>
			<Column dataField="NumPlate" caption="Plate Number" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Name" caption="Name" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Brand" caption="Brand" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} />
			<Column dataField="RegisteredDate" caption="Registered Date" width={150} allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} dataType="date" cellRender={dateRender} />
			<Column dataField="Type" caption="Type" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Status" caption="Status" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Gas" caption="Gas" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Height" caption="Height" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Width" caption="Width" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Axels" caption="Axels" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="MinTurnRadius" caption="Minimum Turn Radius" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Length" caption="Length" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="CompanyId" caption="Company" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true}>
				<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="AssetCategoryId" caption="Asset Category" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true}>
				<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="Image" caption="Image" width={175} allowSorting={false} allowFiltering={false} cellRender={cellRender} editCellRender={editPictureRenderer} />
			<Column dataField="Description" caption="Description" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Column dataField="CodeErp" caption="Code ERP" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Summary>
				<TotalItem column="id" showInColumn="NumPlate" displayFormat="Total: {0}" summaryType="count" />
			</Summary>
		</DataGrid>
	);
}

export default VehiclesView;
