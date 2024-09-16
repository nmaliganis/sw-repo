// Import React hooks
import { useSelector } from "react-redux";

// Import Leaflet and map tools
import L from "leaflet";
import { Marker } from "react-leaflet";
import { MarkerCustomIcon } from "../../utils/mapUtils";
import MapProvider, { PolylineDecorator } from "../MapProvider";
import { LeafletTrackingMarker } from "react-leaflet-tracking-marker";

const icon = L.icon({
	iconUrl: "https://icons.iconarchive.com/icons/icons-land/transporter/256/Truck-Top-Green-icon.png",
	iconAnchor: [12, 12],
	iconSize: [24, 24]
});

const VehicleMarker = ({ vehicle }) => {
	return <LeafletTrackingMarker zIndexOffset={99999} icon={icon} position={[vehicle.Latitude, vehicle.Longitude]} rotationAngle={vehicle.Rotation} duration={200} />;
};

// TODO: Handle logic in accordance to data endpoint
function ItinerariesMap() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedItinerary } = useSelector((state: any) => state.itinerary);

	// Get coordinates from selected containers
	const focusedData = selectedItinerary?.AssignedContainers?.map((item) => [item?.Latitude, item?.Longitude]) || [];

	return (
		<MapProvider focusedData={focusedData}>
			{selectedItinerary ? (
				<>
					{selectedItinerary?.Vehicle ? <VehicleMarker vehicle={selectedItinerary?.Vehicle} /> : <></>}
					{selectedItinerary?.AssignedContainers?.map((item, index) => {
						const markerIcon = MarkerCustomIcon({ binStatus: item.BinStatus, iconSrc: item.Icon, width: item.Width, height: item.Height });

						return <Marker key={index} position={[item.Latitude, item.Longitude]} icon={markerIcon} alt={item} />;
					})}

					<PolylineDecorator polyline={focusedData} />
				</>
			) : (
				<></>
			)}
		</MapProvider>
	);
}

export default ItinerariesMap;
