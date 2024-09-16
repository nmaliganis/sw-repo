import { useMemo, useRef } from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, ColumnChooser, Editing, Sorting, FilterRow, Popup, RemoteOperations, Scrolling, Toolbar, Item as ToolbarItem, Lookup, HeaderFilter } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom components
import { DriversForm } from "../../../components/admin";

// Import custom tools
import { http } from "../../../utils/http";
import { dateRenderer } from "../../../utils/containerUtils";

// Object for filter options on first, last name
const nameOps = ["contains"];

const vehicleRenderer = ({ value }) => {
	return (
		<DataGrid width="100%" height={150} keyExpr="NumPlate" dataSource={value} showBorders={true} showColumnLines={true}>
			<Column dataField="NumPlate" />
			<Column dataField="Name" />
			<Column dataField="Brand" />
			<Column dataField="RegisteredDate" />
			<Column dataField="Type" />
			<Column dataField="Gas" />
			<Column dataField="Width" />
			<Column dataField="Height" />
			<Column dataField="Axels" />
			<Column dataField="MinTurnRadius" />
			<Column dataField="Length" />
		</DataGrid>
	);
};

function DriversView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	const driversDataGridRef = useRef<any>();

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		const getData = async (options = { Filter: null, SearchQuery: null }) => {
			return await http
				.get(process.env.REACT_APP_ROUTING_HTTP + "/v1/Drivers")
				.then(({ data }) => {
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
						notify("Failed to load Drivers.", "error", 2500);

						return {
							data: [],
							totalCount: 0
						};
					}
				})
				.catch(() => {
					notify("Failed to load Drivers.", "error", 2500);

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
				return http
					.post(process.env.REACT_APP_ROUTING_HTTP + "/v1/Drivers", values)
					.then((response) => {
						notify(`${values.LastName} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create Driver.", "error", 2500);
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
					.put(process.env.REACT_APP_ROUTING_HTTP + `/v1/Drivers/${key}`, updatedContainer)
					.then(({ data }) => {
						notify(`${updatedContainer.Name} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update Driver.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_ROUTING_HTTP + `/v1/Drivers/soft/${key}`, { data: { deletedReason: "deleted from sw" } }).then((response) => {
					if (response.status === 200) {
						notify("Driver was deleted successfully.", "success", 2500);
					} else {
						notify("Failed to delete Driver.", "error", 2500);
					}
				});
			}
		});
	}, []);

	return (
		<DataGrid
			ref={driversDataGridRef}
			className="data-grid-common"
			width="100%"
			height="100%"
			dataSource={dataSource}
			showBorders={true}
			showColumnLines={true}
			allowColumnResizing={true}
			rowAlternationEnabled={true}
			allowColumnReordering={true}
			hoverStateEnabled={true}
			autoNavigateToFocusedRow={true}
		>
			<Toolbar>
				<ToolbarItem name="columnChooserButton" location="before" />
				<ToolbarItem name="addRowButton" location="after" widget="dxButton" />
			</Toolbar>
			<Editing mode="popup" allowAdding={true} allowUpdating={true} allowDeleting={true}>
				<Popup title="Driver" showTitle={true} width="60%" height="65%" animation={undefined} dragEnabled={false} resizeEnabled={false} showCloseButton={true} />
				{DriversForm(userData)}
			</Editing>
			<Sorting mode="single" />
			<FilterRow visible={true} applyFilter="auto" />
			<HeaderFilter allowSearch={true} visible={true} />
			<Scrolling mode="virtual" rowRenderingMode="virtual" />
			<ColumnChooser enabled={true} allowSearch={true} mode="select" />
			<RemoteOperations sorting={false} filtering={false} paging={false} />
			<Column dataField="FirstName" caption={"First Name"} filterOperations={nameOps} allowHeaderFiltering={false} selectedFilterOperation="contains" />
			<Column dataField="LastName" caption={"Last Name"} filterOperations={nameOps} allowHeaderFiltering={false} selectedFilterOperation="contains" />
			<Column dataField="RegisteredAt" caption={"Registered At"} allowEditing={false} cellRender={dateRenderer} allowFiltering={false} allowResizing={false} />
			<Column dataField="CompanyId" caption={"Company"} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
				<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="AssignedVehicles" caption={"Assigned Vehicles"} cellRender={vehicleRenderer} allowFiltering={false} allowResizing={false} />
			<Column dataField="Description" visible={false} showInColumnChooser={false} />
			<Column dataField="Image" visible={false} showInColumnChooser={false} />
		</DataGrid>
	);
}

export default DriversView;
