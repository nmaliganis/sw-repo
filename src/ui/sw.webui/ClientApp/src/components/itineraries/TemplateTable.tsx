// Import React hooks
import React, { useEffect, useRef, useCallback, useMemo } from "react";

// Import Redux selector
import { useSelector, useDispatch } from "react-redux";
import { setSelectedTemplate } from "../../redux/slices/itinerarySlice";

// Import Devextreme components
import notify from "devextreme/ui/notify";
import TemplateForm from "./TemplateForm";
import config from "devextreme/core/config";
import Box, { Item } from "devextreme-react/box";
import { formatDate } from "devextreme/localization";
import CustomStore from "devextreme/data/custom_store";
import SpeedDialAction from "devextreme-react/speed-dial-action";
import repaintFloatingActionButton from "devextreme/ui/speed_dial_action/repaint_floating_action_button";
import DataGrid, { Column, Lookup, ColumnChooser, Sorting, RemoteOperations, Scrolling, Summary, TotalItem } from "devextreme-react/data-grid";

// Import custom components
import TemplateToolbar from "./TemplateToolbar";

// Import custom tools
import { occurrenceType, streamType } from "../../utils/consts";
import { createTemplate, getTemplates } from "../../utils/apis/routing";
import { occurrenceRender, startEndLocationRender, streamRender, zoneContainersRender } from "../../utils/itinerariesUtils";

function TemplateTable() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedTemplate, startEndPoints, zonesByCompany } = useSelector((state: any) => state.itinerary);

	const dispatch = useDispatch();

	const templateDataGridRef = useRef<any>();

	// Objects that sets up dataSource for DataGrid
	const store = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load(loadOptions) {
					const getData = async (options = { Filter: null, SearchQuery: null }) => {
						return await getTemplates(options)
							.then((data) => {
								if (data.length) {
									return {
										data: data,
										totalCount: data.length
									};
								} else {
									notify("Failed to load templates.", "error", 3000);

									return {
										data: [],
										totalCount: 0
									};
								}
							})
							.catch(() => {
								notify("Failed to load templates.", "error", 3000);

								return {
									data: [],
									totalCount: 0
								};
							});
					};

					return getData();
				},
				insert: (values) => {
					values.StartTime = formatDate(values.StartTime, "HH:mm:ss");
					values.MinFillLevel = values.MinFillLevel * 100;

					return createTemplate(values);
				}
			}),
		[]
	);

	// Function that handles focused row selection
	const onFocusedRowChanged = useCallback(
		async ({ row }) => {
			if (row.data) {
				dispatch(setSelectedTemplate(row.data));
			}
		},
		[dispatch]
	);

	// Function that opens Create Popup form programmatically
	const addRow = () => {
		templateDataGridRef.current.instance.addRow();
		templateDataGridRef.current.instance.deselectAll();
	};
	// Function that opens Edit Popup form programmatically
	const editRow = () => {
		templateDataGridRef.current.instance.editRow(selectedTemplate);
		templateDataGridRef.current.instance.deselectAll();
	};

	// Function that deletes selected row programmatically
	const deleteRow = () => {
		templateDataGridRef.current.instance.deleteRow(selectedTemplate);
		templateDataGridRef.current.instance.deselectAll();
	};

	// Set up hover button on init
	useEffect(() => {
		config({
			floatingActionButtonConfig: {
				icon: "rowfield",
				position: {
					my: "right bottom",
					at: "right bottom",
					of: "#template-data-grid",
					offset: "-16 -16"
				}
			}
		});

		repaintFloatingActionButton();
	}, []);

	return (
		<Box className="table-background" direction="col" width="100%" height="100%">
			<Item baseSize="auto">
				<TemplateToolbar />
			</Item>
			<Item ratio={1}>
				<DataGrid
					id="template-data-grid"
					ref={templateDataGridRef}
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
					focusedRowKey={selectedTemplate?.Id}
					onFocusedRowChanged={onFocusedRowChanged}
				>
					<Sorting mode="single" />
					<Scrolling mode="virtual" rowRenderingMode="virtual" />
					<ColumnChooser enabled={true} allowSearch={true} mode="select" />
					<RemoteOperations sorting={false} filtering={false} paging={false} />
					{TemplateForm(startEndPoints)}
					<Column dataField="Name" caption={"Name"} />
					<Column dataField="StartFrom" caption={"Start Location"} cellRender={startEndLocationRender} visible={false}>
						<Lookup dataSource={startEndPoints} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="EndTo" caption={"End Location"} cellRender={startEndLocationRender} visible={false}>
						<Lookup dataSource={startEndPoints} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="Zones" caption={"Containers"} width={90} minWidth={85} alignment="right" cellRender={zoneContainersRender}>
						<Lookup dataSource={zonesByCompany} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="Description" caption={"Description"} visible={false} showInColumnChooser={false} />
					<Column dataField="Occurrence" caption={"Occurrence"} cellRender={occurrenceRender}>
						<Lookup dataSource={occurrenceType} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="StartTime" caption={"Start Time"} width={90} minWidth={85} />
					<Column dataField="Stream" caption={"Stream"} width={90} minWidth={85} cellRender={streamRender}>
						<Lookup dataSource={streamType} displayExpr="Name" valueExpr="Id" />
					</Column>
					<Column dataField="MinFillLevel" caption={"Min. Level"} width={85} minWidth={85} />
					<Summary>
						<TotalItem column="Name" displayFormat="Total: {0}" summaryType="count" />
					</Summary>
				</DataGrid>
				<SpeedDialAction icon="add" label="Add" index={1} onClick={addRow} />
				<SpeedDialAction icon="edit" label="Edit" index={2} visible={selectedTemplate !== null} onClick={editRow} />
				<SpeedDialAction icon="trash" label="Delete" index={3} visible={selectedTemplate !== null} onClick={deleteRow} />
			</Item>
		</Box>
	);
}

export default React.memo(TemplateTable);
