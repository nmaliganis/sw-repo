// Import React hooks
import { useState, useEffect, useCallback, useMemo, useRef } from "react";

// Import Devextreme components
import notify from "devextreme/ui/notify";
import TagBox from "devextreme-react/tag-box";
import DateBox from "devextreme-react/date-box";
import Box, { Item } from "devextreme-react/box";
import NumberBox from "devextreme-react/number-box";
import RadioGroup from "devextreme-react/radio-group";
import CustomStore from "devextreme/data/custom_store";
import DataGrid, { Column, ColumnChooser, Selection, Sorting, FilterRow, HeaderFilter, RemoteOperations, Scrolling, Lookup, Summary, TotalItem, Toolbar, Item as ToolbarItem, SearchPanel } from "devextreme-react/data-grid";

// Import custom components
import { GenerateDropDownButton } from "../../../utils/reportUtils";
import LoadingPage from "../../../utils/LoadingPage";

// Import custom tools
import { getContainersByZone, getZonesByCompany } from "../../../utils/apis/assets";
import { CapacityType, DatePatterns, MaterialType, streamType } from "../../../utils/consts";
import { capacityTypeOps, lastUpdatedRenderer, materialTypeOps, streamTypeOps } from "../../../utils/containerUtils";
import { getReportDocument } from "../../../utils/apis/report";

// Array of filter options for name
const nameOps = ["contains"];

// Array of objects for time results
const timeResultsEntities = [
	{ Id: 1, Name: "Hour" },
	{ Id: 2, Name: "Day" }
];

function ReportsWasteForm({ isLoading, setWasteParams }) {
	// State that handles form values
	const [formData, setFormData] = useState({
		dateFrom: new Date().setDate(new Date().getDate() - 5),
		dateTo: new Date().getTime(),
		fillLevelLimit: 5,
		selectedTimeResult: 1,
		selectedContainers: []
	});

	// State to load and handle zones
	const [selectedZones, setSelectedZones] = useState([]);
	const [availableZones, setAvailableZones] = useState([]);

	const wasteDataGridRef = useRef<any>();

	// Objects that sets up dataSource for DataGrid
	const store = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load(loadOptions) {
					const getData = async (options = { Filter: null, SearchQuery: null }) => {
						if (selectedZones.length) {
							return await getContainersByZone(selectedZones, options)
								.then((data) => {
									return {
										data: data,
										totalCount: data.length
									};
								})
								.catch(() => {
									notify("Failed to load containers.", "error", 3000);

									return {
										data: [],
										totalCount: 0
									};
								});
						}
						return {
							data: [],
							totalCount: 0
						};
					};

					return getData();
				}
			}),
		[selectedZones]
	);

	// Function that handles start time range
	const onDateFromChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value).getTime();

			if (newDate < formData.dateTo) setFormData((state) => ({ ...state, dateFrom: newDate }));
		},
		[formData.dateTo]
	);

	// Function that handles end time range
	const onDateToChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value).getTime();

			if (newDate > formData.dateFrom) setFormData((state) => ({ ...state, dateTo: newDate }));
		},
		[formData.dateFrom]
	);

	// Function that updates selected zone on form
	const onZoneSelectionChange = useCallback((e) => {
		setSelectedZones(e.value);
	}, []);

	// Function that updates selected fill limit on form
	const onFillLimitChanged = useCallback((e) => {
		setFormData((state) => ({ ...state, fillLevelLimit: e.value }));
	}, []);

	// Function that updates selected time result on form
	const onTimeResultsValueChanged = useCallback((e) => {
		setFormData((state) => ({ ...state, selectedTimeResult: e.value }));
	}, []);

	// Validation function that checks user input
	const validationEngine = useCallback((form, selectedItems) => {
		if (form.selectedZones.length === 0) {
			notify("No selected Zones.", "error", 3000);
			return false;
		} else if (form.selectedTimeResult === 0) {
			notify("Missing Time results frequency.", "error", 3000);
			return false;
		} else if (selectedItems.length === 0) {
			notify("No containers were selected.", "error", 3000);
			return false;
		}

		return true;
	}, []);

	const onExportClick = useCallback(
		({ itemData }) => {
			const selectedItems = wasteDataGridRef.current.instance.getSelectedRowKeys();

			if (validationEngine(formData, selectedItems)) {
				getReportDocument(itemData.Id, { ...formData, selectedItems: selectedItems });
			}
		},
		[validationEngine, formData]
	);

	// Function that handles form submission
	const onFormSubmit = useCallback(() => {
		const selectedItems = wasteDataGridRef.current.instance.getSelectedRowKeys();

		if (validationEngine(formData, selectedItems)) {
			setWasteParams({
				...formData,
				selectedContainers: selectedItems
			});
		}
	}, [validationEngine, wasteDataGridRef, formData, setWasteParams]);

	// Get Zones and update state
	useEffect(() => {
		(async () => {
			const data = await getZonesByCompany();

			setAvailableZones(data);
			setSelectedZones([data[0] as never]);
		})();
	}, []);

	if (availableZones.length === 0) return <LoadingPage />;

	return (
		<Box direction="col" width="100%" height="100%">
			<Item baseSize="auto">
				<div className="vertical-center-content" style={{ gap: 5 }}>
					Results per:
					<RadioGroup id="radio-group-with-selection" items={timeResultsEntities} value={formData.selectedTimeResult} valueExpr="Id" displayExpr="Name" onValueChanged={onTimeResultsValueChanged} layout="horizontal" />
				</div>
			</Item>
			<Item ratio={1}>
				<DataGrid ref={wasteDataGridRef} width="100%" height="100%" dataSource={store} showBorders={true} showColumnLines={true} hoverStateEnabled={true} allowColumnResizing={true} rowAlternationEnabled={true} allowColumnReordering={true}>
					<Toolbar>
						<ToolbarItem location="before">
							<div className="date-range-picker" style={{ paddingBottom: 5 }}>
								<DateBox type="date" width={120} value={formData.dateFrom} onValueChanged={onDateFromChanged} max={formData.dateTo} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
								<i className="dx-icon-minus" style={{ display: "flex", alignItems: "center" }} />
								<DateBox type="date" width={120} value={formData.dateTo} onValueChanged={onDateToChanged} min={formData.dateFrom} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
							</div>
						</ToolbarItem>
						<ToolbarItem location="before">
							<TagBox
								placeholder="Select Zones..."
								dataSource={availableZones}
								width={150}
								valueExpr="Id"
								displayExpr="Name"
								applyValueMode="useButtons"
								value={selectedZones}
								onValueChanged={onZoneSelectionChange}
								maxFilterQueryLength={900000}
								multiline={false}
								searchEnabled={true}
								showSelectionControls={true}
							/>
						</ToolbarItem>
						<ToolbarItem location="before">Fill limit:</ToolbarItem>
						<ToolbarItem location="before">
							<NumberBox width={60} format="# L" min={0} value={formData.fillLevelLimit} onValueChanged={onFillLimitChanged} />
						</ToolbarItem>

						<ToolbarItem location="after">
							<GenerateDropDownButton isLoading={isLoading} onItemClick={onExportClick} onButtonClick={onFormSubmit} />
						</ToolbarItem>
						<ToolbarItem name="columnChooserButton" />
					</Toolbar>
					<Sorting mode="single" />
					<FilterRow visible={true} applyFilter="auto" />
					<HeaderFilter allowSearch={true} visible={true} />
					<Scrolling mode="virtual" rowRenderingMode="virtual" />
					<ColumnChooser enabled={true} allowSearch={true} mode="select" />
					<SearchPanel visible={true} width={240} placeholder="Search..." />
					<RemoteOperations sorting={false} filtering={false} paging={false} />
					<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
					<Column dataField="Name" caption={"Name"} width={120} filterOperations={nameOps} allowHeaderFiltering={false} selectedFilterOperation="contains" />
					<Column dataField="LastUpdated" caption={"Last Updated"} width={130} allowEditing={false} cellRender={lastUpdatedRenderer} allowHeaderFiltering={false} allowFiltering={false} allowResizing={false} dataType="datetime" />
					<Column dataField="Material" width={100} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
						<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
						<HeaderFilter dataSource={materialTypeOps} />
					</Column>
					<Column dataField="WasteType" caption="Stream" width={120} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
						<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
						<HeaderFilter dataSource={streamTypeOps} />
					</Column>
					<Column dataField="Capacity" width={100} allowHeaderFiltering={true} allowFiltering={false} showInColumnChooser={false}>
						<Lookup dataSource={CapacityType} valueExpr="Id" displayExpr="Name" />
						<HeaderFilter dataSource={capacityTypeOps} />
					</Column>
					<Summary>
						<TotalItem column="Name" summaryType="count" displayFormat="Total: {0}" />
					</Summary>
				</DataGrid>
			</Item>
		</Box>
	);
}

export default ReportsWasteForm;
