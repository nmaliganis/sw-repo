// Import React hooks
import { useSelector } from "react-redux";

// Import Leaflet and map tools
import { Marker } from "react-leaflet";
import { MarkerCustomIcon } from "../../utils/mapUtils";
import MapProvider, { PolylineDecorator } from "../MapProvider";

function TemplateMap() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedTemplate } = useSelector((state: any) => state.itinerary);

	// Get coordinates from selected containers
	const focusedData = selectedTemplate?.AssignedContainers?.map((item) => [item?.Latitude, item?.Longitude]) || [];

	return (
		<MapProvider focusedData={focusedData}>
			{selectedTemplate ? (
				<>
					{selectedTemplate?.AssignedContainers?.map((item, index) => {
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

export default TemplateMap;
