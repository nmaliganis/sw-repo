// Import React hooks
import React, { useEffect } from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setSelectedMapItem } from "../../redux/slices/dashBoardSlice";
import { setPopupDetails } from "../../redux/slices/modalSlice";

// Import Leaflet and map tools
import L from "leaflet";
import { MarkerCustomIcon } from "../../utils/mapUtils";
import { Marker, Popup, useMap } from "react-leaflet";

// Import Devextreme components
import { formatDate } from "devextreme/localization";

// Import custom tools
import { getEventHistoryData } from "../../utils/apis/assets";
import { LineListHorizontal } from "../../utils/dashboardUtils";
import { colorBinStatus, DatePatterns } from "../../utils/consts";
import { containerTypeImage } from "../../utils/containerUtils";

// Function that compares props and updates the map accordingly
const compareProps = (prevProps, nextProps) => {
	return JSON.stringify(prevProps.mapItem) === JSON.stringify(nextProps.mapItem);
};

const SummaryMapSelection = ({ mapItem }) => {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedMapItem } = useSelector((state: any) => state.dashboard);

	const dispatch = useDispatch();

	const map = useMap();

	// Set map position to selected map item, clear selected map and open popup
	useEffect(() => {
		if (selectedMapItem) {
			if (selectedMapItem.Id === mapItem.Id) {
				const latLng = L.latLng(selectedMapItem.Latitude.toFixed(7), selectedMapItem.Longitude.toFixed(7));

				map.setView(latLng, 18);

				dispatch(setSelectedMapItem(null));
			}
		}
	}, [dispatch, map, selectedMapItem, mapItem.Id]);

	return null;
};

function ContainerMarker({ mapItem }) {
	const dispatch = useDispatch();

	const markerIcon = MarkerCustomIcon({ binStatus: mapItem.BinStatus, iconSrc: mapItem.Icon, width: mapItem.Width, height: mapItem.Height });

	const fillColor = colorBinStatus(mapItem.BinStatus).Color;

	return (
		<Marker key={mapItem.Id} position={[mapItem.Latitude, mapItem.Longitude]} icon={markerIcon} alt={mapItem}>
			<SummaryMapSelection mapItem={mapItem} />

			<Popup key={mapItem.Id} maxWidth={1000} closeOnClick={false}>
				<div className="popup-title">
					<div style={{ fontWeight: 800, fontSize: 18 }}>{mapItem.Name}</div>
					<div>{mapItem.Description}</div>
					<div style={{ padding: "8px 0 4px 0", color: "#A6AEB1" }}>
						Last updated: <span style={{ color: "black" }}>{formatDate(new Date(mapItem.LastUpdated), DatePatterns.LongDateTimeNoSeconds)}</span>
					</div>
				</div>

				<div className="popup-container">
					<div
						className="progress-image"
						style={{
							backgroundImage: `linear-gradient( to top, ${fillColor}73, ${fillColor}73 ${mapItem.Level}%, #00000000 ${mapItem.Level}%, #00000000),
                                						  url("data:image/png;base64,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")`
						}}
					/>

					<div style={{ height: "100%", display: "flex", justifyContent: "center", alignItems: "center" }}>
						<div>
							<div style={{ fontWeight: 800, fontSize: 16 }}>LEVEL</div>
							<div style={{ fontWeight: 800, fontSize: 24 }}>{mapItem.Level} %</div>
						</div>
					</div>

					{mapItem.Image === "default" ? (
						<img src={containerTypeImage(mapItem.WasteType, mapItem.Material, mapItem.Capacity)} alt="Container" style={{ height: "100%" }} loading="lazy" />
					) : mapItem.Image instanceof Array ? (
						<LineListHorizontal>
							{mapItem?.Image?.map((item, index) => {
								return (
									<div key={index} className="content">
										<img src={item} alt={`container-${index}`} width="100%" loading="lazy" />
									</div>
								);
							})}
						</LineListHorizontal>
					) : (
						<img src={mapItem.Image} alt="container" width="100%" loading="lazy" />
					)}
				</div>

				<div className="popup-footer">
					<button
						className="dx-widget dx-button dx-button-mode-text dx-button-default"
						style={{ border: "transparent" }}
						onClick={async (e) => {
							e.stopPropagation();
							e.preventDefault();
							dispatch(setPopupDetails({ visible: true, selectedMapItem: mapItem, selectedMapItemHistory: await getEventHistoryData({ selectedItem: mapItem.Id }) }));
						}}
					>
						<div className="dx-button-content">
							<b>Details</b>
						</div>
					</button>
				</div>
			</Popup>
		</Marker>
	);
}

export default React.memo(ContainerMarker, compareProps);
