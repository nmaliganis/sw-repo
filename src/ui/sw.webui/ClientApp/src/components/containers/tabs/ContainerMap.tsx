// Import React hooks
import React, { useEffect } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setSelectedContainer } from "../../../redux/slices/containerSlice";

// import custom tools
import { ContainerModelT } from "../../../utils/types";
import { highlightIcon } from "../../../utils/containerUtils";
import { ClusterCustomIcon, MarkerCustomIcon } from "../../../utils/mapUtils";

// Import Leaflet and map tools
import MapProvider from "../../MapProvider";
import L, { latLngBounds, LatLngExpression } from "leaflet";
import MarkerClusterGroup from "react-leaflet-markercluster";
import { LayersControl, Marker, Polygon, useMap } from "react-leaflet";

import icon from "leaflet/dist/images/marker-icon.png";
import iconShadow from "leaflet/dist/images/marker-shadow.png";

import "leaflet/dist/leaflet.css";

// Object that handles
const zoneOptions = { color: "#ff4f4f", fillColor: "#ffffff", fillOpacity: 0.25 };

// Object to set up icon in case CSS is not loading
const DefaultIcon = L.icon({
	iconUrl: icon,
	shadowUrl: iconShadow,
	iconSize: [25, 41],
	iconAnchor: [10, 41],
	popupAnchor: [2, -40]
});

L.Marker.prototype.options.icon = DefaultIcon;

const SelectedZonesPolygon = () => {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedZones } = useSelector((state: any) => state.container);

	const map = useMap();

	// Change map view to selected zone in accordance to its coordinates
	useEffect(() => {
		if (selectedZones.length) {
			const zoneCoords: [number, number][] = selectedZones.map((item) => item?.Positions || []).flat();

			if (zoneCoords.length) {
				const bounds = latLngBounds([zoneCoords[0]]);
				zoneCoords.forEach((item) => bounds.extend(item));

				map.fitBounds(bounds, {
					padding: L.point(120, 120),
					animate: true
				});
			}
		}
	}, [selectedZones, map]);

	if (selectedZones.length) return selectedZones.map((zone: { Id: React.Key | null | undefined; Positions: LatLngExpression[] | LatLngExpression[][] | LatLngExpression[][][] }) => <Polygon key={zone.Id} interactive={false} pathOptions={zoneOptions} positions={zone?.Positions || []} />);

	return null;
};

const FocusToSelectedItem = ({ selectedContainer }) => {
	const map = useMap();

	// Focus to selected container
	useEffect(() => {
		if (selectedContainer?.Id) {
			map.setView(L.latLng(selectedContainer.Latitude, selectedContainer.Longitude), 16);
		}
	}, [map, selectedContainer]);

	// Indicator on selected container
	if (selectedContainer?.Id) {
		return (
			<>
				<Marker zIndexOffset={99999} position={[selectedContainer.Latitude, selectedContainer.Longitude]} icon={MarkerCustomIcon({ binStatus: selectedContainer.BinStatus })} />
				<Marker zIndexOffset={99998} interactive={false} position={[selectedContainer.Latitude, selectedContainer.Longitude]} icon={highlightIcon} />
			</>
		);
	}

	return null;
};

function ContainerMap({ selectedContainer }: { selectedContainer?: ContainerModelT }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { containersData } = useSelector((state: any) => state.container);

	const dispatch = useDispatch();

	// Refactor dataset to be loaded correctly on map by removing selected container
	let dataset = [...containersData];

	for (let i = 0; i < dataset.length; i++) {
		if (dataset[i].Id === selectedContainer?.Id) {
			dataset.splice(i, 1);
			break;
		}
	}

	return (
		<div className="map-container-tab">
			<MapProvider>
				<SelectedZonesPolygon />
				<LayersControl.Overlay checked name="Containers">
					<FocusToSelectedItem selectedContainer={selectedContainer} />
					{selectedContainer?.Id ? (
						<></>
					) : (
						<MarkerClusterGroup key={new Date().getTime()} disableClusteringAtZoom={18} iconCreateFunction={ClusterCustomIcon} chunkedLoading={true} zoomToBoundsOnClick={true} showCoverageOnHover={false} animateAddingMarkers={false} removeOutsideVisibleBounds={true}>
							{dataset.map((item, index) => {
								const markerIcon = MarkerCustomIcon({ binStatus: item.BinStatus, iconSrc: item.Icon, width: item.Width, height: item.Height });

								const markerEventHandler = {
									click: () => {
										dispatch(setSelectedContainer(item));
									}
								};

								return <Marker key={index} position={[item.Latitude, item.Longitude]} icon={markerIcon} alt={item} eventHandlers={markerEventHandler} />;
							})}
						</MarkerClusterGroup>
					)}
				</LayersControl.Overlay>
			</MapProvider>
		</div>
	);
}

export default ContainerMap;
