// Import Leaflet components
import L from "leaflet";
import { useMap } from "react-leaflet";

// Import custom tools
import { MapCenterT } from "./types";
import { colorBinStatus, colorPalette } from "./consts";

import icon from "leaflet/dist/images/marker-icon.png";

interface markerCustomIcon {
	binStatus: number | null;
	iconSrc?: string;
	width?: string;
	height?: string;
}

// Component that renders custom markers from BinState
export const MarkerCustomIcon = ({ binStatus = null, iconSrc = icon, width = "25px", height = "41px" }: markerCustomIcon) => {
	let color = "transparent";

	if (binStatus !== null) color = colorBinStatus(binStatus).Color;

	// Code that can be used as an image instead of an svg
	// `<img src=${iconSrc} style="width: ${width}; height: ${height}; filter: drop-shadow(2px 0px 0 ${color}) drop-shadow(0px 2px 0 ${color}) drop-shadow(-2px -0px 0 ${color}) drop-shadow(-0px -2px 0 ${color});" />`

	return L.divIcon({
		className: "",
		iconAnchor: [13, 13],
		popupAnchor: [1, -13],
		html: `<svg class="container-icon-state" height="25" width="25">
		<circle cx="13" cy="13" r="10" stroke="white" stroke-width="2" fill=${color} />
	</svg>`
	});
};

interface clusterCustomIcon {
	getAllChildMarkers: () => any[];
	getChildCount: () => any;
}

// Component that renders custom cluster from BinStates
export const ClusterCustomIcon = (cluster: clusterCustomIcon) => {
	// Get unique colors from all markers and sort them in order to avoid having clusters with same colors but in different order
	const uniqueColors = cluster
		.getAllChildMarkers()
		.map((marker) => marker.options.alt.BinStatus)
		?.filter((value, index, self) => self.indexOf(value) === index)
		.sort();

	// Array of objects containing unique colors and the number of occurrences for each
	const colorOccurrence = uniqueColors.map((item) => {
		return {
			color: colorBinStatus(item).Color,
			number: cluster
				.getAllChildMarkers()
				.map((marker) => marker.options.alt.BinStatus)
				?.filter((v) => v === item).length,
			binStatus: item
		};
	});

	// previousColorStep holds the start of each colored part in the circle
	let previousColorStep = 0;

	// Create string of colors in shape (color, start_radius, end_radius) for each color
	const coloredCircle: string[] = colorOccurrence.map((item) => {
		const itemCount = Math.round((item.number / cluster.getAllChildMarkers().length) * 360);

		const colorPart = `${item.color} ${previousColorStep}deg ${previousColorStep + itemCount}deg`;
		previousColorStep = previousColorStep + itemCount;
		return colorPart;
	});

	return L.divIcon({
		html: `<div class="marker-cluster" style="background: radial-gradient(circle at center, #626262 50%, transparent 53%), conic-gradient(${coloredCircle});">
		<b>${cluster.getChildCount()}</b>
			<div class="marker-cluster-tooltip">
  				${colorOccurrence
						.map(
							(item) => `<div style="display: flex; align-items: center; gap: 1rem; color: black;">
										<svg class="container-icon-state" height="25" width="25">
											<circle cx="13" cy="13" r="10" stroke="white" stroke-width="2" fill=${item.color} />
										</svg>
										<div>${item.number} ${colorPalette[item.binStatus].Name}</div>
									</div>`
						)
						.join("")}
  			</div>
		</div>`,
		className: "",
		iconSize: L.point(40, 40)
	});
};

// Component that handles map view when coordinates change programmatically
export const ChangeViewFromNumberBox = ({ center, changeBounds }: MapCenterT) => {
	const map = useMap();

	const latLng = L.latLng(center.Latitude, center.Longitude);

	map.setView(latLng, map.getZoom());

	if (changeBounds)
		map.setMaxBounds([
			[center.Latitude, center.Longitude],
			[center.Latitude + 0.00001, center.Longitude + 0.00001]
		]);

	return null;
};
