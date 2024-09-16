import { useState, useEffect, useMemo, useCallback, forwardRef, useImperativeHandle, useRef } from "react";

// Import devextreme components
import Popup from "devextreme-react/popup";
import Button from "devextreme-react/button";
import Form, { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";

// Import map tools
import L from "leaflet";
import MapProvider from "../MapProvider";
import { FeatureGroup } from "react-leaflet";
import { EditControl } from "react-leaflet-draw";

import "leaflet-draw/dist/leaflet.draw.css";
import styles from "../../styles/admin/Zones.module.scss";

const descriptionOptions = { height: 60 };

const ZoneMapForm = forwardRef(({ coords }: any, ref) => {
	// Return coordinates to parent
	useImperativeHandle(ref, () => ({
		getCoords: () => polygonCoords
	}));

	const [polygonCoords, setPolygonCoords] = useState([]);

	const featureGroupRef = useRef<any>();

	// Function that handles map to draw lines
	const onPolygonStart = useCallback((e) => {
		const drawnItems = featureGroupRef.current.getLayers();

		if (drawnItems.length) {
			drawnItems.forEach((layer, index) => {
				if (index > 0) return;
				featureGroupRef.current.removeLayer(layer);
			});
		}
	}, []);

	// Function that updates state when polygon is created
	const onPolygonCreated = useCallback((e) => {
		const coords = e.layer.getLatLngs();
		setPolygonCoords(coords);
	}, []);

	// Function that updates state when existing polygon exists
	const onPolygonEdited = useCallback((e) => {
		e.layers.eachLayer((layer) => {
			setPolygonCoords(layer.getLatLngs());
		});
	}, []);

	// Function that updates state when polygon is deleted
	const onPolygonDeleted = useCallback(() => {
		setPolygonCoords([]);
	}, []);

	// Update polygon lines when coords change
	useEffect(() => {
		if (coords) {
			const currentPolygon = L.polygon(coords);
			featureGroupRef.current.addLayer(currentPolygon);
		}
	}, [coords]);

	return (
		<FeatureGroup ref={featureGroupRef}>
			<EditControl
				position="topleft"
				onDrawStart={onPolygonStart}
				onCreated={onPolygonCreated}
				onEdited={onPolygonEdited}
				onDeleted={onPolygonDeleted}
				draw={{
					polygon: {
						icon: new L.DivIcon({
							iconSize: new L.Point(8, 8),
							className: "leaflet-div-icon leaflet-editing-icon"
						}),
						shapeOptions: {
							guidelineDistance: 10,
							color: "#03a9f4",
							weight: 3
						}
					},
					marker: false,
					circle: false,
					polyline: false,
					rectangle: false,
					circlemarker: false
				}}
			/>
		</FeatureGroup>
	);
});

function ZonesForm({ onSave, formState, dispatchForm }) {
	const mapRef = useRef<any>();

	const zoneMapRef = useRef<any>();

	let formData = useMemo(() => formState.data || {}, [formState.data]);

	const onFormCancel = useCallback(() => {
		dispatchForm({ type: "CANCEL" });
	}, [dispatchForm]);

	const onFormSave = useCallback(() => {
		onSave({ ...formData, Positions: zoneMapRef.current.getCoords() });
	}, [onSave, formData]);

	const onShown = () => {
		mapRef.current.invalidateSize();
	};

	return (
		<Popup visible={formState.visible} onShown={onShown} onHiding={onFormCancel} resizeEnabled={true} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={false} showTitle={true} title={formState.title} minWidth={800} maxWidth={1200} minHeight={500} maxHeight={900}>
			<div className={styles["form-zones-container"]}>
				<div className={styles["form-zones-items"]}>
					<Form formData={formData}>
						<FormItem itemType="group" colCount={2}>
							<FormItem dataField="Name">
								<Label text="Name" />
								<RequiredRule />
							</FormItem>

							<FormItem dataField="Active" editorType="dxSwitch">
								<Label text="Active" />
								<RequiredRule />
							</FormItem>
							<FormItem dataField="Description" colSpan={2} editorOptions={descriptionOptions}>
								<Label text="Description" />
								<RequiredRule />
							</FormItem>
						</FormItem>
					</Form>
					<div className={styles["form-zones-items-map"]}>
						<label className="dx-field-item-label dx-field-item-label-location-top">
							<span className="dx-field-item-label-content">
								<span className="dx-field-item-label-text">Select Zone Borders</span>
								<span className="dx-field-item-required-mark">&nbsp;*</span>
							</span>
						</label>

						{formState.visible ? (
							<MapProvider ref={mapRef} focusedData={formState.data?.Positions} style={{ flex: 1 }}>
								<ZoneMapForm ref={zoneMapRef} coords={formState.data?.Positions} />
							</MapProvider>
						) : null}
					</div>
				</div>

				<div className={styles["form-zones-footer"]}>
					<Button text="Save" type="default" stylingMode="text" onClick={onFormSave} />
					<Button text="Cancel" type="default" stylingMode="text" onClick={onFormCancel} />
				</div>
			</div>
		</Popup>
	);
}

export default ZonesForm;
