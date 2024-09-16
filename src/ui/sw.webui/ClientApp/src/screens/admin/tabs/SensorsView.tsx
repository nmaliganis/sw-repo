// Import React hooks
import { useState, useEffect, useCallback, useMemo } from "react";

// Import DevExtreme components
import DataGrid, { Column, Lookup, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, RemoteOperations, Toolbar, Item as ToolbarItem, Popup, Summary, TotalItem } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom tools
import { onRowUpdating } from "../../../utils/containerUtils";
import { http } from "../../../utils/http";
import { dateRender, activeHeaderFilter, visibleHeaderFilter } from "../../../utils/adminUtils";

// import custom components
import { getAssetCategories, getDeviceModels, getSensorTypes } from "../../../utils/apis/assets";
import { SensorsForm } from "../../../components/admin";

// Function that sets initial values for Create Window
export const onInitNewRow = (e: {
	data: {
		AssetId: number;
		DeviceId: number;
		SensorTypeId: number;
		Params: string;
		Name: string;
		CodeErp: string;
		IsActive: boolean;
		IsVisible: boolean;
		Order: number;
		MinValue: number;
		MaxValue: number;
		MinNotifyValue: number;
		MaxNotifyValue: number;
		HighThreshold: number;
		LowThreshold: number;
		SamplingInterval: number;
		ReportingInterval: number;
	};
}) => {
	e.data = {
		AssetId: 1,
		DeviceId: 1,
		SensorTypeId: 1,
		Params: "{}",
		Name: "",
		CodeErp: " ",
		IsActive: true,
		IsVisible: true,
		Order: 0,
		MinValue: 0,
		MaxValue: 0,
		MinNotifyValue: 0,
		MaxNotifyValue: 0,
		HighThreshold: 0,
		LowThreshold: 0,
		SamplingInterval: 0,
		ReportingInterval: 0
	};
};

function SensorsView() {
	// State that hold data for DataGrid visualization
	const [assetCategories, setAssetCategories] = useState([]);
	const [deviceModels, setDeviceModels] = useState([]);
	const [sensorTypes, setSensorTypes] = useState([]);

	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load: (props) => {
					return http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Sensors").then((response) => {
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
						.post(process.env.REACT_APP_ASSET_HTTP + "/v1/Sensors", values)
						.then((response) => {
							notify(`${values.Name} was created successfully.`, "success", 2500);

							return response.data;
						})
						.catch(() => {
							notify("Failed to create Sensor.", "error", 2500);
						});
				},
				update: (key, values) => {
					return http
						.put(process.env.REACT_APP_ASSET_HTTP + `/v1/Sensors/${key}`, values)
						.then(({ data }) => {
							notify(`${values.Name} was updated successfully.`, "success", 2500);

							return data;
						})
						.catch(() => {
							notify("Failed to update Sensor.", "error", 2500);
						});
				},
				remove: (key) => {
					return http.delete(process.env.REACT_APP_ASSET_HTTP + `/v1/Sensors/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
						if (response.status === 200) {
							notify(`Sensor ${key} was deleted successfully.`, "success", 2500);
						} else {
							notify(`Failed to delete ${key} Sensor.`, "error", 2500);
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

	// Get asset categories, device models, sensor types from API and add to state
	useEffect(() => {
		(async () => {
			const assetCatData = await getAssetCategories();
			const deviceModelData = await getDeviceModels();
			const sensorTypesData = await getSensorTypes();

			setAssetCategories(assetCatData);
			setDeviceModels(deviceModelData);
			setSensorTypes(sensorTypesData);
		})();
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
			onInitNewRow={onInitNewRow}
			onRowUpdating={onRowUpdating}
		>
			<Editing mode="popup" changes={gridChanges} onChangesChange={onEditingChange} allowAdding={true} allowUpdating={true} allowDeleting={true} selectTextOnEditStart={false} startEditAction="click" useIcons={true}>
				<Popup title="Sensors" showTitle={true} width="60%" height="80%" animation={undefined} dragEnabled={false} resizeEnabled={false} />
				{SensorsForm()}
			</Editing>
			<RemoteOperations sorting={false} filtering={false} paging={false} />
			<Scrolling mode="virtual" showScrollbar="always" />
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
			<Column dataField="Name" caption="Name" width={200} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="AssetId" caption="Asset ID" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false}>
				<Lookup dataSource={assetCategories} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="DeviceId" caption="Device ID" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false}>
				<Lookup dataSource={deviceModels} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="SensorTypeId" caption="SensorType ID" width={150} allowSorting={true} allowFiltering={false} allowHeaderFiltering={true}>
				<Lookup dataSource={sensorTypes} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="IsActive" caption="Active" width={100} allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean">
				<HeaderFilter dataSource={activeHeaderFilter} />
			</Column>
			<Column dataField="IsVisible" caption="Visible" width={100} allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} dataType="boolean">
				<HeaderFilter dataSource={visibleHeaderFilter} />
			</Column>
			<Column dataField="Order" caption="Order" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />

			<Column dataField="MinValue" caption="Min Value" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="MaxValue" caption="Max Value" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />

			<Column dataField="MinNotifyValue" caption="Min Notify Value" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="MaxNotifyValue" caption="Max Notify Value" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="LastValue" caption="Last Value" width={100} allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
			<Column dataField="LastRecordedDate" caption="Last Recorded Date" width={150} allowSorting={true} allowFiltering={false} cellRender={dateRender} />
			<Column dataField="LastReceivedDate" caption="Last Received Date" width={150} allowSorting={true} allowFiltering={false} cellRender={dateRender} />
			<Column dataField="HighThreshold" caption="HighThreshold" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Column dataField="LowThreshold" caption="Low Threshold" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Column dataField="SamplingInterval" caption="Sampling Interval" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Column dataField="ReportingInterval" caption="Reporting Interval" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />
			<Column dataField="CodeErp" caption="Code ERP" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />

			<Summary>
				<TotalItem column="Id" showInColumn="assetId" displayFormat="Total: {0}" summaryType="count" />
			</Summary>
		</DataGrid>
	);
}

export default SensorsView;
