// Import react hooks
import { useRef, useMemo, useCallback } from "react";

// Import Devextreme components
import notify from "devextreme/ui/notify";
import CustomStore from "devextreme/data/custom_store";
import DataGrid, { Column, ColumnChooser, Selection, Sorting, FilterRow, HeaderFilter, RemoteOperations, Scrolling, Lookup, Summary, TotalItem, Toolbar, Item } from "devextreme-react/data-grid";

// Import custom components
import { GenerateDropDownButton } from "../../../utils/reportUtils";

// Import custom tools
import { streamType } from "../../../utils/consts";
import { getItineraries } from "../../../utils/apis/routing";
import { dateRenderer } from "../../../utils/containerUtils";
import { getReportDocument } from "../../../utils/apis/report";

// Array of filter options for name
const nameFilterOperations = ["contains"];

// Array of filter options for date
const dateFilterOperations = ["=", "<", ">", "<=", ">=", "between"];

// Array of objects for stream filter options
const streamHeaderFilter = [
	{
		text: "Recycle",
		value: ["Stream", "=", 1]
	},
	{
		text: "Organics",
		value: ["Stream", "=", 2]
	},
	{
		text: "Composites",
		value: ["Stream", "=", 3]
	}
];

// Function that handles rendering of start time
const startTimeRender = ({ data }) => {
	if (data?.Template?.StartTime) {
		return data.Template.StartTime;
	}

	return "";
};

// Function that handles rendering for the containers
const containersRender = ({ data }) => {
	if (data?.Jobs) {
		return data?.Jobs?.length;
	}

	return "";
};

function ReportsItinerariesForm({ isLoading, setSelectedItineraries }) {
	const itineraryDataGridRef = useRef<any>();

	// Objects that sets up dataSource for DataGrid
	const store = useMemo(
		() =>
			new CustomStore({
				key: "Id",
				load(loadOptions) {
					const getData = async (options = { Filter: null, SearchQuery: null }) => {
						return await getItineraries(options)
							.then((data) => {
								if (data.length) {
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
		[]
	);

	const onItemClick = useCallback(({ itemData }) => {
		const selectedItems = itineraryDataGridRef.current.instance.getSelectedRowKeys();

		if (selectedItems.length) {
			getReportDocument(itemData.Id, { selectedItems: selectedItems });
		} else {
			notify("No items were selected.", "error", 3000);
		}
	}, []);

	// Function that gets selected items and sends to state
	const onGenerateClick = useCallback(() => {
		const selectedItems = itineraryDataGridRef.current.instance.getSelectedRowKeys();

		if (selectedItems.length) {
			setSelectedItineraries(selectedItems);
		} else {
			notify("No items were selected.", "error", 3000);
		}
	}, [itineraryDataGridRef, setSelectedItineraries]);

	return (
		<DataGrid id="itineraries-data-grid" className="reports-data-grid" ref={itineraryDataGridRef} width="100%" height="100%" dataSource={store} showBorders={true} showColumnLines={true} hoverStateEnabled={true} allowColumnResizing={true} rowAlternationEnabled={true} allowColumnReordering={true}>
			<Toolbar>
				<Item location="after">
					<GenerateDropDownButton isLoading={isLoading} onItemClick={onItemClick} onButtonClick={onGenerateClick} />
				</Item>
				<Item name="columnChooserButton" />
			</Toolbar>
			<Sorting mode="single" />
			<FilterRow visible={true} applyFilter="auto" />
			<HeaderFilter allowSearch={true} visible={true} />
			<Scrolling mode="virtual" rowRenderingMode="virtual" />
			<ColumnChooser enabled={true} allowSearch={true} mode="select" />
			<RemoteOperations sorting={false} filtering={false} paging={false} />
			<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
			<Column dataField="Name" caption={"Itinerary"} allowHeaderFiltering={false} allowFiltering={true} filterOperations={nameFilterOperations} />
			<Column dataField="CreatedDate" caption={"Created Date"} cellRender={dateRenderer} allowHeaderFiltering={false} dataType="datetime" filterOperations={dateFilterOperations} selectedFilterOperation="between" />
			<Column dataField="StartTime" caption={"Start"} cellRender={startTimeRender} allowHeaderFiltering={false} />
			<Column dataField="Stream" caption={"Stream"} allowHeaderFiltering={true} allowFiltering={false}>
				<HeaderFilter dataSource={streamHeaderFilter} allowSearch={false} />
				<Lookup dataSource={streamType} displayExpr="Name" valueExpr="Id" />
			</Column>
			<Column dataField="Jobs" caption={"Containers"} cellRender={containersRender} alignment="right" allowHeaderFiltering={false} allowFiltering={true} />
			<Column dataField="MeanTimePerBin" caption={"Mean Time Per Bin"} visible={false} />
			<Column dataField="MeanCollectionTimePerBin" caption={"Mean Collection Time Per Bin"} visible={false} />
			<Summary>
				<TotalItem column="Name" summaryType="count" displayFormat="Total: {0}" />
			</Summary>
		</DataGrid>
	);
}

export default ReportsItinerariesForm;
