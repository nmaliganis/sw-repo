// Import React hooks
import React, { useState, useCallback, useMemo, useRef } from "react";

// Import Leaflet and map tools
import { LatLngExpression } from "leaflet";
import { initMapCenter } from "../../utils/consts";
import { Marker, Popup, useMap } from "react-leaflet";

// Import Devextreme components
import Button from "devextreme-react/button";
import DropDownButton from "devextreme-react/drop-down-button";

import "../../styles/dashboard/MapHomeTool.scss";

const dropDownOptions = { width: 200 };

const homeDropDownList = [
	{ Id: 1, Name: "Change home location" },
	{ Id: 2, Name: "Reset home location" }
];

function MapHomeTool() {
	// State that handles current home marker location
	const [homePosition, setHomePosition] = useState({ isChangeable: false, coords: JSON.parse(localStorage.getItem("dot-waste-dashboard-home") as string) ?? initMapCenter });

	// State that handles draggable marker state
	const [draggableMarker, setDraggableMarker] = useState(false);

	const markerRef = useRef<any>();

	const map = useMap();

	// Function that sets map view in accordance to home marker coordinates
	const onButtonHomeClick = () => {
		map.setView(homePosition.coords as LatLngExpression, 10);
	};

	// Function that disables draggable action
	const onButtonSetHome = () => {
		setDraggableMarker(false);
		setHomePosition((state) => ({ ...state, isChangeable: false }));
	};

	// Function that handles option selection for home map
	const onDropDownItemClick = ({ itemData }: any) => {
		switch (itemData.Id) {
			case 1:
				setHomePosition((state) => ({ ...state, isChangeable: true }));
				map.setView(homePosition.coords as LatLngExpression, 10);
				break;
			case 2:
				localStorage.setItem("dot-waste-dashboard-home", JSON.stringify(initMapCenter));
				setHomePosition({ isChangeable: false, coords: initMapCenter });

				map.setView(initMapCenter as LatLngExpression, 10);
				break;
			default:
				break;
		}
	};

	// Object that handles map event actions and updates home location on drag end
	const eventHandlers = useMemo(
		() => ({
			dragend() {
				const marker: any = markerRef.current;
				if (marker != null) {
					const coords = [marker.getLatLng().lat, marker.getLatLng().lng];

					localStorage.setItem("dot-waste-dashboard-home", JSON.stringify(coords));

					setHomePosition((state) => ({ ...state, coords: coords }));
				}
			}
		}),
		[]
	);

	const toggleDraggable = useCallback(() => {
		setDraggableMarker((state) => !state);
	}, []);

	return (
		<div
			className="leaflet-top leaflet-right"
			style={{ pointerEvents: "auto", margin: "11px 68px 0 0", zIndex: 700 }}
			onMouseOver={() => {
				map.dragging.disable();
			}}
			onMouseOut={() => {
				map.dragging.enable();
			}}
		>
			{homePosition.isChangeable ? (
				<>
					<Button className="home-button-container" icon="home" stylingMode="contained" type="default" hint="Set new home position" onClick={onButtonSetHome} />
					<Marker position={homePosition.coords} draggable={homePosition.isChangeable} eventHandlers={eventHandlers} ref={markerRef}>
						<Popup>
							<div style={{ padding: 20, textAlign: "center" }}>
								<div>{draggableMarker ? "Position is draggable." : "Click the button below to set container location."}</div>
								<Button text={draggableMarker ? "STOP" : "START"} type="default" stylingMode="outlined" onClick={toggleDraggable} />
							</div>
						</Popup>
					</Marker>
				</>
			) : (
				<DropDownButton
					className="home-button-dropdown-container"
					stylingMode="contained"
					splitButton={true}
					useSelectMode={false}
					dropDownOptions={dropDownOptions}
					icon="home"
					items={homeDropDownList}
					displayExpr="Name"
					keyExpr="Id"
					onButtonClick={onButtonHomeClick}
					onItemClick={onDropDownItemClick}
				/>
			)}
		</div>
	);
}

export default React.memo(MapHomeTool);
