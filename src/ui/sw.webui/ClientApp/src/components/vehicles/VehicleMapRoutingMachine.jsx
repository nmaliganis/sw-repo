import { useEffect } from "react";
import L from "leaflet";
import { useMap } from "react-leaflet";

import "leaflet-routing-machine/dist/leaflet-routing-machine.css";
import "leaflet-routing-machine";

const waypoints1 = [L.latLng(38.07836562996712, 23.69164002388398), L.latLng(38.074311140919, 23.72343893801726), L.latLng(38.054311140919, 23.752343893801726)];

const waypoints2 = [L.latLng(38.05836562996712, 23.59164002388398), L.latLng(38.034311140919, 23.62343893801726), L.latLng(38.024311140919, 23.612343893801726)];

function VehicleMapRoutingMachine({ selectedVehicle }) {
	const map = useMap();

	useEffect(() => {
		if (!map) return;

		const instance = L.Routing.control({
			waypoints: [],
			lineOptions: {
				styles: [
					{
						color: "blue",
						opacity: 0.6,
						weight: 4
					}
				]
			},
			routeWhileDragging: false,
			draggableWaypoints: false,
			addWaypoints: false,
			collapsible: true,
			show: false
		}).addTo(map);

		if (selectedVehicle?.Name)
			if (selectedVehicle?.Id % 2 !== 0) instance.setWaypoints(waypoints1);
			else instance.setWaypoints(waypoints2);

		return () => map.removeControl(instance);
	}, [map, selectedVehicle]);

	return null;
}

export default VehicleMapRoutingMachine;
