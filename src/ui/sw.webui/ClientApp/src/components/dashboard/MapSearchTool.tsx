// Import React hooks
import React, { useState, useEffect, useCallback } from "react";

// Import Redux selectors
import { useSelector } from "react-redux";

// Import Leaflet components
import { Marker, useMap } from "react-leaflet";

// Import Devextreme components
import DataGrid, { Column, Scrolling, Sorting, Selection } from "devextreme-react/data-grid";
import { Tabs } from "devextreme-react/tabs";
import Button from "devextreme-react/button";
import Box, { Item } from "devextreme-react/box";
import { TextBox, Button as TextButton } from "devextreme-react/text-box";
import RangeSelector, { Scale, SliderMarker, Behavior, Format } from "devextreme-react/range-selector";

import { searchContainersByCriteria, searchContainersByVolume } from "../../utils/apis/assets";
import { binStatusRenderer, highlightIcon } from "../../utils/containerUtils";

import "../../styles/dashboard/MapSearchTool.scss";

// Array of objects to visualize tab options
export const tabs = [
	{
		id: 1,
		text: "Container"
	},
	// {
	// 	id: 2,
	// 	text: "Street"
	// },
	{
		id: 3,
		text: "Volume"
	}
];

const formatText = ({ valueText }: any) => {
	return `${valueText} %`;
};

function MapSearchTool() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedZones } = useSelector((state: any) => state.dashboard);

	// States that handle search options
	const [searchValue, setSearchValue] = useState("");
	const [searchVolume, setSearchVolume] = useState([1, 100]);
	// const [currentView, setCurrentView] = useState(false);

	// State that handles selected tab for search
	const [selectedIndex, setSelectedIndex] = useState(0);

	// States that handles results for search
	const [foundItemsList, setFoundItemsList] = useState<any>({ visible: true, data: [], selectedItem: null });

	const map = useMap();

	// Function that updates map and list from retrieved items
	const onSearch = useCallback(
		async (searchText) => {
			if (searchText.length > 2) {
				const foundData = await searchContainersByCriteria(searchText, selectedZones);
				// if (currentView) console.log(map.getBounds());
				setFoundItemsList((state) => ({ ...state, data: foundData, selectedItem: null }));
			}
		},
		[selectedZones]
	);

	// Function that updates search value
	const onTextValueChanged = useCallback(
		(e) => {
			setSearchValue(e.value);

			onSearch(e.value);
		},
		[onSearch]
	);

	const onSubmitSearch = useCallback(() => {
		onSearch(searchValue);
	}, [searchValue, onSearch]);

	// Function that updates date range
	const onRangeChanged = useCallback(
		async (data: any) => {
			setSearchVolume(data.value);

			const foundData = await searchContainersByVolume(data.value, selectedZones);

			setFoundItemsList((state) => ({ ...state, data: foundData, selectedItem: null }));
		},
		[selectedZones]
	);

	// Function that sets search only on current bounds
	// const onClickCurrentView = useCallback(() => {
	// 	setCurrentView((state) => !state);
	// }, []);

	// Function that handles selected tab
	const onTabsSelectionChanged = useCallback((args) => {
		if (args.name === "selectedIndex") {
			setSelectedIndex(args.value);
			setSearchValue("");
			setSearchVolume([1, 100]);
		}
	}, []);

	// Function that sets map view in accordance to selected map item
	const onFoundItemSelectionChanged = useCallback(
		({ selectedRowsData }: any) => {
			const data = selectedRowsData[0];

			setFoundItemsList((state) => ({ ...state, selectedItem: data }));
			map.setView([data.Latitude, data.Longitude], 16);
		},
		[map]
	);

	// Function that handles minimizing results
	const onClickHideButton = useCallback(() => {
		setFoundItemsList((state) => ({ ...state, visible: !state.visible }));
	}, []);

	// Reset state when search is empty
	useEffect(() => {
		if (searchValue === "") {
			setFoundItemsList((state) => ({ ...state, data: [], selectedItem: null }));
		}
	}, [searchValue, setFoundItemsList]);

	return (
		<>
			<div
				className="leaflet-top leaflet-left search-tool-container"
				style={{ pointerEvents: "auto" }}
				onMouseOver={() => {
					map.scrollWheelZoom.disable();
					map.dragging.disable();
				}}
				onMouseOut={() => {
					map.scrollWheelZoom.enable();
					map.dragging.enable();
				}}
			>
				<Box direction="row" width="100%" style={{ marginBottom: 5 }}>
					{/* <Item baseSize={30} ratio={0}>
						<Button className="view-button-container" height="100%" type={currentView ? "default" : "normal"} stylingMode="contained" focusStateEnabled={false} onClick={onClickCurrentView}>
							<i className="dx-icon-fullscreen below-icon" title="Use current view of the map"></i>
							<i className="dx-icon-map top-icon" title="Use current view of the map"></i>
						</Button>
					</Item>
					<Item baseSize={5} ratio={0}></Item> */}
					<Item ratio={1}>
						<Tabs className="search-tabs" dataSource={tabs} selectedIndex={selectedIndex} onOptionChanged={onTabsSelectionChanged} />
					</Item>
				</Box>
				{selectedIndex === 1 ? (
					<RangeSelector className="volume-range-selector" value={searchVolume} onValueChanged={onRangeChanged}>
						<Scale tickInterval={10} startValue={0} endValue={100}></Scale>
						<SliderMarker customizeText={formatText}>
							<Format type="fixedPoint" precision={0} />
						</SliderMarker>
						<Behavior snapToTicks={true} />
					</RangeSelector>
				) : (
					<TextBox placeholder="Search..." style={{ background: "#ffffff" }} value={searchValue} onValueChanged={onTextValueChanged} onEnterKey={onSubmitSearch} valueChangeEvent="keyup" stylingMode="filled" width="100%" showClearButton={true}>
						<TextButton
							name="search"
							location="before"
							options={{
								icon: "search",
								stylingMode: "filled",
								onClick: onSubmitSearch
							}}
						/>

						<TextButton name="clear" location="after" />
					</TextBox>
				)}

				{foundItemsList.data.length ? (
					<div style={{ marginTop: 5 }}>
						{foundItemsList.visible ? (
							<DataGrid className="search-tool-list" width="100%" height={600} dataSource={foundItemsList.data} keyExpr="Id" focusedRowEnabled={true} showBorders={false} showColumnHeaders={false} onSelectionChanged={onFoundItemSelectionChanged}>
								<Sorting mode="none" />
								<Selection mode="single" />
								<Scrolling mode="virtual" />
								<Column dataField="BinStatus" cellRender={binStatusRenderer} width={50} />
								<Column dataField="Name" />
							</DataGrid>
						) : (
							<></>
						)}
						<Button className="hide-button-container" width="100%" type="normal" stylingMode="contained" focusStateEnabled={false} onClick={onClickHideButton}>
							<b>{foundItemsList.visible ? "Hide" : "Show"}</b>
						</Button>
					</div>
				) : (
					<></>
				)}
			</div>
			{/* Map search tool with associated markers */}
			{/* icon={MarkerCustomIcon({ level: 0, iconSrc: icon, width: "25px", height: "41px" })} */}
			{foundItemsList?.selectedItem ? <Marker zIndexOffset={99998} interactive={false} position={[foundItemsList.selectedItem.Latitude, foundItemsList.selectedItem.Longitude]} icon={highlightIcon} /> : <></>}
		</>
	);
}

export default React.memo(MapSearchTool);
