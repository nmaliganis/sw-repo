// Import React hooks
import React, { useState, useEffect, forwardRef, useImperativeHandle } from "react";

// Import Leaflet components
import L from "leaflet";
import "leaflet-polylinedecorator";
import { latLngBounds } from "leaflet";
import { LayersControl, MapContainer, TileLayer, useMap, ZoomControl } from "react-leaflet";
import ReactLeafletGoogleLayer from "react-leaflet-google-layer";

// Import custom tools
import { initMapCenter } from "../utils/consts";

// TODO: Set up logic to call API or extract from user data the below settings for the map
export const mapSettings = {
	MapLayers: [
		{
			type: 1,
			style: "roadmap",
			name: "Google Roadmap",
			url: "",
			apiKey: process.env.REACT_APP_GOOGLE_API_KEY,
			minZoom: 5,
			maxZoom: 18,
			defaultZoom: 10,
			defaultViewPort: [37.98381, 23.727539],
			isSelected: true
		},
		{
			type: 1,
			style: "satellite",
			name: "Google Satellite",
			url: "",
			apiKey: process.env.REACT_APP_GOOGLE_API_KEY,
			minZoom: 5,
			maxZoom: 18,
			defaultZoom: 10,
			defaultViewPort: [37.98381, 23.727539],
			isSelected: false
		},
		{
			type: 1,
			style: "terrain",
			name: "Google Terrain",
			url: "",
			apiKey: process.env.REACT_APP_GOOGLE_API_KEY,
			minZoom: 5,
			maxZoom: 18,
			defaultZoom: 10,
			defaultViewPort: [37.98381, 23.727539],
			isSelected: false
		},
		{
			type: 1,
			style: "hybrid",
			name: "Google Hybrid",
			url: "",
			apiKey: process.env.REACT_APP_GOOGLE_API_KEY,
			minZoom: 5,
			maxZoom: 18,
			defaultZoom: 10,
			defaultViewPort: [37.98381, 23.727539],
			isSelected: false
		}
		// {
		// 	type: 2,
		// 	name: "OpenStreetMap",
		// 	url: "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
		// 	attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
		// 	apiKey: "",
		// 	minZoom: 9,
		// 	maxZoom: 13,
		// 	defaultZoom: 12,
		// 	defaultViewPort: [51.805, 19.075],
		// 	isSelected: false
		// },
		// {
		// 	type: 2,
		// 	name: "Stadia",
		// 	url: "https://tiles.stadiamaps.com/tiles/osm_bright/{z}/{x}/{y}{r}.png",
		// 	attribution: '&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org/">OpenMapTiles</a> &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors',
		// 	apiKey: "",
		// 	minZoom: 5,
		// 	maxZoom: 20,
		// 	defaultZoom: 7,
		// 	defaultViewPort: [51.805, 19.075],
		// 	isSelected: false
		// },
		// {
		// 	type: 2,
		// 	name: "CyclOSM",
		// 	url: "https://{s}.tile-cyclosm.openstreetmap.fr/cyclosm/{z}/{x}/{y}.png",
		// 	attribution: '<a href="https://github.com/cyclosm/cyclosm-cartocss-style/releases" title="CyclOSM - Open Bicycle render">CyclOSM</a> | Map data: &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
		// 	apiKey: "",
		// 	minZoom: 5,
		// 	maxZoom: 20,
		// 	defaultZoom: 7,
		// 	defaultViewPort: [51.805, 19.075],
		// 	isSelected: false
		// },
		// {
		// 	type: 3,
		// 	name: "OWS-TOPO-Overlay",
		// 	url: "http://ows.mundialis.de/services/service?",
		// 	layers: "TOPO-WMS,OSM-Overlay-WMS",
		// 	attribution: "",
		// 	apiKey: "",
		// 	minZoom: 5,
		// 	maxZoom: 20,
		// 	defaultZoom: 7,
		// 	defaultViewPort: [51.805, 19.075],
		// 	isSelected: false
		// },
		// {
		// 	type: 3,
		// 	name: "OWS-TOPO",
		// 	url: "http://ows.mundialis.de/services/service?",
		// 	layers: "TOPO-WMS",
		// 	attribution: "",
		// 	apiKey: "",
		// 	minZoom: 5,
		// 	maxZoom: 20,
		// 	defaultZoom: 7,
		// 	defaultViewPort: [51.805, 19.075],
		// 	isSelected: false
		// }
	],
	MapOverlays: [
		{
			clustering: true,
			showOnStart: true,
			name: "Clustered markers",
			showPopups: true,
			overlayType: 1
		}
		// {
		// 	clustering: false,
		// 	showOnStart: true,
		// 	name: "Markers with Popups",
		// 	showPopups: true,
		// 	overlayType: 2
		// },
		// {
		// 	clustering: true,
		// 	showOnStart: false,
		// 	name: "Markers without Popups",
		// 	showPopups: false,
		// 	overlayType: 3
		// }
	]
};

// Array of objects that holds settings for arrow on polyline decorator
const arrow = [
	{
		repeat: 50,
		symbol: L.Symbol.arrowHead({
			pixelSize: 6,
			headAngle: 75,
			polygon: false,
			pathOptions: {
				stroke: true,
				weight: 3,
				color: "#FFFFFF"
			}
		})
	}
];

// Component that handles the visualization of polygon creation
export const PolylineDecorator = ({ patterns = arrow, polyline }) => {
	const map = useMap();

	useEffect(() => {
		if (!map) return;

		L.polyline(polyline, {
			weight: 9,
			color: "#35baf6"
		}).addTo(map);
		L.polylineDecorator(polyline, {
			patterns
		}).addTo(map);
	}, [map, patterns, polyline]);

	return null;
};

// Component that handles map re-rendering when prop changes
const MapInvalidateSizeHandler = ({ triggerMapInvalidateSize }) => {
	const map = useMap();

	useEffect(() => {
		map.invalidateSize();
	}, [map, triggerMapInvalidateSize]);

	return null;
};

// Component that generates map layers, clusters and markers
const LayerCreator = ({ focusedData, children }) => {
	const map = useMap();

	useEffect(() => {
		if (focusedData?.length) {
			const bounds = latLngBounds([focusedData[0]]);

			focusedData.forEach((item) => bounds.extend(item));

			map.fitBounds(bounds, {
				padding: [75, 75],
				animate: true
			});
		}
	}, [map, focusedData]);

	return (
		<LayersControl position="topright">
			{/* Layer menu set up */}
			{mapSettings.MapLayers.map((item, index) => {
				switch (item.type) {
					case 1:
						switch (item.style) {
							case "satellite":
								return (
									<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
										<ReactLeafletGoogleLayer apiKey={item.apiKey} minZoom={item.minZoom} maxZoom={item.maxZoom} type={"satellite"} />
									</LayersControl.BaseLayer>
								);
							case "terrain":
								return (
									<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
										<ReactLeafletGoogleLayer apiKey={item.apiKey} minZoom={item.minZoom} maxZoom={item.maxZoom} type={"terrain"} />
									</LayersControl.BaseLayer>
								);
							case "hybrid":
								return (
									<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
										<ReactLeafletGoogleLayer apiKey={item.apiKey} minZoom={item.minZoom} maxZoom={item.maxZoom} type={"hybrid"} />
									</LayersControl.BaseLayer>
								);
							case "roadmap":
							default:
								return (
									<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
										<ReactLeafletGoogleLayer apiKey={item.apiKey} minZoom={item.minZoom} maxZoom={item.maxZoom} type={"roadmap"} />
									</LayersControl.BaseLayer>
								);
						}

					default:
						return (
							<LayersControl.BaseLayer key={index} checked={item.isSelected} name="OpenStreetMap BlackAndWhite">
								<TileLayer attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors' url="https://tiles.wmflabs.org/bw-mapnik/{z}/{x}/{y}.png" />
							</LayersControl.BaseLayer>
						);
				}
			})}

			{children}
		</LayersControl>
	);
};

// Components that provides a map wrapper to other components and holds numerous settings
const MapProvider = forwardRef(
	(
		{
			focusedData = [],
			boxZoom = true,
			keyboard = true,
			dragging = true,
			touchZoom = true,
			trackResize = true,
			doubleClickZoom = true,
			scrollWheelZoom = true,
			style = {},
			children = <></>
		}: {
			focusedData?: [number, number][] | any[];
			boxZoom?: boolean;
			keyboard?: boolean;
			dragging?: boolean;
			touchZoom?: boolean;
			trackResize?: boolean;
			doubleClickZoom?: boolean;
			scrollWheelZoom?: boolean;
			style?: any;
			children?: React.ReactNode;
		},
		ref
	) => {
		const [triggerMapInvalidateSize, setTriggerMapInvalidateSize] = useState(true);

		// Re-render map when state changes through ref
		useImperativeHandle(ref, () => ({
			invalidateSize: () => {
				setTriggerMapInvalidateSize((state) => !state);
			}
		}));

		return (
			<div className="map-container" style={style}>
				<MapContainer
					className="map-container"
					center={JSON.parse(localStorage.getItem("dot-waste-dashboard-home") as string) ?? initMapCenter}
					zoomControl={false}
					zoom={10}
					minZoom={5}
					maxZoom={17}
					closePopupOnClick={false}
					zoomAnimationThreshold={2}
					zoomAnimation={false}
					boxZoom={boxZoom}
					keyboard={keyboard}
					dragging={dragging}
					touchZoom={touchZoom}
					trackResize={trackResize}
					doubleClickZoom={doubleClickZoom}
					scrollWheelZoom={scrollWheelZoom}
				>
					<LayerCreator focusedData={focusedData}>{children}</LayerCreator>

					<MapInvalidateSizeHandler triggerMapInvalidateSize={triggerMapInvalidateSize} />

					<ZoomControl position="bottomright" />
				</MapContainer>
			</div>
		);
	}
);

export default MapProvider;
