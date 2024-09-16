// Import React hooks
import React, { useState, useCallback, useMemo } from "react";

// Import DevExtreme components
import DataGrid, { Column, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, RemoteOperations, Toolbar, Item as ToolbarItem, Popup, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom tools
import { onRowUpdating } from "../../../utils/containerUtils";
import { http } from "../../../utils/http";

// import custom components
import { SensorTypesForm } from "../../../components/admin";

// Function that sets initial values for Create Window
export const onInitNewRow = (e: {
	data: {
		Name: string;
		ShowAtStatus: boolean;
		StatusExpiryMinutes: number;
		ShowOnMap: boolean;
		ShowAtReport: boolean;
		ShowAtChart: boolean;
		ResetValues: boolean;
		SumValues: boolean;
		Precision: number;
		Tunit: string;
		CalcPosition: boolean;
		CodeErp: string;
	};
}) => {
	e.data = {
		Name: "",
		ShowAtStatus: true,
		StatusExpiryMinutes: 0,
		ShowOnMap: true,
		ShowAtReport: true,
		ShowAtChart: true,
		ResetValues: true,
		SumValues: true,
		Precision: 1,
		Tunit: "",
		CalcPosition: true,
		CodeErp: ""
	};
};

function SensorTypesView() {
	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load: (props) => {
					return http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/SensorTypes").then((response) => {
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
					console.log(values);
					return http
						.post(process.env.REACT_APP_ASSET_HTTP + "/v1/SensorTypes", values)
						.then((response) => {
							notify(`${values.Name} was created successfully.`, "success", 2500);

							return response.data;
						})
						.catch(() => {
							notify("Failed to create Sensor Type.", "error", 2500);
						});
				},
				update: (key, values) => {
					return http
						.put(process.env.REACT_APP_ASSET_HTTP + `/v1/SensorTypes/${key}`, values)
						.then(({ data }) => {
							notify(`${values.Name} was updated successfully.`, "success", 2500);

							return data;
						})
						.catch(() => {
							notify("Failed to update Sensor Type.", "error", 2500);
						});
				},
				remove: (key) => {
					return http.delete(process.env.REACT_APP_ASSET_HTTP + `/v1/SensorTypes/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
						if (response.status === 200) {
							notify(`Sensor Type ${key} was deleted successfully.`, "success", 2500);
						} else {
							notify(`Failed to delete ${key} Sensor Type.`, "error", 2500);
						}
					});
				}
			}),
		[]
	);

	// Function that updates state whenever a cell changes
	const onEditingChange = useCallback((changes) => {
		setGridChanges(changes);
	}, []);

	return (
		<DataGrid
			className="data-grid-common"
			dataSource={dataSource}
			width="100%"
			height="100%"
			showBorders={true}
			showRowLines={true}
			showColumnLines={true}
			repaintChangesOnly={true}
			rowAlternationEnabled={true}
			allowColumnResizing={true}
			columnResizingMode="nextColumn"
			columnMinWidth={50}
			onInitNewRow={onInitNewRow}
			onRowUpdating={onRowUpdating}
		>
			<Editing mode="popup" changes={gridChanges} onChangesChange={onEditingChange} allowAdding={true} allowUpdating={true} allowDeleting={true} selectTextOnEditStart={false} startEditAction="click" useIcons={true}>
				<Popup title="Sensor Types" showTitle={true} width="45%" height="50%" animation={undefined} dragEnabled={false} resizeEnabled={false} />
				{SensorTypesForm()}
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
			<Column dataField="Name" caption="Name" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="StatusExpiryMinutes" caption="Status Expiry Minutes" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="Precision" caption="Precision" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} width={80} />
			<Column dataField="Tunit" caption="T Unit" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="SensorTypeIndex" caption="Sensor Type Index" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="ShowAtStatus" caption="Show At Status" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="ShowOnMap" caption="Show On Map" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="ShowAtReport" caption="Show At Report" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="ShowAtChart" caption="Show At Chart" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="ResetValues" caption="Reset Values" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="SumValues" caption="Sum Values" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="CalcPosition" caption="Calc Position" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
			<Column dataField="CodeErp" caption="Code ERP" allowSorting={true} allowFiltering={true} visible={false} />
			<Summary>
				<TotalItem column="id" showInColumn="Name" displayFormat="Total: {0}" summaryType="count" />
			</Summary>
		</DataGrid>
	);
}

export default SensorTypesView;
