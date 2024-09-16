// Import React hooks
import { useSelector } from "react-redux";

// Import Leaflet and map tools
import { Polygon } from "react-leaflet";
import MapProvider from "../MapProvider";

function RestrictionsMap() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { restrictionsData, focusedRestriction } = useSelector((state: any) => state.itinerary);

	// Get coordinates from selected restriction or from all restrictionData
	const focusedData: [number, number][] = focusedRestriction?.Positions ? focusedRestriction?.Positions : restrictionsData.map((item: { Positions: any }) => item?.Positions || []).flat();

	return (
		<MapProvider focusedData={focusedData}>
			<>
				{restrictionsData?.map((item) => (
					<Polygon key={item.Id} interactive={false} pathOptions={{ color: "#ff4f4f", fillColor: "#ff4f4f", fillOpacity: 0.2 }} positions={item?.Positions} />
				))}

				{focusedRestriction?.Positions ? <Polygon key={focusedRestriction.Id} interactive={false} pathOptions={{ color: "yellow", fillColor: "yellow", fillOpacity: 0.2 }} positions={focusedRestriction.Positions} /> : null}
			</>
		</MapProvider>
	);
}

export default RestrictionsMap;
