// Import React hooks
import { useReducer, useEffect, useCallback, useMemo, useRef } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setFocusedRestriction, setRestrictionsData } from "../../redux/slices/itinerarySlice";

// Import Devextreme components
import config from "devextreme/core/config";
import Box, { Item } from "devextreme-react/box";
import { formatDate } from "devextreme/localization";
import CustomStore from "devextreme/data/custom_store";
import SpeedDialAction from "devextreme-react/speed-dial-action";
import repaintFloatingActionButton from "devextreme/ui/speed_dial_action/repaint_floating_action_button";
import DataGrid, { Column, ColumnChooser, Sorting, HeaderFilter, RemoteOperations, Scrolling } from "devextreme-react/data-grid";

// Import custom tools
import { DatePatterns } from "../../utils/consts";
import { activeHeaderFilter } from "../../utils/adminUtils";

// Import custom components
import RestrictionsForm from "./RestrictionsForm";
import { http } from "../../utils/http";
import notify from "devextreme/ui/notify";

// Array of objects for needsCleanup filter options
const needsCleanupHeaderFilter = [
	{
		text: "Needs Cleanup",
		value: ["NeedsCleanup", "=", true]
	},
	{
		text: "Optional Cleanup",
		value: ["NeedsCleanup", "=", false]
	}
];

// TODO: Remove when endpoint exists
const dataSet = [
	{
		Id: 1,
		Name: "28ης Οκτωβρίου 152-291",
		Schedule: { from: "2022-12-12 00:00", to: "2023-12-12 00:00" },
		Reason: "Πορεία",
		NeedsCleanup: true,
		Active: true,
		Positions: [
			[38.079447, 22.960739],
			[38.076204, 22.977905],
			[38.053498, 23.026657],
			[38.052957, 23.059616],
			[38.061608, 23.117294],
			[38.03349, 23.116608],
			[38.02105, 23.130341],
			[37.996704, 23.106308],
			[37.968561, 23.164673],
			[37.952861, 23.181152],
			[37.922534, 23.139267],
			[37.912242, 23.058243],
			[37.924701, 23.050003],
			[37.917659, 23.014297],
			[37.951778, 22.962112],
			[37.979928, 22.982025],
			[37.988587, 22.958679],
			[38.007525, 22.92572],
			[38.022131, 22.891388],
			[38.027, 22.850876],
			[38.052957, 22.888641],
			[38.079447, 22.960739]
		]
	},
	{
		Id: 2,
		Name: "Παν. Τσαλδάρη 24 - 112",
		Schedule: { from: "2022-10-28 08:00", to: "2022-10-28 16:00" },
		Reason: "Παρέλαση",
		NeedsCleanup: true,
		Active: true,
		Positions: [
			[37.9393239, 23.6621475],
			[37.9398654, 23.6624908],
			[37.9417607, 23.669014],
			[37.9412192, 23.6772537],
			[37.9349915, 23.6875534],
			[37.9295757, 23.6861801],
			[37.9160343, 23.7012863],
			[37.901136, 23.7177658],
			[37.8810862, 23.7246323],
			[37.8599467, 23.7503815],
			[37.8729563, 23.7730408],
			[37.8878604, 23.7781906],
			[37.9084501, 23.7771606],
			[37.9027614, 23.7689209],
			[37.9051995, 23.7572479],
			[37.931742, 23.7603378],
			[37.955568, 23.7788773],
			[37.9739737, 23.7960434],
			[37.9975154, 23.831749],
			[38.0086072, 23.8447952],
			[38.0194268, 23.8396454],
			[38.0370053, 23.8101196],
			[37.9393239, 23.6621475]
		]
	},
	{
		Id: 3,
		Name: "Ψαρών 71 - 89",
		Schedule: { from: "2022-12-12 08:00", to: "2022-12-12 16:00" },
		Reason: "Road Block",
		NeedsCleanup: false,
		Active: true,
		Positions: [
			[38.0570126, 23.7696075],
			[38.0694467, 23.7170792],
			[38.0637705, 23.7108994],
			[38.0156401, 23.6645508],
			[38.0232133, 23.6528778],
			[38.0132057, 23.6384583],
			[37.9969743, 23.656311],
			[37.9785744, 23.6357117],
			[37.9709966, 23.6212921],
			[37.9653128, 23.6123657],
			[37.9550266, 23.6120224],
			[37.9485293, 23.6130524],
			[37.9420315, 23.6178589],
			[37.9423022, 23.6405182],
			[37.9401362, 23.6322784],
			[37.9376993, 23.6274719],
			[37.9336376, 23.625412],
			[37.9290341, 23.6267853],
			[37.9282216, 23.6418915],
			[38.0570126, 23.7696075]
		]
	}
];

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

const scheduleRender = ({ value }) => {
	return `${formatDate(new Date(value.from), DatePatterns.LongDateTimeYearSmall)} - ${formatDate(new Date(value.to), DatePatterns.LongDateTimeYearSmall)}`;
};

function RestrictionsTable() {
	// State that handles restriction form
	const [formState, dispatchForm] = useReducer(formReducer, initForm);

	// Extract the relevant state from the Redux store using the useSelector hook
	const { focusedRestriction } = useSelector((state: any) => state.itinerary);

	const restrictionsDataGridRef = useRef<any>();

	const dispatch = useDispatch();

	// Function that handles selected row and updates Redux store
	const onFocusedRowChanged = useCallback(
		({ row }) => {
			if (row.data) {
				dispatch(setFocusedRestriction(row.data));
			}
		},
		[dispatch]
	);

	// Objects that sets up dataSource for DataGrid and updates Redux store
	// TODO: add logic to handle delete action
	const store = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load(loadOptions) {
					return http
						.get(process.env.REACT_APP_ROUTING_HTTP + "/v1/Restrictions")
						.then((response) => {
							if (response.status === 200) {
								dispatch(setRestrictionsData(dataSet));

								return {
									data: response.data.Value,
									totalCount: response.data.Value.length
								};
							}

							return {
								data: [],
								totalCount: 0
							};
						})
						.catch(() => {
							throw new Error("Data Loading Error");
						});
				}
			}),
		[dispatch]
	);

	// Function that saves changes to grid
	const onSave = useCallback(
		(form) => {
			// TODO: Add logic to handle action on save
			if (formState.isNewRecord) {
				http
					.post(process.env.REACT_APP_ROUTING_HTTP + "/v1/Restrictions", form.data)
					.then((response) => {
						notify(`${form.data.Name} was created successfully.`, "success", 2500);

						return response.data;
					})
					.catch(() => {
						notify("Failed to create Restriction.", "error", 2500);
					});
			} else {
				http
					.put(process.env.REACT_APP_ROUTING_HTTP + `/v1/Restrictions/${form.data.Id}`, form.data)
					.then(({ data }) => {
						notify(`${form.data.Name} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update Restriction.", "error", 2500);
					});
			}

			restrictionsDataGridRef.current.instance.refresh(true);
			dispatchForm({ type: "FINISH" });
		},
		[formState.isNewRecord]
	);

	// Function that opens popup to create new Restriction
	const addRow = () => {
		dispatchForm({ type: "ADD" });
		restrictionsDataGridRef.current.instance.deselectAll();
	};
	// Function that opens popup to edit existing Restriction
	const editRow = () => {
		dispatchForm({ type: "EDIT", data: focusedRestriction });
		restrictionsDataGridRef.current.instance.deselectAll();
	};
	// Function that deletes selected restriction
	const deleteRow = () => {
		restrictionsDataGridRef.current.instance.deselectAll();
		// TODO: Add logic to handle action on delete
	};

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

	return (
		<Box className="table-background" direction="col" width="100%" height="100%">
			<Item baseSize="auto">
				<h3>Restrictions List</h3>
				<RestrictionsForm formState={formState} dispatchForm={dispatchForm} onSave={onSave} />
			</Item>
			<Item ratio={1}>
				<DataGrid
					id="itineraries-data-grid"
					ref={restrictionsDataGridRef}
					width="100%"
					height="100%"
					dataSource={store}
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
					focusedRowKey={focusedRestriction?.Id}
					onFocusedRowChanged={onFocusedRowChanged}
				>
					<Sorting mode="single" />
					<Scrolling mode="virtual" rowRenderingMode="virtual" />
					<ColumnChooser enabled={true} allowSearch={true} mode="select" />
					<RemoteOperations sorting={false} filtering={false} paging={false} />
					<Column dataField="Name" caption={"Road Restriction"} />
					<Column dataField="Schedule" caption={"Schedule"} cellRender={scheduleRender} width={220} />
					<Column dataField="Reason" dataType="string" caption={"Reason"} />
					<Column dataField="NeedsCleanup" dataType="boolean" caption={"Needs Cleanup"} width={120}>
						<HeaderFilter dataSource={needsCleanupHeaderFilter} />
					</Column>
					<Column dataField="Active" dataType="boolean" caption={"Active"} width={80}>
						<HeaderFilter dataSource={activeHeaderFilter} />
					</Column>
				</DataGrid>
				<SpeedDialAction icon="add" label="Add" index={1} onClick={addRow} />
				<SpeedDialAction icon="edit" label="Edit" index={3} visible={focusedRestriction !== null} onClick={editRow} />
				<SpeedDialAction icon="trash" label="Delete" index={2} visible={focusedRestriction !== null} onClick={deleteRow} />
			</Item>
		</Box>
	);
}

export default RestrictionsTable;
