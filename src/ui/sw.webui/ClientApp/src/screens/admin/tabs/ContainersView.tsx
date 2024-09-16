import { useMemo, useRef } from "react";

// Import redux selector
import { useSelector } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, ColumnChooser, Editing, SearchPanel, Sorting, FilterRow, Popup, RemoteOperations, Scrolling, Toolbar, Item as ToolbarItem, Lookup, HeaderFilter } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom components
import { ContainerTableForm } from "../../../components/containers";

// Import custom tools
import { http } from "../../../utils/http";
import { BucketStatus, CapacityType, MaterialType, streamType } from "../../../utils/consts";
import { getContainers } from "../../../utils/apis/assets";
import { binStatusRenderer, bucketStatusOps, capacityTypeOps, dateRenderer, fillLevelRenderer, lastUpdatedRenderer, materialTypeOps, onInitNewRow, onRowUpdating, streamTypeOps } from "../../../utils/containerUtils";
import { editLocationRenderer, editMandatoryPickUpRenderer, editPictureRenderer } from "../../../components/containers/ContainerTableForm";

// Object for settings on editing
const editingTexts = { addRow: "Add new Container" };

// Object for filter options on name
const nameOps = ["contains"];

// Object for filter options on fill level
const fillLevelOps = ["=", ">", "<"];

// Object for action buttons
// const actionButtonOps = { type: "default", stylingMode: "contained" };

function ContainersView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);
	const { availableZones } = useSelector((state: any) => state.container);

	const dataGridContainerRef = useRef<any>();

	// Object containing the dataSource that is being used for the DataGrid
	const containerDataSource = useMemo(() => {
		const getData = async (options = { Filter: null, SearchQuery: null }) => {
			return await getContainers(options)
				.then((data) => {
					// TODO: change data handling when API is updated
					if (data.length) {
						let containersData = [];

						// Change shape of each object in order to handle changes inside the Form dynamically
						containersData = data.map((item: { Latitude: any; Longitude: any; MandatoryPickupActive: any; MandatoryPickupDate: any }) => ({
							...item,
							LocationMap: { Latitude: item.Latitude, Longitude: item.Longitude },
							MandatoryPickup: { MandatoryPickupActive: item.MandatoryPickupActive, MandatoryPickupDate: item.MandatoryPickupDate }
						}));

						return {
							data: containersData,
							totalCount: containersData.length
						};
					} else {
						notify("Failed to load containers.", "error", 2500);

						return {
							data: [],
							totalCount: 0
						};
					}
				})
				.catch(() => {
					notify("Failed to load containers.", "error", 2500);

					return {
						data: [],
						totalCount: 0
					};
				});
		};

		return new CustomStore({
			key: "Id",
			load(loadOptions) {
				return getData();
			},
			insert: (values) => {
				const insertedContainer = {
					...values,
					isVisible: true,
					lastServicedDate: new Date(),
					mandatoryPickupActive: values.MandatoryPickup?.MandatoryPickupActive,
					mandatoryPickUpDate: values.MandatoryPickup?.MandatoryPickUpDate,
					latitude: values.LocationMap?.Latitude,
					longitude: values.LocationMap?.Longitude,
					codeErp: "1000",
					image: "img"
				};

				return http
					.post(process.env.REACT_APP_ASSET_HTTP + "/v1/Containers", insertedContainer)
					.then((response) => {
						notify(`${insertedContainer.Name} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create container.", "error", 2500);
					});
			},
			update: (key, values) => {
				const updatedContainer = {
					...values,
					mandatoryPickupActive: values.MandatoryPickup?.MandatoryPickupActive,
					mandatoryPickUpDate: values.MandatoryPickup?.MandatoryPickUpDate,
					latitude: values.LocationMap?.Latitude,
					longitude: values.LocationMap?.Longitude,
					codeErp: "1000",
					image: "img"
				};

				return http
					.put(process.env.REACT_APP_ASSET_HTTP + `/v1/Containers/${key}`, updatedContainer)
					.then(({ data }) => {
						notify(`${updatedContainer.Name} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update container.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_ASSET_HTTP + `/v1/Containers/soft/${key}`, { data: { deletedReason: "deleted from sw" } }).then((response) => {
					if (response.status === 200) {
						notify("Container was deleted successfully.", "success", 2500);
					} else {
						notify("Failed to delete container.", "error", 2500);
					}
				});
			}
		});
	}, []);

	return (
		<DataGrid
			ref={dataGridContainerRef}
			className="data-grid-common"
			width="100%"
			height="100%"
			dataSource={containerDataSource}
			showBorders={true}
			showColumnLines={true}
			allowColumnResizing={true}
			rowAlternationEnabled={true}
			allowColumnReordering={true}
			onInitNewRow={onInitNewRow}
			onRowUpdating={onRowUpdating}
			hoverStateEnabled={true}
			autoNavigateToFocusedRow={true}
		>
			<Toolbar>
				<ToolbarItem name="columnChooserButton" location="before" />
				<ToolbarItem name="addRowButton" location="after" widget="dxButton" />
			</Toolbar>
			<Sorting mode="single" />
			<FilterRow visible={true} applyFilter="auto" />
			<HeaderFilter allowSearch={true} visible={true} />
			<Scrolling mode="virtual" rowRenderingMode="virtual" />
			<ColumnChooser enabled={true} allowSearch={true} mode="select" />
			<SearchPanel visible={true} width={240} placeholder="Search..." />
			<RemoteOperations sorting={false} filtering={false} paging={false} />

			<Editing mode="popup" allowAdding={true} allowUpdating={true} allowDeleting={true} texts={editingTexts}>
				<Popup title="Container" showTitle={true} width="70%" height="95%" animation={undefined} dragEnabled={false} resizeEnabled={false} showCloseButton={true} />
				{ContainerTableForm(userData)}
			</Editing>

			<Column dataField="BinStatus" caption={"State"} cellRender={binStatusRenderer} width={80} alignment="center" allowFiltering={false} allowResizing={false} />
			<Column dataField="Name" caption={"Name"} filterOperations={nameOps} allowHeaderFiltering={false} selectedFilterOperation="contains" />
			<Column dataField="Level" caption={"Fill Level"} allowEditing={false} allowHeaderFiltering={false} selectedFilterOperation="=" filterOperations={fillLevelOps} cellRender={fillLevelRenderer} width={120} />
			{/* <Column dataField="FillLevelPred" caption={"Predicted level"} cellRender={fillLevelPredRenderer} /> */}
			<Column dataField="LastServicedDate" caption={"Last Serviced"} allowEditing={false} cellRender={dateRenderer} width={120} allowFiltering={false} allowResizing={false} />
			<Column dataField="LastUpdated" caption={"Last Updated"} allowEditing={false} cellRender={lastUpdatedRenderer} width={130} allowFiltering={false} allowResizing={false} />
			{/* Boilerplate columns that are used only inside the Data Grid Form */}
			<Column dataField="CompanyId" caption={"Company"} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="AssetCategoryId" caption={"Asset Category"} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="Status" allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={BucketStatus} valueExpr="Id" displayExpr="Name" />
				<HeaderFilter dataSource={bucketStatusOps} />
			</Column>
			<Column dataField="Material" allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
				<HeaderFilter dataSource={materialTypeOps} />
			</Column>
			<Column dataField="WasteType" allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
				<HeaderFilter dataSource={streamTypeOps} />
			</Column>
			<Column dataField="Capacity" allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={CapacityType} valueExpr="Id" displayExpr="Name" />
				<HeaderFilter dataSource={capacityTypeOps} />
			</Column>
			<Column dataField="ZoneId" visible={false} showInColumnChooser={false}>
				<Lookup dataSource={availableZones} valueExpr="Id" displayExpr="Name" />
				<HeaderFilter dataSource={availableZones} />
			</Column>
			<Column dataField="PickUpOn" visible={false} showInColumnChooser={false} />
			<Column dataField="MandatoryPickup" visible={false} showInColumnChooser={false} editCellRender={editMandatoryPickUpRenderer} />
			<Column dataField="Description" visible={false} showInColumnChooser={false} />
			<Column dataField="Image" visible={false} showInColumnChooser={false} editCellRender={editPictureRenderer} />
			<Column dataField="LocationMap" visible={false} showInColumnChooser={false} editCellRender={editLocationRenderer} />
		</DataGrid>
	);
}

export default ContainersView;
