// Import React hooks
import { useState, useEffect, useCallback, useMemo, useReducer } from "react";

// Import DevExtreme components
import DataGrid, { Column, Button as DataGridButton, Lookup, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, MasterDetail, RemoteOperations, Toolbar, Item as ToolbarItem, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import { Popup } from "devextreme-react/popup";
import Button from "devextreme-react/button";
import notify from "devextreme/ui/notify";

// Import custom tools
import { getDeviceModels } from "../../../utils/apis/assets";
import { dateRender, activatedHeaderFilter, enabledHeaderFilter } from "../../../utils/adminUtils";
import { onRowUpdating } from "../../../utils/containerUtils";
import { http } from "../../../utils/http";

// Import custom components
import DevicesForm from "../../../components/admin/DevicesForm";
import DeviceDetailsView from "./DeviceDetailsView";

import "../../../styles/admin/Devices.scss";

// Initial state of the device editing form popup
const initPopupState = {
	formData: {},
	popupVisible: false,
	popupMode: ""
};

// Function that that sets initial values for the data object passed as an argument. It is used to set initial values when a new row is added to the table.
const popupReducer = (state: any, action: { type: any; data: any; popupMode: any }) => {
	switch (action.type) {
		case "initPopup":
			return {
				formData: action.data,
				popupVisible: true,
				popupMode: action.popupMode
			};
		case "hidePopup":
			return {
				popupVisible: false
			};
		default:
			break;
	}
};

// Array of objects used to visualize data for the Activation column
const activationData = [
	{
		ID: 1,
		Name: "Activation 1"
	},
	{
		ID: 2,
		Name: "Activation 2"
	},
	{
		ID: 3,
		Name: "Activation 3"
	}
];

// Array of objects used to visualize data for the ProvisioningBy column
const provisioningData = [
	{
		ID: 1,
		Name: "Provisioning 1"
	},
	{
		ID: 2,
		Name: "Provisioning 2"
	},
	{
		ID: 3,
		Name: "Provisioning 3"
	}
];

// Array of objects used to visualize data for the ResetBy column
const resetData = [
	{
		ID: 1,
		Name: "Reset 1"
	},
	{
		ID: 2,
		Name: "Reset 2"
	},
	{
		ID: 3,
		Name: "Reset 3"
	}
];

// Function that sets initial values for Create Window
export const onInitNewRow = (e: {
	data: {
		Activated: boolean;
		ActivationBy: number;
		ActivationCode: string;
		ActivationDate: Date;
		DeviceModelId: number;
		Enabled: boolean;
		Imei: string;
		IpAddress: string;
		LastReceivedDate: Date;
		LastRecordedDate: Date;
		ProvisioningBy: number;
		ProvisioningCode: string;
		ProvisioningDate: Date;
		ResetBy: number;
		ResetCode: string;
		ResetDate: Date;
		SerialNumber: string;
		CodeErp: string;
	};
}) => {
	e.data = {
		Activated: true,
		ActivationBy: 1,
		ActivationCode: "act",
		ActivationDate: new Date(),
		DeviceModelId: 2,
		Enabled: true,
		Imei: "ABC-1",
		IpAddress: "192.168.001.001",
		LastReceivedDate: new Date(),
		LastRecordedDate: new Date(),
		ProvisioningBy: 1,
		ProvisioningCode: "prov",
		ProvisioningDate: new Date(),
		ResetBy: 1,
		ResetCode: "res",
		ResetDate: new Date(),
		SerialNumber: "1001",
		CodeErp: ""
	};
};

// async function processBatchRequest(url: string, changes: any, component: { refresh: (arg0: boolean) => any; cancelEditData: () => void }) {
// 	// Send data to server in any way you want
// 	switch (changes.type) {
// 		case "insert":
// 			await http
// 				.post(url, changes.data)
// 				.then((response) => {
// 					return response;
// 				})
// 				.catch((error) => {
// 					throw new Error("Failed to send data");
// 				});
// 			break;
// 		case "update":
// 			await http
// 				.put(url + `/${changes.key}`, changes.data)
// 				.then((response) => {
// 					return response;
// 				})
// 				.catch((error) => {
// 					throw new Error("Failed to send data");
// 				});
// 			break;
// 		case "remove":
// 			await http
// 				.delete(url + `/soft/${changes.key}`, {
// 					data: {
// 						deletedReason: `Deleted ${changes.key} from sw Admin`
// 					}
// 				})
// 				.then((response) => {
// 					return response;
// 				})
// 				.catch((error) => {
// 					throw new Error("Failed to send data");
// 				});
// 			break;
// 		default:
// 			// Send data to server in any way you want
// 			await http
// 				.get(url, { params: {} })
// 				.then((response) => {
// 					console.log(response);
// 					return response;
// 				})
// 				.catch((error) => {
// 					throw new Error("Failed to send data");
// 				});
// 			break;
// 	}

// 	// Force component to refresh to call latest data and clear edit mode to remove the styles for each change
// 	await component.refresh(true);
// 	component.cancelEditData();
// }

function DevicesView() {
	// State that holds possible device models
	const [deviceModels, setDeviceModels] = useState([]);

	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	// Reducer that holds data for created, edited item
	const [{ formData, popupVisible, popupMode }, dispatchPopup] = useReducer<(arg1: any, actions: any) => any>(popupReducer, initPopupState);

	// Function that dispatches an action to update the popupVisible, popupMode, and formData state variables.
	const showPopup = (popupMode: any, data: any) => {
		dispatchPopup({ type: "initPopup", data, popupMode });
	};

	// Function that calls the showPopup function with the "Add" mode and an empty data object
	const addClick = () => {
		showPopup("Add", {});
	};

	// Function that calls the showPopup function with the "Edit" mode and the data from the row that was clicked
	const editClick = (e: any) => {
		showPopup("Edit", { ...e.row.data });
	};

	// Function that dispatches an action to update the popupVisible state variable
	const onHiding = () => {
		dispatchPopup({ type: "hidePopup" });
	};

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load: (props) => {
					return http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Devices").then((response) => {
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
						.post(process.env.REACT_APP_ASSET_HTTP + "/v1/Devices", values)
						.then((response) => {
							notify(`${values.Imei} was created successfully.`, "success", 2500);

							return response.data;
						})
						.catch(() => {
							notify("Failed to create Device.", "error", 2500);
						});
				},
				update: (key, values) => {
					return http
						.put(process.env.REACT_APP_ASSET_HTTP + `/v1/Devices/${key}`, values)
						.then(({ data }) => {
							notify(`${values.Imei} was updated successfully.`, "success", 2500);

							return data;
						})
						.catch(() => {
							notify("Failed to update Device.", "error", 2500);
						});
				},
				remove: (key) => {
					return http.delete(process.env.REACT_APP_ASSET_HTTP + `/v1/Devices/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
						if (response.status === 200) {
							notify(`Device ${key} was deleted successfully.`, "success", 2500);
						} else {
							notify(`Failed to delete ${key} Device.`, "error", 2500);
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

	// Get device models from API and add to state
	useEffect(() => {
		(async () => {
			const deviceModelData = await getDeviceModels();

			setDeviceModels(deviceModelData);
		})();
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
				<Editing mode="popup" changes={gridChanges} onChangesChange={onEditingChange} allowAdding={true} allowUpdating={true} allowDeleting={true} selectTextOnEditStart={false} startEditAction="click" useIcons={true} />
				<Sorting mode="single" />
				<ColumnFixing enabled={true} />
				<Scrolling mode="virtual" showScrollbar="always" />
				<HeaderFilter allowSearch={true} visible={true} />
				<FilterRow visible={true} applyFilter="Immediately" />
				<MasterDetail enabled={true} component={DeviceDetailsView} />
				<ColumnChooser enabled={true} allowSearch={true} mode="select" />
				<RemoteOperations sorting={false} filtering={false} paging={false} />
				<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
				<Toolbar>
					<ToolbarItem location="after">
						<Button icon="add" onClick={addClick} />
					</ToolbarItem>
					<ToolbarItem location="after" name="saveButton" />
					<ToolbarItem location="after" name="revertButton" />
					<ToolbarItem location="before" name="columnChooserButton" />
				</Toolbar>
				<Column dataField="DeviceModelId" caption="Device Model Id" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false}>
					<Lookup dataSource={deviceModels} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="Imei" caption="IMEI" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
				<Column dataField="SerialNumber" caption="Serial Number" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
				<Column dataField="ActivationBy" visible={false}>
					<Lookup dataSource={activationData} valueExpr="ID" displayExpr="Name" />
				</Column>
				<Column dataField="ActivationCode" caption="Activation Code" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} />
				<Column dataField="ActivationDate" caption="Activation Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} cellRender={dateRender} />
				<Column dataField="ProvisioningCode" />
				<Column dataField="ProvisioningBy" visible={false}>
					<Lookup dataSource={provisioningData} valueExpr="ID" displayExpr="Name" />
				</Column>
				<Column dataField="ProvisioningDate" caption="Provisioning Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} cellRender={dateRender} />
				<Column dataField="ResetCode" caption="Reset Code" visible={false} />
				<Column dataField="ResetBy" caption="Reset By" visible={false}>
					<Lookup dataSource={resetData} valueExpr="ID" displayExpr="Name" />
				</Column>
				<Column dataField="ResetDate" caption="Reset Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} cellRender={dateRender} />
				<Column dataField="Activated" caption="Activated" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean">
					<HeaderFilter dataSource={activatedHeaderFilter} />
				</Column>
				<Column dataField="Enabled" caption="Enabled" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean">
					<HeaderFilter dataSource={enabledHeaderFilter} />
				</Column>
				<Column dataField="IpAddress" caption="Ip Address" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
				<Column dataField="Port" caption="Port" visible={false} allowSorting={true} allowFiltering={true} allowHeaderFiltering={true} />
				<Column dataField="LastRecordedDate" caption="Last Recorded Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} cellRender={dateRender} />
				<Column dataField="LastReceivedDate" caption="Last Received Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} cellRender={dateRender} />
				<Column dataField="CodeErp" caption="Code ERP" visible={false} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
				<Column type="buttons">
					<DataGridButton name="edit" onClick={editClick} />
					<DataGridButton name="delete" />
				</Column>
				<Summary>
					<TotalItem column="id" showInColumn="DeviceModelId" displayFormat="Total: {0}" summaryType="count" />
				</Summary>
			</DataGrid>
			<Popup title={popupMode} closeOnOutsideClick={true} visible={popupVisible} onHiding={onHiding} showCloseButton={true} width={700} height={600}>
				<DevicesForm formData={formData} popupMode={popupMode} dispatchPopup={dispatchPopup} dataSource={dataSource} />
			</Popup>
		</>
	);
}

export default DevicesView;
