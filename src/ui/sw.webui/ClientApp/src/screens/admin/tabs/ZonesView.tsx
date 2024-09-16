import { useReducer, useCallback, useMemo, useRef } from "react";

// Import Devextreme components
import notify from "devextreme/ui/notify";
import Button from "devextreme-react/button";
import CustomStore from "devextreme/data/custom_store";
import DataGrid, { Column, ColumnChooser, Sorting, HeaderFilter, RemoteOperations, Scrolling, Button as DataGridButton, Toolbar, Item as ToolbarItem, Editing } from "devextreme-react/data-grid";

// Import custom components
import { ZonesForm } from "../../../components/admin";

// Import custom tools
import { http } from "../../../utils/http";
import { activeHeaderFilter } from "../../../utils/adminUtils";

const initForm = { type: "", title: "", isNewRecord: false, visible: false, data: null };

// Function that handles state reducer
const formReducer = (state, action) => {
	switch (action.type) {
		case "ADD":
			return { type: "ADD", title: "Create Zone", isNewRecord: true, visible: true, data: null };
		case "EDIT":
			return { type: "EDIT", title: "Edit Zone", isNewRecord: false, visible: true, data: action.payload };
		case "CANCEL":
			return { ...initForm, type: "CANCEL" };
		case "FINISH":
			return { ...initForm, type: "FINISH", data: action.data };

		default:
			throw new Error();
	}
};

function ZonesView() {
	const [formState, dispatchForm] = useReducer(formReducer, initForm);

	const zonesDataGridRef = useRef<any>();

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Zones").then((response) => {
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
			remove: (key) => {
				return http.delete(process.env.REACT_APP_ASSET_HTTP + `/v1/Zones/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
					if (response.status === 200) {
						notify(`Zone ${key} was deleted successfully.`, "success", 2500);
					} else {
						notify(`Failed to delete ${key} Zone.`, "error", 2500);
					}
				});
			}
		});
	}, []);

	const onSave = useCallback(
		(form) => {
			if (formState.isNewRecord) {
				http
					.post(process.env.REACT_APP_ASSET_HTTP + "/v1/Zones", form.data)
					.then((response) => {
						notify(`${form.data.Name} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create Zone.", "error", 2500);
					});
			} else {
				http
					.put(process.env.REACT_APP_ASSET_HTTP + `/v1/Zones/${form.data.Id}`, form.data)
					.then(({ data }) => {
						notify(`${form.data.Name} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update Zone.", "error", 2500);
					});
			}

			zonesDataGridRef.current.instance.refresh(true);
			dispatchForm({ type: "FINISH" });
		},
		[formState]
	);

	const addRow = useCallback(() => {
		dispatchForm({ type: "ADD" });
	}, []);

	const editRow = useCallback((e) => {
		dispatchForm({ type: "EDIT", data: e.row.data });
	}, []);

	return (
		<>
			<ZonesForm onSave={onSave} formState={formState} dispatchForm={dispatchForm} />
			<DataGrid
				className="data-grid-common"
				ref={zonesDataGridRef}
				width="100%"
				height="100%"
				dataSource={dataSource}
				showBorders={true}
				showColumnLines={true}
				columnMinWidth={80}
				allowColumnResizing={true}
				columnResizingMode="widget"
				rowAlternationEnabled={true}
				allowColumnReordering={true}
				focusedRowEnabled={true}
				hoverStateEnabled={true}
				autoNavigateToFocusedRow={false}
			>
				<Toolbar>
					<ToolbarItem location="after">
						<Button icon="add" type="normal" stylingMode="contained" onClick={addRow} />
					</ToolbarItem>
				</Toolbar>
				<Sorting mode="single" />
				<Scrolling mode="virtual" rowRenderingMode="virtual" />
				<ColumnChooser enabled={false} allowSearch={true} mode="select" />
				<Editing allowUpdating={true} allowAdding={true} allowDeleting={true} useIcons={true} mode="popup" />
				<RemoteOperations sorting={false} filtering={false} paging={false} />
				<Column dataField="Name" caption={"Zone"} />
				<Column dataField="Active" dataType="boolean" caption={"Active"} width={80}>
					<HeaderFilter dataSource={activeHeaderFilter} />
				</Column>
				<Column type="buttons">
					<DataGridButton name="edit" onClick={editRow} />
					<DataGridButton name="delete" />
				</Column>
			</DataGrid>
		</>
	);
}

export default ZonesView;
