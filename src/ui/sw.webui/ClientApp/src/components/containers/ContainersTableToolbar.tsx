// Import React hooks
import React, { useState, useEffect, Dispatch, SetStateAction, useCallback } from "react";

// Import Redux action creators
import { useSelector, useDispatch } from "react-redux";
import { setAvailableZones, setSelectedFilterBinStatus, setSelectedZones, addSelectedStreamFilters, removeSelectedStreamFilters } from "../../redux/slices/containerSlice";

// Import Devextreme components
import TagBox from "devextreme-react/tag-box";
import Popover from "devextreme-react/popover";
import TextBox from "devextreme-react/text-box";
import Box, { Item } from "devextreme-react/box";
import SelectBox from "devextreme-react/select-box";
import { ButtonGroup } from "devextreme-react/button-group";
import PieChart, { Series, Legend, Animation } from "devextreme-react/pie-chart";

// import custom tools
import { getZonesByCompany } from "../../utils/apis/assets";
import { SummaryPieChart } from "../../utils/dashboardUtils";
import { binStatusRenderer } from "../../utils/containerUtils";
import { colorPalette, searchFilterData } from "../../utils/consts";
import ContainersTotalIndicators from "./ContainersTotalIndicators";

const filterItems = [
	{
		value: 0,
		hint: "Offline",
		Total: 0
	},
	{
		value: 1,
		hint: "Empty",
		Total: 0
	},
	{
		value: 2,
		hint: "Normal",
		Total: 0
	},
	{
		value: 3,
		hint: "Full",
		Total: 0
	},
	{
		value: 4,
		hint: "No Device",
		Total: 0
	}
];

function ContainersTableToolbar({ searchType, searchValue, setSearchType, setSearchValue }: { searchType: number; searchValue: string; setSearchType: Dispatch<SetStateAction<number>>; setSearchValue: Dispatch<SetStateAction<string>> }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { containersData, totalTrash, totalRecycle, totalCompost, availableZones, selectedZones, selectedFilterBinStatus } = useSelector((state: any) => state.container);

	const [popoverVisible, setPopoverVisible] = useState(false);

	const dispatch = useDispatch();

	const togglePopover = () => {
		setPopoverVisible((state) => !state);
	};

	const onZoneSelectionChange = useCallback(
		({ value }: any) => {
			dispatch(setSelectedZones(value));
		},
		[dispatch]
	);

	const searchFilter = useCallback(
		(value) => {
			setSearchType(value);
		},
		[setSearchType]
	);

	const searchValueChanged = useCallback(
		(data) => {
			setSearchValue(data.value);
		},
		[setSearchValue]
	);

	const onFilterBinStatusChanged = useCallback(
		(e) => {
			if (e.name === "selectedItemKeys") dispatch(setSelectedFilterBinStatus(e.value));
		},
		[dispatch]
	);

	// Array of objects containing unique colors and the number of occurrences for each
	const typeOccurrenceArray = colorPalette.map((item) => {
		return {
			Color: item.Color,
			BinStatus: item.BinStatus,
			Name: item.Name,
			Total: containersData.map((mapItem: { BinStatus: number }) => mapItem.BinStatus)?.filter((v: number) => v === item.BinStatus).length
		};
	});

	// Load localStorage settings
	useEffect(() => {
		const containersCustomFilters = JSON.parse(localStorage.getItem("containersCustomFilters") as string);
		if (containersCustomFilters) {
			dispatch(setSelectedFilterBinStatus(containersCustomFilters?.selectedFilterBinStatus));
			dispatch(setSelectedZones(containersCustomFilters?.selectedZones));
			setSearchType(containersCustomFilters?.searchType);
			setSearchValue(containersCustomFilters?.searchValue);
		}
	}, [dispatch, setSearchType, setSearchValue]);

	// Save localStorage settings
	useEffect(() => {
		const containersCustomFilters = {
			selectedFilterBinStatus: selectedFilterBinStatus,
			selectedZones: selectedZones,
			searchType: searchType,
			searchValue: searchValue
		};

		localStorage.setItem("containersCustomFilters", JSON.stringify(containersCustomFilters));
	}, [selectedFilterBinStatus, selectedZones, searchType, searchValue]);

	useEffect(() => {
		(async () => {
			const data = await getZonesByCompany();

			if (data.length) {
				dispatch(setAvailableZones(data));
				dispatch(setSelectedZones([data[0] as never]));
			}
		})();
	}, [dispatch]);

	return (
		<>
			<Box direction="row" width="100%" height={100} style={{ padding: "20px 5px", gap: 10 }}>
				<Item ratio={1}>
					<ContainersTotalIndicators totalTrash={totalTrash} totalRecycle={totalRecycle} totalCompost={totalCompost} addStreamFilters={addSelectedStreamFilters} removeStreamFilters={removeSelectedStreamFilters} />
				</Item>
				<Item baseSize={190}>
					<PieChart className="pie-stats pie-stats-helper" id="pieInfo" type="pie" onPointHoverChanged={togglePopover} dataSource={typeOccurrenceArray} redrawOnResize={true} palette={typeOccurrenceArray.map((item) => item.Color)}>
						<Animation enabled={true} />
						<Series argumentField="Name" valueField="Total"></Series>
						<Legend visible={false}></Legend>
					</PieChart>

					<Popover target="#pieInfo" position="bottom" visible={popoverVisible}>
						<SummaryPieChart data={containersData} />
					</Popover>
				</Item>
				<Item baseSize="auto">
					<div className="vertical-center-content-right" style={{ height: "100%" }}>
						<div className="toolbar-label">LEVEL</div>
						<ButtonGroup className="level-filter-container" items={filterItems} buttonRender={binStatusRenderer} keyExpr="value" stylingMode="text" selectionMode="multiple" selectedItemKeys={selectedFilterBinStatus} onOptionChanged={onFilterBinStatusChanged} />
					</div>
				</Item>
			</Box>
			<Box direction="row" width="50%">
				<Item ratio={1}>
					<TagBox
						style={{ marginRight: 5 }}
						dataSource={availableZones}
						value={selectedZones}
						onValueChanged={onZoneSelectionChange}
						displayExpr="Name"
						placeholder="Select Zones..."
						maxFilterQueryLength={900000}
						searchEnabled={true}
						showSelectionControls={true}
						multiline={false}
						applyValueMode="useButtons"
					/>
				</Item>
				<Item ratio={1} visible={false}>
					<SelectBox dataSource={searchFilterData} displayExpr="Name" valueExpr="Id" value={searchType} onValueChange={searchFilter} />
				</Item>
				<Item ratio={2} visible={false}>
					<TextBox placeholder="Search..." showClearButton={true} valueChangeEvent="keyup" value={searchValue} onValueChanged={searchValueChanged} />
				</Item>
			</Box>
		</>
	);
}

export default React.memo(ContainersTableToolbar);
