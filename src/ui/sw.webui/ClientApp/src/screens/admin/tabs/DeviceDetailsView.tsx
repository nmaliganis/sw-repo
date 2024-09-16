// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import Leaflet components
import L from "leaflet";
import { MapContainer, Marker } from "react-leaflet";
import ReactLeafletGoogleLayer from "react-leaflet-google-layer";

// Create a marker icon using Leaflet's divIcon method
const markerIcon = L.divIcon({
	className: "",
	iconAnchor: [0, 24],
	popupAnchor: [13, -24],
	html: `<svg class="container-icon-state" height="25" width="25">
	<circle cx="13" cy="13" r="10" stroke="white" stroke-width="2" fill="#03a9f4" />
</svg>`
});

function DeviceDetailsView({ data }: any) {
	// Extract latitude and longitude coordinates from the "data" prop
	const deviceCoords = L.latLng(data.data.Lat, data.data.Lon);

	return (
		<Box direction="row" width="100%" height={350}>
			<Item ratio={1}>
				<h3 className="device-details-title">Selected Photos</h3>
				<div className="device-details-images-container">
					{data.data.Images?.length ? (
						data.data.Images.map((image: string, index: number) => <img className="device-details-image" key={index} src={image} alt={`Item ${index}`} />)
					) : (
						<div className="device-details-no-image">
							<i className="dx-icon-image" style={{ fontSize: "72px", color: "#585858cc" }}></i>
							<p style={{ marginBottom: 0 }}>No image(s) available</p>
						</div>
					)}
				</div>
			</Item>
			<Item baseSize="auto">
				<div className="device-details-divider" />
			</Item>
			<Item ratio={1}>
				<h3 className="device-details-title">Location</h3>
				<div className="map-container">
					<MapContainer className="map-container" center={deviceCoords} zoomControl={false} zoom={10} minZoom={10} maxZoom={10} closePopupOnClick={false} zoomAnimationThreshold={2} zoomAnimation={false}>
						<ReactLeafletGoogleLayer apiKey={process.env.REACT_APP_GOOGLE_API_KEY} type={"roadmap"} />
						<Marker icon={markerIcon} position={deviceCoords} />
					</MapContainer>
				</div>
			</Item>
		</Box>
	);
}

export default DeviceDetailsView;
