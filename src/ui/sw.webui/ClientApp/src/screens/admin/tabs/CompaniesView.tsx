// Import React hooks
import { useState, useCallback, useMemo } from "react";

// Import Redux action creators
import { setCompanyUpdatedData } from "../../../redux/slices/loginSlice";
import { useDispatch } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, Popup, RemoteOperations, Toolbar, Item as ToolbarItem, Form, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom tools
import { activeHeaderFilter, dateRender } from "../../../utils/adminUtils";
import { onRowUpdating } from "../../../utils/containerUtils";
import { http } from "../../../utils/http";

import "../../../styles/admin/Devices.scss";

// Define the label for the checkbox editor used in the "Active" field
const activeOptions = {
	text: "Active"
};

// Function that sets initial values for Create Window
export const onInitNewRow = (e: {
	data: {
		Name: string;
		Description: string;
		Active: boolean;
		CreatedDate: Date;
		ModifiedDate: Date;
		CodeErp: string;
	};
}) => {
	e.data = {
		Name: "",
		Description: "",
		Active: true,
		CreatedDate: new Date(),
		ModifiedDate: new Date(),
		CodeErp: ""
	};
};

function EmployeesView() {
	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	const dispatch = useDispatch();

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Companies").then((response) => {
					if (response.status === 200) {
						dispatch(setCompanyUpdatedData(response.data.Value));

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
					.post(process.env.REACT_APP_AUTH_HTTP + "/v1/Companies", values)
					.then((response) => {
						notify(`${values.Name} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create Company.", "error", 2500);
					});
			},
			update: (key, values) => {
				const data = { ...values, ModifiedDate: new Date() };

				return http
					.put(process.env.REACT_APP_AUTH_HTTP + `/v1/Companies/${key}`, data)
					.then(({ data }) => {
						notify(`${values.Name} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update Company.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_AUTH_HTTP + `/v1/Companies/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
					if (response.status === 200) {
						notify(`Company ${key} was deleted successfully.`, "success", 2500);
					} else {
						notify(`Failed to delete ${key} Company.`, "error", 2500);
					}
				});
			}
		});
	}, [dispatch]);

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
				<Popup title="Company" showTitle={true} width="40%" height="40%" animation={undefined} dragEnabled={false} resizeEnabled={false} />
				<Form>
					<FormItem itemType="group" colCount={1} colSpan={2}>
						<FormItem dataField="Name">
							<RequiredRule />
						</FormItem>
						<FormItem dataField="Description" />
						<FormItem dataField="CodeErp">
							<RequiredRule />
						</FormItem>
						<FormItem dataField="Active" editorType="dxCheckBox" editorOptions={activeOptions}>
							<Label visible={false} />
						</FormItem>
					</FormItem>
				</Form>
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
			<Column dataField="Active" caption="Active" dataType="boolean" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true}>
				<HeaderFilter dataSource={activeHeaderFilter} />
			</Column>
			<Column dataField="Description" caption="Description" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="CreatedDate" caption="Created Date" dataType="datetime" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} allowEditing={false} cellRender={dateRender} />
			<Column dataField="ModifiedDate" caption="Modified Date" dataType="datetime" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} allowEditing={false} cellRender={dateRender} />
			<Column dataField="CodeErp" caption="Code ERP" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Summary>
				<TotalItem column="Id" showInColumn="Name" displayFormat="Total: {0}" summaryType="count" />
			</Summary>
		</DataGrid>
	);
}

export default EmployeesView;
