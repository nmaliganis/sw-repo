// Import React hooks
import { useState, useEffect, useCallback, useMemo, useRef, Dispatch, SetStateAction } from "react";

// Import Devextreme components
import { Form } from "devextreme-react/data-grid";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";
import Button from "devextreme-react/button";
import FileUploader from "devextreme-react/file-uploader";
import ProgressBar from "devextreme-react/progress-bar";
import { NumberBox } from "devextreme-react/number-box";
import CheckBox from "devextreme-react/check-box";
import DateBox from "devextreme-react/date-box";
import "devextreme-react/text-area";

// Import Leaflet and map components
import L from "leaflet";
import { ChangeViewFromNumberBox } from "../../utils/mapUtils";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import icon from "leaflet/dist/images/marker-icon.png";
import iconShadow from "leaflet/dist/images/marker-shadow.png";

// Import custom tools
import { containerTypeImage } from "../../utils/containerUtils";
import { BucketStatus, CapacityType, DatePatterns, MaterialType, PickUpRateType, streamType } from "../../utils/consts";

import "leaflet/dist/leaflet.css";
import "../../styles/containers/ContainerTableForm.scss";

// Allowed image selection
const allowedFileExtensions = [".jpg", ".jpeg", ".gif", ".png"];

// Object to set up icon in case CSS is not loading
const DefaultIcon = L.icon({
	iconUrl: icon,
	shadowUrl: iconShadow,
	iconSize: [25, 41],
	iconAnchor: [10, 41],
	popupAnchor: [2, -40]
});

L.Marker.prototype.options.icon = DefaultIcon;

export const DraggableMarker = ({ tempLocationMarker, setTempLocationMarker, containerCellInfo }: { tempLocationMarker: any; setTempLocationMarker: Dispatch<SetStateAction<{ Latitude: number; Longitude: number }>>; containerCellInfo: any }) => {
	// State that handles marker to be draggable on map
	const [draggableMarker, setDraggableMarker] = useState(false);

	const markerRef = useRef(null);

	// Object that updates location of marker when the marker is let go
	const eventHandlers = useMemo(
		() => ({
			dragend() {
				const marker: any = markerRef.current;
				if (marker != null) {
					const coords = { Latitude: marker.getLatLng().lat, Longitude: marker.getLatLng().lng };

					setTempLocationMarker(coords);
				}
			}
		}),
		[setTempLocationMarker]
	);

	// Function that handles action to update the marker on the Form
	const toggleDraggable = useCallback(() => {
		setDraggableMarker((state) => {
			if (state) {
				containerCellInfo.setValue(tempLocationMarker);
			}
			return !state;
		});
	}, [setDraggableMarker, tempLocationMarker, containerCellInfo]);

	// Update marker location
	useEffect(() => {
		if (containerCellInfo.value) setTempLocationMarker(containerCellInfo.value);
	}, [containerCellInfo.value, setTempLocationMarker]);

	return (
		<Marker draggable={draggableMarker} eventHandlers={eventHandlers} position={{ lat: tempLocationMarker.Latitude, lng: tempLocationMarker.Longitude }} ref={markerRef}>
			<Popup>
				<div style={{ padding: 20, textAlign: "center" }}>
					<div>{draggableMarker ? "Container is draggable." : "Click the button below to set container location."}</div>
					<Button text={draggableMarker ? "STOP" : "START"} type="default" stylingMode="outlined" onClick={toggleDraggable} />
				</div>
			</Popup>
		</Marker>
	);
};

// Function that passes component and allows use of hooks on the mandatory pick up editor
export const editMandatoryPickUpRenderer = (cellInfo: { value: any; setValue: any }) => {
	return <EditMandatoryPickUpForm cellInfo={cellInfo} />;
};

// Function that passes component and allows use of hooks on the picture editor
export const editPictureRenderer = (cellInfo: { data: any; value: any; setValue: any }) => {
	return <EditPictureForm cellInfo={cellInfo} />;
};

// Function that passes component and allows use of hooks on the location editor
export const editLocationRenderer = (cellInfo: { value: any; setValue: any }) => {
	return <EditLocationForm cellInfo={cellInfo} />;
};

const EditMandatoryPickUpForm = ({ cellInfo }: { cellInfo: { value: any; setValue: any } }) => {
	// States that get updated in accordance of the data coming from cellInfo if the user selects Edit or Create mode
	const [mandatoryPickupActive, setMandatoryPickUpActive] = useState<boolean>(cellInfo.value?.MandatoryPickupActive ? cellInfo.value?.MandatoryPickupActive : false);
	const [mandatoryPickUpDate, setMandatoryPickUpDate] = useState(cellInfo.value?.MandatoryPickUpDate ? cellInfo.value?.MandatoryPickUpDate : new Date());

	// Update form value on state change
	const onMandatoryPickupChange = ({ value }: any) => {
		cellInfo.setValue({ MandatoryPickupActive: value, MandatoryPickUpDate: mandatoryPickUpDate });
		setMandatoryPickUpActive(value);
	};

	// Update form value on state change
	const onDateValueChanged = ({ value }: any) => {
		cellInfo.setValue({ MandatoryPickupActive: mandatoryPickupActive, MandatoryPickUpDate: value });
		setMandatoryPickUpDate(value);
	};

	return (
		<>
			<CheckBox className="mandatory-pickup-checkbox" text="Mandatory pick up" value={mandatoryPickupActive} onValueChanged={onMandatoryPickupChange} />
			<DateBox className="mandatory-pickup-date" type="datetime" label="Mandatory pick up date" labelMode="floating" value={mandatoryPickUpDate} onValueChanged={onDateValueChanged} disabled={!mandatoryPickupActive} displayFormat={DatePatterns.LongDateTimeNoSeconds} />
		</>
	);
};

// Add logic to handle image upload
const EditPictureForm = ({ cellInfo }: { cellInfo: { data: any; value: any; setValue: any } }) => {
	// States that handle image uploading actions
	const [isDropZoneActive, setDropZoneActive] = useState(false);
	const [progressValue, setProgressValue] = useState(0);

	// State image that gets updated when the user changes the waste, material, capacity type
	const [imageSource, setImageSource] = useState(containerTypeImage(cellInfo?.data?.WasteType, cellInfo?.data?.Material, cellInfo?.data?.Capacity));

	// Function that enables drag and drop action
	const onDropZoneEnter = (e: { dropZoneElement: { id: string } }) => {
		if (e.dropZoneElement.id === "dropzone-external") {
			setDropZoneActive(true);
		}
	};

	// Function that disables drag and drop action
	const onDropZoneLeave = (e: { dropZoneElement: { id: string } }) => {
		if (e.dropZoneElement.id === "dropzone-external") {
			setDropZoneActive(false);
		}
	};

	// Function that handles file upload
	const onUploaded = (e: { file: any }) => {
		const { file } = e;
		const fileReader: any = new FileReader();
		fileReader.onload = () => {
			setDropZoneActive(false);
			setImageSource(fileReader.result);
		};
		fileReader.readAsDataURL(file);
		setProgressValue(0);
	};

	// Function that clears up state on uploading
	const onUploadStarted = () => {
		setImageSource("");
		setProgressValue(1);
	};

	// Function that handles errors when image fails to load
	const onUploadError = (e: { request: any; message: string; error: { responseText: any } }) => {
		let httpRequest = e.request;

		if (httpRequest.status === 400) {
			e.message = e.error.responseText;
		}
		if (httpRequest.readyState === 4 && httpRequest.status === 0) {
			e.message = "Connection failed";
		}
	};

	// Function that updates progress indicator
	const onProgress = (e: { bytesLoaded: number; bytesTotal: number }) => {
		setProgressValue((e.bytesLoaded / e.bytesTotal) * 100);
	};

	return (
		<div className="widget-container">
			<div id="dropzone-external" className={`flex-box ${isDropZoneActive ? "dx-theme-accent-as-border-color dropzone-active" : "dx-theme-border-color"}`}>
				{imageSource && <img id="dropzone-image" src={imageSource} alt="" />}
				{!imageSource && (
					<div id="dropzone-text" className="flex-box">
						<span>Drag & drop an image or click to browse instead.</span>
					</div>
				)}
				<ProgressBar id="upload-progress" min={0} max={100} width="30%" showStatus={false} visible={progressValue !== 0} value={progressValue}></ProgressBar>
			</div>
			<FileUploader
				disabled={true}
				id="file-uploader"
				dialogTrigger="#dropzone-external"
				dropZone="#dropzone-external"
				multiple={false}
				allowedFileExtensions={allowedFileExtensions}
				uploadMode="instantly"
				uploadUrl="https://js.devexpress.com/Demos/NetCore/FileUploader/Upload"
				visible={false}
				onDropZoneEnter={onDropZoneEnter}
				onDropZoneLeave={onDropZoneLeave}
				onProgress={onProgress}
				onUploaded={onUploaded}
				onUploadError={onUploadError}
				onUploadStarted={onUploadStarted}
			/>
		</div>
	);
};

const EditLocationForm = ({ cellInfo }: { cellInfo: { value: any; setValue: any } }) => {
	// State that handles the visualization of the marker on the map
	const [tempLocationMarker, setTempLocationMarker] = useState({ Latitude: 37.98381, Longitude: 23.727539 });

	let locationMap = cellInfo.value;

	// Function that updates latitude manually and set the form state
	const setLatManually = (e: any) => {
		setTempLocationMarker((state) => {
			cellInfo.setValue({ Latitude: e.value, Longitude: state.Longitude });

			return { Latitude: e.value, Longitude: state.Longitude };
		});
	};

	// Function that updates longitude manually and set the form state
	const setLngManually = (e: any) => {
		setTempLocationMarker((state) => {
			cellInfo.setValue({ Latitude: state.Latitude, Longitude: e.value });

			return { Latitude: state.Latitude, Longitude: e.value };
		});
	};

	return (
		<>
			<div style={{ backgroundColor: "rgba(191, 191, 191, 0.15)", display: "flex", justifyContent: "left", padding: 10 }}>
				<NumberBox valueChangeEvent="keyup" value={tempLocationMarker.Latitude} onValueChanged={setLatManually} style={{ marginRight: 5 }} />
				<NumberBox valueChangeEvent="keyup" value={tempLocationMarker.Longitude} onValueChanged={setLngManually} />
			</div>
			<div className="table-form-map-wrapper">
				<MapContainer className="table-form-map-wrapper" center={{ lat: locationMap.Latitude, lng: locationMap.Longitude }} zoom={13} scrollWheelZoom={false}>
					<ChangeViewFromNumberBox center={tempLocationMarker} />
					<TileLayer attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors' url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
					<DraggableMarker tempLocationMarker={tempLocationMarker} setTempLocationMarker={setTempLocationMarker} containerCellInfo={cellInfo} />
				</MapContainer>
			</div>
		</>
	);
};

function ContainerTableForm(userData) {
	// Function that sets data options in accordance to the editor
	const editorOptions = (dataType) => {
		let items: any = [];

		switch (dataType) {
			case 0:
				items = userData.UserParams.Companies;
				break;
			case 1:
				items = userData.UserParams.AssetCategories;
				break;
			case 2:
				items = CapacityType;
				break;
			case 3:
				items = BucketStatus;
				break;
			case 4:
				items = PickUpRateType;
				break;
			case 5:
				items = MaterialType;
				break;
			case 6:
				items = streamType;
				break;

			default:
				break;
		}

		return {
			items: items,
			displayExpr: "Name",
			valueExpr: "Id"
		};
	};

	return (
		<Form labelMode="floating" height="100%" elementAttr={{ class: "table-form-container" }}>
			<FormItem itemType="group" colCount={2} colSpan={1} cssClass="info-item-container">
				<FormItem itemType="group" colCount={1} colSpan={1}>
					<FormItem dataField="CompanyId" editorType="dxSelectBox" editorOptions={editorOptions(0)}>
						<Label text="Company" />
						<RequiredRule />
					</FormItem>
					<FormItem dataField="Name">
						<Label text="Name" />
						<RequiredRule />
					</FormItem>
					<FormItem dataField="ZoneId">
						<Label text="Zone" />
						<RequiredRule />
					</FormItem>
					<FormItem dataField="ContainerStatus" editorType="dxSelectBox" editorOptions={editorOptions(3)}>
						<Label text="Container Status" />
					</FormItem>
					<FormItem dataField="Material" editorType="dxSelectBox" editorOptions={editorOptions(5)}>
						<Label text="Bucket Material" />
						<RequiredRule />
					</FormItem>
					<FormItem dataField="WasteType" editorType="dxSelectBox" editorOptions={editorOptions(6)}>
						<Label text="Bucket Type" />
						<RequiredRule />
					</FormItem>
				</FormItem>
				<FormItem itemType="group" colCount={1} colSpan={1}>
					<FormItem dataField="AssetCategoryId" editorType="dxSelectBox" editorOptions={editorOptions(1)}>
						<Label text="Asset Category" />
						<RequiredRule />
					</FormItem>
					<FormItem dataField="Capacity" editorType="dxSelectBox" editorOptions={editorOptions(2)}>
						<Label text="Capacity (Lt)" />
						<RequiredRule />
					</FormItem>

					<FormItem dataField="MandatoryPickup">
						<Label visible={false} />
					</FormItem>
					<FormItem dataField="PickUpOn" editorType="dxSelectBox" editorOptions={editorOptions(4)}>
						<Label text="Mandatory Pick up method" />
					</FormItem>
				</FormItem>

				<FormItem dataField="Description" editorType="dxTextArea" colCount={2} colSpan={2}>
					<Label text="Description" />
				</FormItem>
			</FormItem>
			<FormItem itemType="group" colCount={1} colSpan={1} cssClass="image-item-container">
				<FormItem dataField="Image" cssClass="image-item-container" />
			</FormItem>
			<FormItem itemType="group" caption="Location" colCount={1} colSpan={2} cssClass="form-location-group">
				<FormItem dataField="LocationMap" cssClass="form-location-item" />
			</FormItem>
		</Form>
	);
}

export default ContainerTableForm;
