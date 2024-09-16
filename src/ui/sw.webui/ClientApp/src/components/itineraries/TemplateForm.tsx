// Import React hooks
import { useState, useCallback, forwardRef, useRef, useImperativeHandle } from "react";

// Import Devextreme components
import { Editing, Popup, Form } from "devextreme-react/data-grid";
import { Item, Label, RequiredRule } from "devextreme-react/form";

// Import Leaflet and map tools
import L from "leaflet";
import MapProvider from "../MapProvider";
import { Marker, Polygon } from "react-leaflet";
import { ClusterCustomIcon } from "../../utils/mapUtils";
import ContainerMarker from "../dashboard/ContainerMarker";
import MarkerClusterGroup from "react-leaflet-markercluster";
import { getContainersByZone, getZonesByCompany } from "../../utils/apis/assets";

import start from "../../images/start.png";
import finish from "../../images/finish.png";

import "../../styles/itineraries/Template.scss";

const startMarkerIcon = L.icon({
	iconUrl: start,
	iconSize: [24, 24],
	iconAnchor: [12, 24],
	popupAnchor: [0, -24]
});

const endMarkerIcon = L.icon({
	iconUrl: finish,
	iconSize: [24, 24],
	iconAnchor: [12, 24],
	popupAnchor: [0, -24]
});

const descriptionOptions = { height: 100, maxLength: 200 };

const startTimeOptions = { type: "time", showClearButton: true };

const minFillLevelOptions = { format: "#0%" };

const occurrenceEditorOptions = { height: 32, showSelectionControls: true, applyValueMode: "useButtons" };

const zonePolygonOptions = { color: "#ff4f4f", fillColor: "#ffffff", fillOpacity: 0.25 };

const EditMapTemplate = forwardRef((props, ref) => {
	// State that handles new Template item visualization
	const [selectedZone, setSelectedZone] = useState<any>(null);
	const [containersByZone, setContainersByZone] = useState<any>([]);
	const [startPoint, setStartPoint] = useState<any>(null);
	const [endPoint, setEndPoint] = useState<any>(null);

	// Ref that sets Zones from API, and start, end location from parent component
	useImperativeHandle(
		ref,
		() => {
			return {
				setZone(zoneId) {
					(async () => {
						const zonesData = await getZonesByCompany();

						const containersData = await getContainersByZone(zoneId);

						if (containersData.length) {
							setContainersByZone(containersData);
						}

						if (zonesData.length) {
							setSelectedZone(zonesData[0] as never);
						}
					})();
				},
				setStartLocation(location) {
					setStartPoint(location);
				},
				setEndLocation(location) {
					setEndPoint(location);
				}
			};
		},
		[]
	);

	// Get coordinates to set focused items on map
	const focusedData = containersByZone?.map((item) => [item.Latitude, item.Longitude]) || [];

	return (
		<MapProvider focusedData={focusedData}>
			{selectedZone && <Polygon interactive={false} pathOptions={zonePolygonOptions} positions={selectedZone?.Positions || []} />}
			<MarkerClusterGroup disableClusteringAtZoom={16} iconCreateFunction={ClusterCustomIcon} chunkedLoading={true} zoomToBoundsOnClick={true} showCoverageOnHover={true} animateAddingMarkers={false} removeOutsideVisibleBounds={true}>
				{containersByZone?.map((mapItem: any) => {
					return <ContainerMarker key={mapItem.Id} mapItem={mapItem} />;
				})}
			</MarkerClusterGroup>
			{startPoint && <Marker position={[startPoint.Lat, startPoint.Lon]} icon={startMarkerIcon} />}
			{endPoint && <Marker position={[endPoint.Lat, endPoint.Lon]} icon={endMarkerIcon} />}
		</MapProvider>
	);
});

function TemplateForm(startEndPoints) {
	const mapTemplateRef = useRef<any>();
	const currentChangesRef = useRef<any>({});

	// Function that handles changes to specific editors and visualize results
	const onFormChangesChange = useCallback(
		(change) => {
			if (change.length) {
				const currentChanges = change[0]?.data;

				if (currentChanges.Zones && currentChanges.Zones !== currentChangesRef.current?.Zones) {
					mapTemplateRef.current.setZone(currentChanges.Zones);
				}

				if (currentChanges.StartLocation && currentChanges.StartLocation !== currentChangesRef.current?.StartLocation) {
					mapTemplateRef.current.setStartLocation(startEndPoints.find((item) => item.Id === currentChanges.StartLocation));
				}

				if (currentChanges.EndLocation && currentChanges.EndLocation !== currentChangesRef.current?.EndLocation) {
					mapTemplateRef.current.setEndLocation(startEndPoints.find((item) => item.Id === currentChanges.EndLocation));
				}

				currentChangesRef.current = currentChanges;
			}
		},
		[startEndPoints]
	);

	return (
		<Editing mode="popup" onChangesChange={onFormChangesChange}>
			<Popup title="Template" showTitle={true} width={900} height={550} />
			<Form>
				<Item itemType="group" colCount={2} colSpan={2}>
					<Item itemType="group" colCount={2} colSpan={1}>
						<Item dataField="Name">
							<Label text="Name" />
							<RequiredRule />
						</Item>
						<Item dataField="Zones" editorType="dxSelectBox">
							<Label text="Zone" />
							<RequiredRule />
						</Item>
						<Item dataField="Description" editorType="dxTextArea" colSpan={2} editorOptions={descriptionOptions}>
							<Label text="Description" />
						</Item>
						<Item dataField="Occurrence" editorType="dxTagBox" editorOptions={occurrenceEditorOptions}>
							<Label text="Occurrence" />
							<RequiredRule />
						</Item>
						<Item dataField="StartTime" editorType="dxDateBox" editorOptions={startTimeOptions}>
							<Label text="Start Time" />
							<RequiredRule />
						</Item>
						<Item dataField="StartFrom" editorType="dxSelectBox">
							<Label text="Start from" />
							<RequiredRule />
						</Item>
						<Item dataField="EndTo" editorType="dxSelectBox">
							<Label text="End to" />
							<RequiredRule />
						</Item>
						<Item dataField="Stream" editorType="dxSelectBox">
							<Label text="Stream" />
							<RequiredRule />
						</Item>
						<Item dataField="MinFillLevel" editorType="dxNumberBox" editorOptions={minFillLevelOptions}>
							<Label text="Min. Fill Level" />
							<RequiredRule />
						</Item>
					</Item>
					<Item itemType="group" cssClass={"template-form"} colCount={1} colSpan={1}>
						<Item render={() => <EditMapTemplate ref={mapTemplateRef} />} />
					</Item>
				</Item>
			</Form>
		</Editing>
	);
}

export default TemplateForm;
