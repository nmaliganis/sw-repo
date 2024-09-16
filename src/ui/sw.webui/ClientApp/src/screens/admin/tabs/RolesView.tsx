// Import React hooks
import { useState, useCallback, useMemo } from "react";

// Import Redux action creators
import { setRolesUpdatedData } from "../../../redux/slices/loginSlice";
import { useDispatch } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, RemoteOperations, Toolbar, Item as ToolbarItem, Popup, Form, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom tools
import { onRowUpdating } from "../../../utils/containerUtils";
import { activeHeaderFilter, dateRender } from "../../../utils/adminUtils";
import { http } from "../../../utils/http";

// Define the label for the checkbox editor used in the "Active" field
const activeOptions = {
	text: "Active"
};

// Function that sets initial values for Create Window
const onInitNewRow = (e: {
	data: {
		Name: string;
		CreatedDate: Date;
		Active: boolean;
		CodeErp: string;
	};
}) => {
	e.data = {
		Name: "",
		CreatedDate: new Date(),
		Active: true,
		CodeErp: ""
	};
};

function RolesView() {
	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	const dispatch = useDispatch();

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Roles").then((response) => {
					if (response.status === 200) {
						dispatch(setRolesUpdatedData(response.data.Value));
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
					.post(process.env.REACT_APP_AUTH_HTTP + "/v1/Roles", values)
					.then((response) => {
						notify(`${values.Imei} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create Role.", "error", 2500);
					});
			},
			update: (key, values) => {
				return http
					.put(process.env.REACT_APP_AUTH_HTTP + `/v1/Roles/${key}`, values)
					.then(({ data }) => {
						notify(`${values.Imei} was created successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update Role.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_AUTH_HTTP + `/v1/Roles/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
					if (response.status === 200) {
						notify(`Role ${key} was deleted successfully.`, "success", 2500);
					} else {
						notify(`Failed to delete ${key} Role.`, "error", 2500);
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
				<Popup title="Roles" showTitle={true} width="50%" height="25%" animation={undefined} dragEnabled={false} resizeEnabled={false} />
				<Form>
					<FormItem itemType="group" colCount={2} colSpan={2}>
						<FormItem dataField="Name">
							<RequiredRule />
						</FormItem>
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
			<Column dataField="Active" caption="Active" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean">
				<HeaderFilter dataSource={activeHeaderFilter} />
			</Column>
			<Column dataField="CreatedDate" caption="Created Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} dataType="date" cellRender={dateRender} />
			<Column dataField="ModifiedDate" caption="Modified Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} dataType="date" cellRender={dateRender} />
			<Column dataField="CodeErp" visible={false} allowSorting={true} allowFiltering={true} />
			<Summary>
				<TotalItem column="id" showInColumn="Name" displayFormat="Total: {0}" summaryType="count" />
			</Summary>
		</DataGrid>
	);
}

export default RolesView;
