// Import React hooks
import React, { useState, useEffect, useMemo, useCallback, useRef } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setContainersData, setSelectedContainer, setTotals } from "../../redux/slices/containerSlice";
import { setSelectedMapItemHistory } from "../../redux/slices/modalSlice";

// Import DevExtreme components
import { TextBox, Button as TextBoxButton } from "devextreme-react/text-box";
import SelectBox, { Button as SelectBoxButton } from "devextreme-react/select-box";
import Box, { Item } from "devextreme-react/box";
import Button from "devextreme-react/button";
import DataGrid, { Column, ColumnChooser, Editing, SearchPanel, Selection, Sorting, FilterRow, Popup, RemoteOperations, Scrolling, Toolbar, Item as ToolbarItem, Lookup, HeaderFilter } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

// Import custom tools
import { http } from "../../utils/http";
import { getContainersByZone, getContainersByZoneTotal, getEventHistoryData } from "../../utils/apis/assets";
import { BucketStatus, CapacityType, MaterialType, streamType } from "../../utils/consts";
import { onInitNewRow, onRowUpdating, dateRenderer, binStatusRenderer, fillLevelRenderer, lastUpdatedRenderer, bucketStatusOps, materialTypeOps, streamTypeOps, capacityTypeOps } from "../../utils/containerUtils";

// Import custom components
import { ContainersTableToolbar } from "../../components/containers";
import ContainerTableForm, { editMandatoryPickUpRenderer, editPictureRenderer, editLocationRenderer } from "../../components/containers/ContainerTableForm";

import "../../styles/containers/ContainersTable.scss";
import "../../styles/containers/ContainerTableForm.scss";

// Object for settings on editing
const editingTexts = { addRow: "Add new Container" };

// Object for filter options on name
const nameOps = ["contains"];

// Object for filter options on fill level
const fillLevelOps = ["=", ">", "<"];

// Object for action buttons
const actionButtonOps = { type: "default", stylingMode: "contained" };

// TODO: Check logic with endpoint that supports batch editing
// Function that sends data to the server and changes the shape of each object returned from the server
async function processBatchRequest(url: string, changes: any, component: { refresh: (arg0: boolean) => any; cancelEditData: () => void }) {
	// Send data to server in any way you want
	await http
		.get(url, { params: {} })
		.then((response) => {
			let containerData = [];

			if (response.status === 200) {
				// Change shape of each object in order to handle changes inside the Form dynamically
				containerData = response.data.Value.map((item: { Latitude: any; Longitude: any; MandatoryPickupActive: any; MandatoryPickupDate: any }) => ({
					...item,
					LocationMap: { Latitude: item.Latitude, Longitude: item.Longitude },
					MandatoryPickup: { MandatoryPickupActive: item.MandatoryPickupActive, MandatoryPickupDate: item.MandatoryPickupDate }
				}));

				// dispatch(setContainersData(containerData));

				return {
					data: containerData,
					totalCount: containerData.length
				};
			}
		})
		.catch((error) => {
			throw new Error("Failed to send data");
		});

	console.log(changes);

	// Force component to refresh to call latest data and clear edit mode to remove the styles for each change
	await component.refresh(true);
	component.cancelEditData();
}

// Function that passes the url, changes, and DataGrid to processBatchRequest if there are changes. The result is assigned to the promise property of the event e
const onSaving = (e: any) => {
	// Cancel call request in CustomStore
	e.cancel = true;

	// Pass to batch request the url for the current url, changes, and the datagrid
	if (e.changes.length) {
		e.promise = processBatchRequest(process.env.REACT_APP_ASSET_HTTP + "/v1/Containers", e.changes, e.component);
	}
};

function ContainersTableView({ multiEditMode, setMultiEditMode }: { multiEditMode: boolean; setMultiEditMode: React.Dispatch<React.SetStateAction<boolean>> }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);
	const { availableZones, selectedZones, selectedFilterBinStatus, selectedStreamFilters, selectedContainer } = useSelector((state: any) => state.container);
	const { latestActivityData } = useSelector((state: any) => state.activity);

	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState<{ key: string | number; type: string; data: any }[]>([]);

	// States that hold the filters applied on the DataGrid
	const [searchType, setSearchType] = useState<number>(0);
	const [searchValue, setSearchValue] = useState("");
	const [selectedColumn, setSelectedColumn] = useState("Name");
	const [selectedColumnValue, setSelectedColumnValue] = useState(null);

	const columnValueTextRef = useRef<any>();

	const dataGridContainerRef = useRef<any>();

	const dispatch = useDispatch();

	// Function that handles the action of switching and resetting states if user wants to start/end multi edit mode
	const onMultiEditChange = useCallback(() => {
		setMultiEditMode((state) => {
			if (state) setGridChanges([]);
			return !state;
		});
	}, [setMultiEditMode]);

	// Function that clears all selected containers
	const onClickClearSelection = useCallback(() => {
		dispatch(setSelectedContainer(null));
	}, [dispatch]);

	// Object containing the dataSource that is being used for the DataGrid
	const containerDataSource = useMemo(() => {
		const getData = async (options = { Filter: null, SearchQuery: null }) => {
			return await getContainersByZone(selectedZones, options)
				.then((data) => {
					// TODO: change data handling when API is updated
					if (data.length) {
						// Filter data from selected Stream
						let containersData = data.filter((obj) => !selectedStreamFilters.includes(obj.WasteType));

						containersData = selectedFilterBinStatus.length ? containersData.filter((obj) => selectedFilterBinStatus.includes(obj.BinStatus)) : containersData;

						// Change shape of each object in order to handle changes inside the Form dynamically
						containersData = containersData.map((item: { Latitude: any; Longitude: any; MandatoryPickupActive: any; MandatoryPickupDate: any }) => ({
							...item,
							LocationMap: { Latitude: item.Latitude, Longitude: item.Longitude },
							MandatoryPickup: { MandatoryPickupActive: item.MandatoryPickupActive, MandatoryPickupDate: item.MandatoryPickupDate }
						}));

						getContainersByZoneTotal(selectedZones).then((data) => {
							dispatch(setTotals(data.Counts));
						});

						dispatch(setContainersData({ ContainersData: containersData }));

						return {
							data: containersData,
							totalCount: containersData.length
						};
					} else {
						dispatch(setContainersData({ ContainersData: [] }));

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
				let options: any = {
					Filter: searchType,
					SearchQuery: searchValue
				};

				if (loadOptions.sort) {
					options = {
						...options,
						OrderBy: loadOptions.sort[0].selector,
						SortDirection: loadOptions.sort[0].desc ? "desc" : "asc"
					};
				}

				return getData(options);
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

						dispatch(setSelectedContainer(data));
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
	}, [dispatch, searchType, searchValue, selectedZones, selectedFilterBinStatus, selectedStreamFilters]);

	// Function that handles changes on focused row selection
	const onFocusedRowChanged = useCallback(
		async (e) => {
			const selectedItem = e.row.data;

			if (selectedItem) {
				// TODO: Question business to how it should be approached.
				// dispatch(setSelectedContainer(await getSelectedContainer(selectedKey)));
				dispatch(setSelectedContainer(selectedItem));

				// Reset data on select
				dispatch(setSelectedMapItemHistory([]));

				dispatch(setSelectedMapItemHistory(await getEventHistoryData({ selectedItem: selectedItem.Id })));
			}
		},
		[dispatch]
	);

	// Function that handles column option changes
	const onColumnSelectChange = ({ value }: any) => {
		setSelectedColumn(value);
		setSelectedColumnValue(null);
	};

	// Function that handles column value changes
	const onColumnValueChange = ({ value }: any) => {
		setSelectedColumnValue(value);
	};

	// Array of objects that holds options to allow the user to change multiple columns
	const changeableMultiColumns = useMemo(
		() => [
			{ Id: "Name", Name: "Name", dataSource: [] },
			{ Id: "CompanyId", Name: "Company", dataSource: userData.UserParams.Companies },
			{ Id: "AssetCategoryId", Name: "Asset Category", dataSource: userData.UserParams.AssetCategories },
			{ Id: "Status", Name: "Status", dataSource: BucketStatus },
			{ Id: "Material", Name: "Material", dataSource: MaterialType },
			{ Id: "WasteType", Name: "Waste Type", dataSource: streamType },
			{ Id: "Capacity", Name: "Capacity", dataSource: CapacityType }
		],
		[userData.UserParams.Companies, userData.UserParams.AssetCategories]
	);

	// Object that holds the settings for apply button
	const applyButton = useMemo(
		() => ({
			type: "default",
			text: "Apply",
			onClick: () => {
				// get selected rows and set value to selected column
				const dataGridInstance = dataGridContainerRef.current._instance;
				const selectedRowKeys = dataGridInstance.getSelectedRowKeys();

				if (selectedRowKeys) {
					let changedValue: string | null = null;
					switch (selectedColumn) {
						case "Name":
							changedValue = columnValueTextRef.current._instance._changedValue;
							break;
						default:
							changedValue = selectedColumnValue;
							break;
					}

					const changedRows = selectedRowKeys.map((key: any) => {
						return { key: key, type: "update", data: { [selectedColumn]: changedValue } };
					});

					setGridChanges((currentGrid) => {
						// Pass changed items and join them with already changed items in the grid to avoid duplicates
						const data = changedRows.map((item: { key: any; data: any }) => {
							const changedItem = currentGrid.find((row: { key: any }) => row.key === item.key);

							if (changedItem) return { ...item, data: { ...changedItem.data, ...item.data } };

							return item;
						});

						return data;
					});
				}
			}
		}),
		[selectedColumn, selectedColumnValue]
	);

	// Function that sets selected items as deleted
	const setSelectedAsDeleted = useCallback(() => {
		const dataGridInstance = dataGridContainerRef.current._instance;
		const selectedRowKeys = dataGridInstance.getSelectedRowKeys();

		const deletedRows: any = selectedRowKeys.map((key: any) => {
			return { key: key, type: "remove" };
		});

		setGridChanges((currentGrid) => currentGrid.filter(({ key }) => !selectedRowKeys.includes(key)).concat(deletedRows));

		dataGridInstance.clearSelection();
	}, []);

	// Function that updates state whenever a cell changes
	const onEditingChange = useCallback(
		(changes) => {
			if (multiEditMode) setGridChanges(changes);
		},
		[multiEditMode]
	);

	// The functions setCellValueMaterial, setCellValueStream, setCellValueCapacity are updating the form's data programmatically to re-render the EditPictureForm in order to update the displaying image.
	const setCellValueMaterial = useCallback((newData, value) => {
		newData.Material = value;
	}, []);

	const setCellValueStream = useCallback((newData, value) => {
		newData.WasteType = value;
	}, []);

	const setCellValueCapacity = useCallback((newData, value) => {
		newData.Capacity = value;
	}, []);

	// TODO: Ask business team if they wish to save DataGrid settings on browser. There has been conflicts
	// // Handler for saving local settings of Grid
	// const customGridLocalSave = (state: any) => {
	// 	localStorage.setItem("localStateContainerGridStorage", JSON.stringify(state));
	// };

	// // Handler for loading local settings of Grid. Remove unwanted settings
	// const customGridLocalLoad = () => {
	// 	return new Promise((resolve) => {
	// 		let state = JSON.parse(localStorage.getItem("localStateContainerGridStorage") as string);

	// 		if (state) {
	// 			delete state.allowedPageSizes;
	// 			delete state.pageIndex;
	// 			delete state.pageSize;
	// 			delete state.selectedRowKeys;
	// 		}

	// 		resolve(state);
	// 	});
	// };

	// On latestActivityData change update the DataGrid with new data
	useEffect(() => {
		if (dataGridContainerRef.current) dataGridContainerRef.current.instance.refresh();
	}, [dataGridContainerRef, latestActivityData]);

	return (
		<Box className="table-background" direction="col" width="100%" height="100%">
			<Item baseSize="auto">
				<ContainersTableToolbar searchType={searchType} searchValue={searchValue} setSearchType={setSearchType} setSearchValue={setSearchValue} />
			</Item>
			<Item ratio={1}>
				<DataGrid
					ref={dataGridContainerRef}
					width="100%"
					height="100%"
					dataSource={containerDataSource}
					showBorders={true}
					showColumnLines={true}
					allowColumnResizing={true}
					rowAlternationEnabled={true}
					allowColumnReordering={true}
					onSaving={multiEditMode ? onSaving : undefined}
					onInitNewRow={onInitNewRow}
					onRowUpdating={onRowUpdating}
					focusedRowEnabled={!multiEditMode}
					hoverStateEnabled={true}
					autoNavigateToFocusedRow={true}
					focusedRowKey={selectedContainer?.Id}
					onFocusedRowChanged={onFocusedRowChanged}
				>
					<Toolbar>
						<ToolbarItem name="columnChooserButton" location="before" />
						<ToolbarItem location="before" visible={multiEditMode}>
							<Button icon="trash" text="Mark as deleted" stylingMode="text" onClick={setSelectedAsDeleted} />
						</ToolbarItem>
						<ToolbarItem location="before" visible={multiEditMode}>
							<SelectBox width={125} dataSource={changeableMultiColumns} valueExpr="Id" displayExpr="Name" stylingMode="underlined" labelMode="hidden" value={selectedColumn} onValueChanged={onColumnSelectChange} />
						</ToolbarItem>
						<ToolbarItem location="before" visible={multiEditMode && selectedColumn === "Name"}>
							<TextBox width={300} placeholder="..." stylingMode="underlined" labelMode="hidden" ref={columnValueTextRef}>
								<TextBoxButton name="apply" location="after" options={applyButton} />
							</TextBox>
						</ToolbarItem>
						<ToolbarItem location="before" visible={multiEditMode && selectedColumn !== "Name"}>
							<SelectBox width={230} dataSource={changeableMultiColumns.find((item) => item.Id === selectedColumn)?.dataSource} valueExpr="Id" displayExpr="Name" stylingMode="underlined" labelMode="hidden" value={selectedColumnValue} onValueChanged={onColumnValueChange}>
								<SelectBoxButton name="apply" location="after" options={applyButton} />
							</SelectBox>
						</ToolbarItem>
						<ToolbarItem location="after" visible={false}>
							<Button icon={multiEditMode ? "clear" : "edittableheader"} hint={multiEditMode ? "Cancel" : "Multi Edit"} type="default" stylingMode="contained" onClick={onMultiEditChange} />
						</ToolbarItem>
						<ToolbarItem cssClass="container-add-button" name="addRowButton" location="after" widget="dxButton" options={actionButtonOps} />
						<ToolbarItem cssClass="container-add-button" name="saveButton" location="after" visible={multiEditMode} widget="dxButton" options={actionButtonOps} />
						<ToolbarItem location="after" visible={selectedContainer !== null}>
							<Button icon="close" text="Clear Selection" type="default" stylingMode="contained" onClick={onClickClearSelection} />
						</ToolbarItem>
					</Toolbar>
					<Sorting mode="single" />
					<FilterRow visible={true} applyFilter="auto" />
					<HeaderFilter allowSearch={true} visible={true} />
					<Scrolling mode="virtual" rowRenderingMode="virtual" />
					<ColumnChooser enabled={true} allowSearch={true} mode="select" />
					<SearchPanel visible={true} width={240} placeholder="Search..." />
					<RemoteOperations sorting={false} filtering={false} paging={false} />
					<Selection mode={multiEditMode ? "multiple" : "none"} selectAllMode="allPages" showCheckBoxesMode="always" />
					{/* To avoid infinite re rendering the Editing settings are switched when user selects multi edit mode */}
					{!multiEditMode ? (
						<Editing mode="popup" allowAdding={true} allowUpdating={true} allowDeleting={false} texts={editingTexts}>
							<Popup title="Container" showTitle={true} width={1100} height={900} animation={undefined} dragEnabled={false} resizeEnabled={false} showCloseButton={true} />
							{ContainerTableForm(userData)}
						</Editing>
					) : (
						<Editing mode="batch" changes={gridChanges} onChangesChange={onEditingChange} allowAdding={true} allowUpdating={true} allowDeleting={true} texts={editingTexts} />
					)}
					<Column dataField="BinStatus" caption={"State"} allowEditing={!multiEditMode} cellRender={binStatusRenderer} width={80} alignment="center" allowFiltering={false} allowResizing={false} />
					<Column dataField="Name" caption={"Name"} filterOperations={nameOps} allowHeaderFiltering={false} selectedFilterOperation="contains" />
					<Column dataField="Level" caption={"Fill Level"} visible={!multiEditMode} allowEditing={false} allowHeaderFiltering={false} selectedFilterOperation="=" filterOperations={fillLevelOps} cellRender={fillLevelRenderer} width={120} />
					{/* <Column dataField="FillLevelPred" caption={"Predicted level"} cellRender={fillLevelPredRenderer} /> */}
					<Column dataField="LastServicedDate" caption={"Last Serviced"} allowEditing={false} cellRender={dateRenderer} width={120} allowFiltering={false} allowResizing={false} />
					<Column dataField="LastUpdated" caption={"Last Updated"} allowEditing={false} cellRender={lastUpdatedRenderer} width={130} allowFiltering={false} allowResizing={false} />
					{/* Boilerplate columns that are used only inside the Data Grid Form */}
					<Column dataField="CompanyId" caption={"Company"} visible={multiEditMode} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
						<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="AssetCategoryId" caption={"Asset Category"} visible={multiEditMode} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
						<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
					</Column>
					<Column dataField="ContainerStatus" visible={multiEditMode} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
						<Lookup dataSource={BucketStatus} valueExpr="Id" displayExpr="Name" />
						<HeaderFilter dataSource={bucketStatusOps} />
					</Column>
					<Column dataField="Material" visible={multiEditMode} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false} setCellValue={setCellValueMaterial}>
						<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
						<HeaderFilter dataSource={materialTypeOps} />
					</Column>
					<Column dataField="WasteType" visible={true} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false} setCellValue={setCellValueStream}>
						<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
						<HeaderFilter dataSource={streamTypeOps} />
					</Column>
					<Column dataField="Capacity" visible={multiEditMode} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false} setCellValue={setCellValueCapacity}>
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
			</Item>
		</Box>
	);
}

export default React.memo(ContainersTableView);
