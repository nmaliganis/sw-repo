import React, { useEffect } from "react";

// Redux
import { useSelector, useDispatch } from "react-redux";
import { setSelectedVehicle } from "../../redux/slices/vehicleSlice";

import L from "leaflet";
import { MapContainer, TileLayer, Marker, LayersControl, useMapEvents, useMap, ZoomControl } from "react-leaflet";
import ReactLeafletGoogleLayer from "react-leaflet-google-layer";
import { mapSettings } from "../MapProvider";

import icon from "leaflet/dist/images/marker-icon.png";
import iconShadow from "leaflet/dist/images/marker-shadow.png";

import VehicleMapRoutingMachine from "./VehicleMapRoutingMachine";

const DefaultIcon = L.icon({
	iconUrl: icon,
	shadowUrl: iconShadow,
	iconSize: [25, 41],
	iconAnchor: [10, 41],
	popupAnchor: [2, -40]
});

L.Marker.prototype.options.icon = DefaultIcon;

const mapData = [
	{
		Id: 1,
		Latitude: 38.07836562996712,
		Longitude: 23.69164002388398
	},
	{
		Id: 2,
		Latitude: 38.05836562996712,
		Longitude: 23.59164002388398
	}
];

function LayerCreator() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { vehiclesData, selectedVehicle } = useSelector((state: any) => state.vehicles);

	const dispatch = useDispatch();

	const map = useMap();

	useMapEvents({
		baselayerchange: (e) => {
			const getLayer: any = mapSettings.MapLayers.find((layer: { name: string }) => layer.name === e.name);

			//On tile layer change pan to the default center
			if (getLayer) map.setView(getLayer?.defaultViewPort, getLayer?.defaultZoom);
		}
	});

	useEffect(() => {
		if (mapSettings.MapLayers?.length) {
			let latLng = L.latLng(mapSettings?.MapLayers[0].defaultViewPort[0], mapSettings?.MapLayers[0].defaultViewPort[1]);
			map.setView(latLng, mapSettings.MapLayers[0].defaultZoom);
		}
	}, [map]);

	if (mapSettings.MapLayers?.length)
		return (
			<>
				<LayersControl position="topright">
					{/* Layer menu set up */}
					{mapSettings.MapLayers.map((item, index) => {
						switch (item.type) {
							case 1:
								return (
									<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
										<ReactLeafletGoogleLayer key={item.type} apiKey={item.apiKey} minZoom={item.minZoom} maxZoom={item.maxZoom} type={"roadmap"} />
									</LayersControl.BaseLayer>
								);
							// case 2:
							// 	return (
							// 		<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
							// 			<TileLayer key={item.type} attribution={item.attribution} url={item.url} minZoom={item.minZoom} maxZoom={item.maxZoom} />
							// 		</LayersControl.BaseLayer>
							// 	);
							// case 3:
							// 	return (
							// 		<LayersControl.BaseLayer key={index} checked={item.isSelected} name={item.name}>
							// 			<WMSTileLayer key={item.type} url={item.url} layers={item.layers} minZoom={item.minZoom} maxZoom={item.maxZoom} />
							// 		</LayersControl.BaseLayer>
							// 	);
							default:
								return (
									<LayersControl.BaseLayer key={index} checked={item.isSelected} name="OpenStreetMap BlackAndWhite">
										<TileLayer attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors' url="https://tiles.wmflabs.org/bw-mapnik/{z}/{x}/{y}.png" />
									</LayersControl.BaseLayer>
								);
						}
					})}
					{vehiclesData.map((mapItem: any, index: React.Key | number) => {
						const markerIcon = L.divIcon({
							className: "",
							iconAnchor: [0, 24],
							popupAnchor: [13, -24],
							html: `<img src="https://www.freeiconspng.com/uploads/truck-icon-31.png" style="width: 30px; height: 24px;" />`
						});

						return (
							<Marker
								key={index}
								position={[mapData[index].Latitude, mapData[index].Longitude]}
								icon={markerIcon}
								alt={mapItem}
								eventHandlers={{
									click: (e) => {
										dispatch(setSelectedVehicle(mapItem));
									}
								}}
							/>
						);
					})}
				</LayersControl>
				<VehicleMapRoutingMachine selectedVehicle={selectedVehicle} />
			</>
		);
	else return <TileLayer url="https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.png" />;
}

function VehiclesMapView() {
	return (
		<div className="map-container">
			<MapContainer
				className="map-container"
				center={[37.98381, 23.727539]}
				// maxBounds={[
				// 	[38.1334764, 23.5663605],
				// 	[37.7967632, 23.9316559]
				// ]}
				zoomControl={false}
				zoom={10}
				minZoom={5}
				maxZoom={40}
				closePopupOnClick={true}
				zoomAnimationThreshold={2}
			>
				<LayerCreator />
				<ZoomControl position="bottomright" />
			</MapContainer>
		</div>
	);
}

export default VehiclesMapView;
