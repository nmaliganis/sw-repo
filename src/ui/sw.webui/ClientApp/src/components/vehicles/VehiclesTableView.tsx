import React, { useCallback, useMemo } from "react";

// Redux
import { useDispatch, useSelector } from "react-redux";
import { setSelectedVehicle, setVehiclesData } from "../../redux/slices/vehicleSlice";

import DataGrid, { Column, Lookup, Scrolling, Selection, ColumnChooser, Sorting, FilterRow, RemoteOperations, Toolbar, Item as ToolbarItem, Summary, TotalItem } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";

import { dateRender } from "../../utils/adminUtils";

import { http } from "../../utils/http";

function VehiclesTableView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);
	const { selectedVehicle } = useSelector((state: any) => state.vehicles);

	const dispatch = useDispatch();

	// Objects that sets up dataSource for DataGrid and updates Redux store
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_ASSET_HTTP + "/v1/Vehicles").then((response) => {
					dispatch(setVehiclesData(response.data.Value));

					return {
						data: response.data.Value,
						totalCount: response.data.Value.length
					};
				});
			}
		});
	}, [dispatch]);

	const onSelectionChanged = useCallback(
		({ row }: any) => {
			dispatch(setSelectedVehicle(row.data));
		},
		[dispatch]
	);

	return (
		<DataGrid
			dataSource={dataSource}
			width="100%"
			height="100%"
			showBorders={true}
			showRowLines={true}
			showColumnLines={true}
			repaintChangesOnly={true}
			rowAlternationEnabled={true}
			focusedRowEnabled={true}
			focusedRowKey={selectedVehicle?.Id}
			autoNavigateToFocusedRow={true}
			onFocusedRowChanged={onSelectionChanged}
		>
			<RemoteOperations sorting={false} filtering={false} paging={false} />
			<Scrolling mode="virtual" />
			<Sorting mode="single" />
			<Selection mode="single" selectAllMode="allPages" showCheckBoxesMode="always" />
			<FilterRow visible={true} applyFilter="Immediately" />
			<ColumnChooser enabled={true} allowSearch={true} mode="select" />
			<Toolbar>
				<ToolbarItem location="after" name="columnChooserButton" />
			</Toolbar>
			<Column dataField="NumPlate" caption="Plate Number" allowSorting={true} allowFiltering={true} />
			<Column dataField="Name" caption="Name" allowSorting={true} allowFiltering={true} />
			<Column dataField="Brand" caption="Brand" allowSorting={true} allowFiltering={true} />
			<Column dataField="RegisteredDate" caption="Registered Date" allowSorting={true} allowFiltering={true} dataType="date" cellRender={dateRender} />
			<Column dataField="Type" caption="Type" visible={false} showInColumnChooser={false} />
			<Column dataField="Status" caption="Status" visible={false} showInColumnChooser={false} />
			<Column dataField="Gas" caption="Gas" visible={false} showInColumnChooser={false} />
			<Column dataField="Height" caption="Height" visible={false} showInColumnChooser={false} />
			<Column dataField="Width" caption="Width" visible={false} showInColumnChooser={false} />
			<Column dataField="Axels" caption="Axels" visible={false} showInColumnChooser={false} />
			<Column dataField="MinTurnRadius" caption="Minimum Turn Radius" visible={false} showInColumnChooser={false} />
			<Column dataField="Length" caption="Length" visible={false} showInColumnChooser={false} />
			<Column dataField="CompanyId" caption="Company" visible={false} showInColumnChooser={false}>
				<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="AssetCategoryId" caption="Asset Category" visible={false} showInColumnChooser={false}>
				<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
			</Column>
			<Column dataField="Image" caption="Image" visible={false} showInColumnChooser={false} />
			<Column dataField="Description" caption="Description" visible={false} showInColumnChooser={false} />
			<Column dataField="CodeErp" caption="Code ERP" visible={false} showInColumnChooser={false} />
			<Summary>
				<TotalItem column="id" showInColumn="NumPlate" displayFormat="Total: {0}" summaryType="count" />
			</Summary>
		</DataGrid>
	);
}

export default VehiclesTableView;
