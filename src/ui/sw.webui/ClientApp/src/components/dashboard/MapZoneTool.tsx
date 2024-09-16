// Import React hooks
import React, { useState, useEffect, useCallback } from "react";

// Import Redux action creators
import { useSelector, useDispatch } from "react-redux";
import { setSelectedZones } from "../../redux/slices/dashBoardSlice";

// Import Leaflet components
import L from "leaflet";
import { Polygon, useMap } from "react-leaflet";
import { latLngBounds, LatLngExpression } from "leaflet";

// Import Devextreme components
import TagBox from "devextreme-react/tag-box";

// Import custom tools
import { getZonesByCompany } from "../../utils/apis/assets";
import { ZoneI } from "../../utils/types";

const zoneOptions = { color: "#ff4f4f", fillColor: "#ffffff", fillOpacity: 0.25 };

function MapZoneTool() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedZones } = useSelector((state: any) => state.dashboard);

	// States that handle zones
	const [availableZones, setAvailableZones] = useState<ZoneI[]>([]);

	const dispatch = useDispatch();

	const map = useMap();

	// Function that sets selected zones
	const onSelectedZoneChanged = useCallback(
		({ value }: any) => {
			dispatch(setSelectedZones(value));
		},
		[dispatch]
	);

	// Update map view when selectedZones changes
	useEffect(() => {
		if (selectedZones.length) {
			const zoneCoords: [number, number][] = selectedZones.map((item: { Positions: any }) => item?.Positions || []).flat();

			if (zoneCoords.length) {
				const bounds = latLngBounds([zoneCoords[0]]);

				zoneCoords.forEach((item) => bounds.extend(item));

				map.fitBounds(bounds, {
					padding: L.point(150, 150),
					animate: true
				});
			}
		}
	}, [selectedZones, map]);

	// Get zones and sets states accordingly
	useEffect(() => {
		(async () => {
			const data = await getZonesByCompany();

			if (data.length) {
				dispatch(setSelectedZones([data[0] as never]));
				setAvailableZones(data);
			}
		})();
	}, [dispatch]);

	return (
		<>
			<div className="leaflet-bottom leaflet-left" style={{ pointerEvents: "auto" }}>
				<TagBox
					disabled={availableZones.length === 0}
					className="leaflet-control"
					width={250}
					style={{ background: "white" }}
					value={selectedZones}
					onValueChanged={onSelectedZoneChanged}
					items={availableZones}
					displayExpr="Name"
					showSelectionControls={true}
					applyValueMode="instantly"
					maxFilterQueryLength={900000}
				/>
			</div>
			{/* Area polygon which indicates the assigned area of the user */}
			{selectedZones.length
				? selectedZones?.map((zone: { Id: React.Key | null | undefined; Name: string; Positions: LatLngExpression[] | LatLngExpression[][] | LatLngExpression[][][] }) => <Polygon key={zone.Id} interactive={false} pathOptions={zoneOptions} positions={zone?.Positions || []} />)
				: null}
		</>
	);
}

export default MapZoneTool;
