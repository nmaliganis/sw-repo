// Import React hooks
import { useState } from "react";

// Import Redux action creators
import L from "leaflet";
import { latLngBounds } from "leaflet";
import { Polygon, useMap } from "react-leaflet";

// Import Devextreme components
import TagBox from "devextreme-react/tag-box";

const zoneOptions = { color: "#ff4f4f", fillColor: "#ffffff", fillOpacity: 0.35 };

const zones: { ID: number; Name: string; Positions: [number, number][] }[] = [
	{
		ID: 1,
		Name: "Zone 1",
		Positions: [
			[37.9393239, 23.6621475],
			[37.9398654, 23.6624908],
			[37.9417607, 23.669014],
			[37.9412192, 23.6772537],
			[37.9349915, 23.6875534],
			[37.9295757, 23.6861801],
			[37.9160343, 23.7012863],
			[37.901136, 23.7177658],
			[37.8810862, 23.7246323],
			[37.8599467, 23.7503815],
			[37.8729563, 23.7730408],
			[37.8878604, 23.7781906],
			[37.9084501, 23.7771606],
			[37.9027614, 23.7689209],
			[37.9051995, 23.7572479],
			[37.931742, 23.7603378],
			[37.955568, 23.7788773],
			[37.9739737, 23.7960434],
			[37.9975154, 23.831749],
			[38.0086072, 23.8447952],
			[38.0194268, 23.8396454],
			[38.0370053, 23.8101196],
			[38.0570126, 23.7696075],
			[38.0694467, 23.7170792],
			[38.0637705, 23.7108994],
			[38.0156401, 23.6645508],
			[38.0232133, 23.6528778],
			[38.0132057, 23.6384583],
			[37.9969743, 23.656311],
			[37.9785744, 23.6357117],
			[37.9709966, 23.6212921],
			[37.9653128, 23.6123657],
			[37.9550266, 23.6120224],
			[37.9485293, 23.6130524],
			[37.9420315, 23.6178589],
			[37.9423022, 23.6405182],
			[37.9401362, 23.6322784],
			[37.9376993, 23.6274719],
			[37.9336376, 23.625412],
			[37.9290341, 23.6267853],
			[37.9282216, 23.6418915],
			[37.9393239, 23.6621475]
		]
	},
	{
		ID: 2,
		Name: "Zone 2",
		Positions: [
			[38.079447, 22.960739],
			[38.076204, 22.977905],
			[38.053498, 23.026657],
			[38.052957, 23.059616],
			[38.061608, 23.117294],
			[38.03349, 23.116608],
			[38.02105, 23.130341],
			[37.996704, 23.106308],
			[37.968561, 23.164673],
			[37.952861, 23.181152],
			[37.922534, 23.139267],
			[37.912242, 23.058243],
			[37.924701, 23.050003],
			[37.917659, 23.014297],
			[37.951778, 22.962112],
			[37.979928, 22.982025],
			[37.988587, 22.958679],
			[38.007525, 22.92572],
			[38.022131, 22.891388],
			[38.027, 22.850876],
			[38.052957, 22.888641],
			[38.079447, 22.960739]
		]
	}
];

function MapGeoLocation() {
	// State that handles zone selection
	const [selectedZones, setSelectedZones] = useState([1]);

	const map = useMap();

	const onSelectedZoneChanged = (e: any) => {
		setSelectedZones(e.value);
	};

	const zoneCoords: [number, number][] = zones
		.filter((zone) => selectedZones.includes(zone.ID as never))
		.map((item) => item?.Positions || [])
		.flat();

	if (selectedZones.length) {
		const bounds = latLngBounds([zoneCoords[0]]);

		zoneCoords.forEach((item) => bounds.extend(item));

		map.fitBounds(bounds, {
			padding: L.point(150, 150),
			animate: true
		});
	}

	return (
		<>
			<div className="leaflet-bottom leaflet-left" style={{ pointerEvents: "auto" }}>
				<TagBox className="leaflet-control" width={250} style={{ background: "white" }} value={selectedZones} onValueChanged={onSelectedZoneChanged} items={zones} valueExpr="ID" displayExpr="Name" showSelectionControls={true} applyValueMode="instantly" />
			</div>
			{/* Area polygon which indicates the assigned area of the user */}
			{selectedZones.length ? zones.filter((zone) => selectedZones.includes(zone.ID as never)).map((zone) => <Polygon key={zone.ID} interactive={false} pathOptions={zoneOptions} positions={zone.Positions} />) : null}
		</>
	);
}

export default MapGeoLocation;
