// Import React hooks
import { useState, useCallback, useMemo } from "react";

// Import Redux action creators
import { useSelector } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, Lookup, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, Popup, RemoteOperations, Toolbar, Form, Item as ToolbarItem, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";
import notify from "devextreme/ui/notify";

// Import custom components
import { onRowUpdating } from "../../../utils/containerUtils";
import { dateRender } from "../../../utils/adminUtils";
import { http } from "../../../utils/http";

// Define the label for the checkbox editor used in the "Active" field
const activeOptions = {
	text: "Activated"
};

// function cellRender(data: any) {
// 	return <img src={data.value} alt={data.value} />;
// }

// Function that sets initial values for Create Window
const onInitNewRow = (e: {
	data: {
		Name: string;
		Notes: string;
		CompanyId: number;
		CreatedDate: Date;
		ModifiedDate: Date;
		Active: boolean;
		CodeErp: string;
	};
}) => {
	e.data = {
		Name: "",
		Notes: "",
		CompanyId: 1,
		CreatedDate: new Date(),
		ModifiedDate: new Date(),
		Active: true,
		CodeErp: ""
	};
};

function DepartmentsView() {
	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Departments").then((response) => {
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
					.post(process.env.REACT_APP_AUTH_HTTP + "/v1/Departments", values)
					.then((response) => {
						notify(`${values.Imei} was created successfully.`, "success", 2500);
						return response.data;
					})
					.catch(() => {
						notify("Failed to create Department.", "error", 2500);
					});
			},
			update: (key, values) => {
				const data = { ...values, ModifiedDate: new Date() };

				return http
					.put(process.env.REACT_APP_AUTH_HTTP + `/v1/Departments/${key}`, data)
					.then(({ data }) => {
						notify(`${values.Imei} was updated successfully.`, "success", 2500);
						return data;
					})
					.catch(() => {
						notify("Failed to update Department.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_AUTH_HTTP + `/v1/Departments/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
					if (response.status === 200) {
						notify(`Department ${key} was deleted successfully.`, "success", 2500);
					} else {
						notify(`Failed to delete ${key} Department.`, "error", 2500);
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
		<>
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
					<Popup title="Departments" showTitle={true} width="50%" height="40%" animation={undefined} dragEnabled={false} resizeEnabled={false} />
					<Form>
						<FormItem itemType="group" colCount={2} colSpan={2}>
							<FormItem dataField="Name">
								<RequiredRule />
							</FormItem>
							<FormItem dataField="CompanyId">
								<RequiredRule />
							</FormItem>
							<FormItem dataField="Active" editorType="dxCheckBox" editorOptions={activeOptions}>
								<Label visible={false} />
							</FormItem>
							<FormItem dataField="CodeErp">
								<RequiredRule />
							</FormItem>
							<FormItem dataField="Notes" editorType="dxTextArea" colCount={2} colSpan={2} />
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
				<Column dataField="Notes" caption="Notes" allowSorting={true} allowFiltering={true} visible={false} />
				<Column dataField="CompanyId" caption="Company" allowSorting={true} allowFiltering={true} visible={false}>
					<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="CreatedDate" caption="Created Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} dataType="date" cellRender={dateRender} />
				<Column dataField="ModifiedDate" caption="Modified Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} dataType="date" cellRender={dateRender} />
				<Column dataField="Active" caption="Active" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean" />
				<Column dataField="CodeErp" caption="Code ERP" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
				{/* <Column dataField="thumbnailUrl" width={175} allowSorting={false} cellRender={cellRender} allowEditing={false} /> */}
				<Summary>
					<TotalItem column="id" showInColumn="title" displayFormat="Total: {0}" summaryType="count" />
				</Summary>
			</DataGrid>

			{/* <Item ratio={1}>
				<Box className="device-box-inner" direction="col" width="100%" height="100%">
					<Item ratio={1}>
						<DataGrid dataSource={dataSource} width="100%" height="100%" showBorders={true} showRowLines={true} showColumnLines={true} repaintChangesOnly={true} rowAlternationEnabled={true}>
							<RemoteOperations sorting={true} filtering={true} paging={false} />
							<Scrolling mode="virtual" />
							<Sorting mode="single" />
							<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
							<Column dataField="title" caption="Name" allowSorting={true} allowFiltering={true} />
							<Column dataField="email" caption="Email" allowSorting={true} allowFiltering={true} />
							<Summary>
								<TotalItem column="id" showInColumn="title" displayFormat="Total: {0}" summaryType="count" />
							</Summary>
						</DataGrid>
					</Item>
					<Item ratio={1}>
						<DataGrid dataSource={dataSource} width="100%" height="100%" showBorders={true} showRowLines={true} showColumnLines={true} repaintChangesOnly={true} rowAlternationEnabled={true}>
							<RemoteOperations sorting={true} filtering={true} paging={false} />
							<Scrolling mode="virtual" />
							<Sorting mode="single" />
							<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
							<Column dataField="title" caption="Title" allowSorting={true} allowFiltering={true} />
							<Column dataField="body" caption="Body" allowSorting={true} allowFiltering={true} />
							<Summary>
								<TotalItem column="id" showInColumn="title" displayFormat="Total: {0}" summaryType="count" />
							</Summary>
						</DataGrid>
					</Item>
				</Box>
			</Item> */}
		</>
	);
}

export default DepartmentsView;
