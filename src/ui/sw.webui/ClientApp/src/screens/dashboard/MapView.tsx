import React from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import Leaflet and custom map components
import MarkerClusterGroup from "react-leaflet-markercluster";
import { LayersControl } from "react-leaflet";
import MapProvider from "../../components/MapProvider";
import { ClusterCustomIcon } from "../../utils/mapUtils";
import { ContainerMarker, MapHomeTool, MapMultiSelectTool, MapSearchTool, MapZoneTool } from "../../components/dashboard";

import "leaflet/dist/leaflet.css";
import "../../styles/dashboard/Map.scss";

function MapView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { mapData, mapDataFilter, selectedStreamFilters } = useSelector((state: any) => state.dashboard);

	// Filter data in accordance of the selected Stream and Bin Status
	const data = mapData?.filter((item: { WasteType: number }) => !selectedStreamFilters.includes(item.WasteType)).filter((item: { BinStatus: number }) => !mapDataFilter.includes(item.BinStatus));

	return (
		<MapProvider>
			<LayersControl.Overlay checked name="Containers">
				<MarkerClusterGroup disableClusteringAtZoom={16} iconCreateFunction={ClusterCustomIcon} chunkedLoading={true} zoomToBoundsOnClick={true} showCoverageOnHover={false} animateAddingMarkers={false} removeOutsideVisibleBounds={true}>
					{data?.map((mapItem: any) => {
						return <ContainerMarker key={mapItem.Id} mapItem={mapItem} />;
					})}
				</MarkerClusterGroup>
			</LayersControl.Overlay>
			<MapMultiSelectTool mapData={data} />
			<MapSearchTool />
			<MapZoneTool />
			<MapHomeTool />
		</MapProvider>
	);
}

export default React.memo(MapView);
