// Import React hooks
import React, { useState, useEffect, useCallback, useReducer, useMemo, useRef } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setItineraryData, setSelectedItinerary } from "../../redux/slices/itinerarySlice";

// Import Devextreme components
import Box, { Item } from "devextreme-react/box";
import DateBox from "devextreme-react/date-box";
import SpeedDialAction from "devextreme-react/speed-dial-action";
import DataGrid, { Column, ColumnChooser, Selection, Sorting, RemoteOperations, Scrolling, Lookup } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import config from "devextreme/core/config";
import notify from "devextreme/ui/notify";
import repaintFloatingActionButton from "devextreme/ui/speed_dial_action/repaint_floating_action_button";

// import custom tools
import { createItinerary, getItineraries } from "../../utils/apis/routing";
import { DatePatterns, streamType } from "../../utils/consts";

// Import custom components
import ItinerariesForm from "./ItinerariesForm";
import ItinerariesStats from "./ItinerariesStats";

import styles from "../../styles/itineraries/Itineraries.module.scss";

const initForm = { type: "", title: "", visible: false, data: null };

// Function that renders driver cell
const driverItineraryRender = ({ data }) => {
	if (data?.DriverTransportCombination?.Driver) {
		const driver = data?.DriverTransportCombination?.Driver;

		return `${driver.Firstname} ${driver.Lastname}`;
	}

	return "";
};

// Function that renders vehicle cell
const vehicleItineraryRender = ({ data }) => {
	if (data?.DriverTransportCombination?.TransportCombination?.Vehicles) {
		const vehicles = data?.DriverTransportCombination?.TransportCombination?.Vehicles.map((item) => item.Vehicle.NumPlate).join(",");

		return vehicles;
	}

	return "";
};

// Function that renders startTime cell
const startTimeRender = ({ data }) => {
	if (data?.Template?.StartTime) {
		return data.Template.StartTime;
	}

	return "";
};

// Function that renders container cell
const containersRender = ({ data }) => {
	if (data?.Jobs) {
		return data?.Jobs?.length;
	}

	return "";
};

// Function that handles state reducer
const formReducer = (state, action) => {
	switch (action.type) {
		case "ADD":
			return { type: "ADD", title: "Create Itinerary", visible: true, data: null };
		case "EDIT":
			return { type: "EDIT", title: "Edit Itinerary", visible: true, data: action.payload };
		case "CANCEL":
			return { ...initForm, type: "CANCEL" };
		case "DRAFT":
			return { ...initForm, type: "DRAFT", data: action.data };
		case "FINISH":
			return { ...initForm, type: "FINISH", data: action.data };

		default:
			throw new Error();
	}
};

function ItinerariesTable() {
	// State that handles date range
	const [dateFrom, setDateFrom] = useState(new Date().setDate(new Date().getDate() - 1));
	const [dateTo, setDateTo] = useState(new Date().getTime());

	const [selectedItineraries, setSelectedItineraries] = useState([]);

	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedItinerary, itineraryData, vehicles } = useSelector((state: any) => state.itinerary);

	const itineraryDataGridRef = useRef<any>();

	const dispatch = useDispatch();

	const [formState, dispatchForm] = useReducer(formReducer, initForm);

	// Objects that sets up dataSource for DataGrid and updates Redux store
	// TODO: add logic to handle delete action
	const store = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load(loadOptions) {
					const getData = async (options = { Filter: null, SearchQuery: null }) => {
						return await getItineraries(options)
							.then((data) => {
								if (data.length) {
									dispatch(setItineraryData(data));

									return {
										data: data,
										totalCount: data.length
									};
								} else {
									notify("Failed to load itineraries.", "error", 3000);

									return {
										data: [],
										totalCount: 0
									};
								}
							})
							.catch(() => {
								notify("Failed to load itineraries.", "error", 3000);

								return {
									data: [],
									totalCount: 0
								};
							});
					};

					return getData();
				}
			}),
		[dispatch]
	);

	// Function that handles start time range
	const onDateFromChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value).getTime();

			if (newDate < dateTo) setDateFrom(newDate);
		},
		[dateTo]
	);

	// Function that handles end time range
	const onDateToChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value).getTime();

			if (newDate > dateFrom) setDateTo(newDate);
		},
		[dateFrom]
	);

	// Function that handles itinerary selection
	const onFocusedRowChanged = useCallback(
		async ({ row }) => {
			if (row.data) {
				dispatch(setSelectedItinerary(row.data));
			}
		},
		[dispatch]
	);

	// Function that handles multi itineraries selection
	const onItinerariesSelectionChanged = useCallback(({ selectedRowKeys }) => {
		setSelectedItineraries(selectedRowKeys);
	}, []);

	const addRow = useCallback(() => {
		dispatchForm({ type: "ADD" });
	}, []);

	const editRow = useCallback(() => {
		dispatchForm({ type: "EDIT", data: selectedItinerary });
		// itineraryDataGridRef.current.instance.editRow(selectedItinerary?.Id);
		// itineraryDataGridRef.current.instance.deselectAll();
	}, [selectedItinerary]);

	const deleteRow = useCallback(() => {
		// itineraryDataGridRef.current.instance.deleteRow(selectedItinerary?.Id);
		itineraryDataGridRef.current.instance.deselectAll();
	}, []);

	// Set up hover button on init
	useEffect(() => {
		config({
			floatingActionButtonConfig: {
				icon: "rowfield",
				position: {
					my: "right bottom",
					at: "right bottom",
					of: "#itineraries-data-grid",
					offset: "-16 -16"
				}
			}
		});

		repaintFloatingActionButton();
	}, []);

	// On formState change do the following actions
	// TODO: Add logic to handle other actions
	useEffect(() => {
		switch (formState?.type) {
			case "FINISH":
				createItinerary(formState.data).then(() => {
					itineraryDataGridRef.current.instance.refresh();
				});
				break;
			case "DRAFT":
				break;
			case "CANCEL":
				break;

			default:
				break;
		}
	}, [formState]);

	return (
		<Box className="table-background" direction="col" width="100%" height="100%">
			<Item baseSize="auto">
				<div className={styles["date-range-picker"]}>
					<DateBox type="date" value={dateFrom} onValueChanged={onDateFromChanged} max={dateTo} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
					<i className="dx-icon-minus" style={{ display: "flex", alignItems: "center" }} />
					<DateBox type="date" value={dateTo} onValueChanged={onDateToChanged} min={dateFrom} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
				</div>
			</Item>
			<Item ratio={1}>
				<ItinerariesStats itineraryData={itineraryData} />
				<ItinerariesForm formState={formState} dispatchForm={dispatchForm} />
			</Item>
			<Item ratio={2}>
				<DataGrid
					id="itineraries-data-grid"
					ref={itineraryDataGridRef}
					width="100%"
					height="100%"
					dataSource={store}
					showBorders={true}
					showColumnLines={true}
					allowColumnResizing={true}
					rowAlternationEnabled={true}
					allowColumnReordering={true}
					focusedRowEnabled={true}
					hoverStateEnabled={true}
					autoNavigateToFocusedRow={false}
					focusedRowKey={selectedItinerary?.Id}
					onFocusedRowChanged={onFocusedRowChanged}
					onSelectionChanged={onItinerariesSelectionChanged}
				>
					<Sorting mode="single" />
					<Scrolling mode="virtual" rowRenderingMode="virtual" />
					<ColumnChooser enabled={true} allowSearch={true} mode="select" />
					<RemoteOperations sorting={false} filtering={false} paging={false} />
					<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
					<Column dataField="Name" caption={"Itinerary"} />
					<Column dataField="Driver" caption={"Driver"} width={180} cellRender={driverItineraryRender} />
					<Column dataField="Vehicle" caption={"Vehicle"} cellRender={vehicleItineraryRender}>
						<Lookup dataSource={vehicles} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="StartTime" caption={"Start"} cellRender={startTimeRender} />
					<Column dataField="Jobs" caption={"Containers"} cellRender={containersRender} alignment="right" />
					<Column dataField="Stream" caption={"Stream"} visible={false}>
						<Lookup dataSource={streamType} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="MeanTimePerBin" caption={"Mean Time Per Bin"} visible={false} />
					<Column dataField="MeanCollectionTimePerBin" caption={"Mean Collection Time Per Bin"} visible={false} />
				</DataGrid>

				<SpeedDialAction icon="add" label="Add" index={3} onClick={addRow} />
				<SpeedDialAction icon="edit" label="Edit" index={2} visible={selectedItineraries.length === 0 && selectedItinerary !== null} onClick={editRow} />
				<SpeedDialAction icon="trash" label="Delete" index={1} visible={selectedItineraries.length === 0 && selectedItinerary !== null} onClick={deleteRow} />
			</Item>
		</Box>
	);
}

export default React.memo(ItinerariesTable);
