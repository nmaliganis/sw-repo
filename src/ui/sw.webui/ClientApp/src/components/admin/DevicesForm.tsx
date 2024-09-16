// Import React hooks
import { useState, useEffect, useMemo, useCallback, useRef } from "react";

// Import DevExtreme components
import notify from "devextreme/ui/notify";
import Button from "devextreme-react/button";
import FileUploader from "devextreme-react/file-uploader";
import LoadIndicator from "devextreme-react/load-indicator";
import { Form, SimpleItem, EmptyItem, GroupItem, Label, RequiredRule, TabbedItem, TabPanelOptions, Tab, ButtonItem } from "devextreme-react/form";

// Import Leaflet and custom map components
import { MapContainer } from "react-leaflet";
import ReactLeafletGoogleLayer from "react-leaflet-google-layer";

import ED120Device from "../../images/ED120 Device.png";

const allowedFileExtensions = [".jpg", ".jpeg", ".png"];

const deviceIndicator = {
	readOnly: true
};

// Arrays for handling step form
const stepInformationItems = ["DeviceID", "ContainerType", "ContainerSize"];
const stepPositionItems = ["Location"];
const stepPhotosItems = ["Photos"];
const stepActivationItems = ["DeviceActivated"];

const selectBoxSettings = {
	valueExpr: "Id",
	displayExpr: "Name",
	searchEnabled: true,
	placeholder: "Search...",
	searchMode: "contains",
	searchExpr: "Name",
	items: [
		{ Id: 1, Name: "Choice 1" },
		{ Id: 2, Name: "Choice 2" }
	]
};

function DevicesForm({ formData, popupMode, dispatchPopup, dataSource }: any) {
	// State that handles current step on form
	const [selectedTabIndex, setSelectedIndex] = useState(0);

	// State to handle Photos on form
	const [arePhotosLoading, setArePhotosLoading] = useState(false);
	const [selectedPhotos, setSelectedPhotos] = useState<string[]>([]);

	const [activationState, setActivationState] = useState("Standby");

	const fileUploaderRef = useRef<any>();
	const formRef = useRef<any>();

	// Function that handles form validation
	const confirmClick = useCallback(
		(e) => {
			let result = formRef.current?.instance.validate();

			if (result.isValid) {
				if (popupMode === "Add") {
					dataSource.insert(formData);
				} else if (popupMode === "Edit") {
					dataSource.push(formData["Id"], formData);
				}

				dispatchPopup({ type: "hidePopup" });
				setSelectedIndex(0);
			} else {
				for (let i = 0; i < result.brokenRules.length; i++) {
					if (stepInformationItems.some((substring) => result.brokenRules[i].message.includes(substring))) {
						setSelectedIndex(0);
						break;
					} else if (stepPositionItems.some((substring) => result.brokenRules[i].message.includes(substring))) {
						setSelectedIndex(1);
						break;
					} else if (stepPhotosItems.some((substring) => result.brokenRules[i].message.includes(substring))) {
						setSelectedIndex(2);
						break;
					} else if (stepActivationItems.some((substring) => result.brokenRules[i].message.includes(substring))) {
						setSelectedIndex(3);
						break;
					}
				}
				formRef.current?.instance.validate();
			}
		},
		[formData, popupMode, dataSource, dispatchPopup]
	);

	// Object for previous button settings. Memoization prevents flickering
	const previousBtnOptions = useMemo(() => {
		return {
			text: "PREVIOUS",
			type: "normal",
			onClick: () => {
				setSelectedIndex((state) => state - 1);
			}
		};
	}, []);

	// Object for previous button settings. Memoization prevents flickering
	const nextBtnOptions = useMemo(() => {
		return {
			text: "NEXT",
			type: "normal",
			onClick: () => {
				setSelectedIndex((state) => state + 1);
			}
		};
	}, []);

	// Object for confirm button settings. Memoization prevents flickering
	const confirmBtnOptions = useMemo(() => {
		return {
			text: "CONFIRM",
			type: "success",
			onClick: confirmClick
		};
	}, [confirmClick]);

	// Function that generates custom map form indicator
	const renderMapLocation = useCallback(() => {
		return (
			<div className="map-container" style={{ height: 370 }}>
				<MapContainer className="map-container" center={[37.98381, 23.727539]} zoomControl={false} zoom={10} minZoom={10} maxZoom={10} closePopupOnClick={false} zoomAnimationThreshold={2} zoomAnimation={false}>
					<ReactLeafletGoogleLayer apiKey={process.env.REACT_APP_GOOGLE_API_KEY} type={"roadmap"} />
				</MapContainer>
			</div>
		);
	}, []);

	// Function that generates custom photo form
	const renderPhotos = useCallback(() => {
		// Function that handles uploaded image
		const onUploaded = (e: { file: any }) => {
			const { file } = e;
			const fileReader = new FileReader();
			fileReader.onload = () => {
				if (selectedPhotos.some((photo) => photo === fileReader.result)) {
					fileUploaderRef.current._instance.abortUpload();
					notify("Photo already exists.", "error", 5000);
				} else {
					setSelectedPhotos((state) => [...state, fileReader.result as string]);
				}
			};
			fileReader.readAsDataURL(file);
		};

		// Function that handles actions when upload is started
		const onUploadStarted = () => {
			if (selectedPhotos.length >= 3) {
				fileUploaderRef.current._instance.abortUpload();
				notify("You can't upload more than 3 photos", "warning", 5000);
			} else {
				setArePhotosLoading(true);
			}
		};

		// Function that handles multiple file upload
		const onFilesUploaded = () => {
			setArePhotosLoading(false);
		};

		// Function that handles image deletion
		const onPhotoDelete = (photo: string) => {
			setSelectedPhotos((state) => state.filter((item) => item !== photo));
		};

		return (
			<>
				<div id="file-upload" style={{ display: "flex", alignItems: "center", gap: 12, justifyContent: "center" }}>
					{arePhotosLoading ? (
						<>
							<LoadIndicator height={30} width={30} />
							<div>Loading...</div>
						</>
					) : (
						<>
							<Button style={{ fontSize: 17 }}>Select Photos</Button>
							<div style={{ userSelect: "none" }}>or Drop photos here</div>
						</>
					)}
				</div>
				<br />
				<FileUploader
					ref={fileUploaderRef}
					multiple={true}
					visible={false}
					onUploaded={onUploaded}
					onUploadStarted={onUploadStarted}
					onFilesUploaded={onFilesUploaded}
					accept="image/*"
					uploadMode="instantly"
					dropZone="#file-upload"
					dialogTrigger="#file-upload"
					labelText="or Drop photos here"
					selectButtonText="Select Photo"
					allowedFileExtensions={allowedFileExtensions}
					uploadUrl="https://js.devexpress.com/Demos/NetCore/FileUploader/Upload"
				/>
				<div className="device-details-images-container">
					{selectedPhotos?.length ? (
						selectedPhotos.map((photo: string, index: number) => (
							<div key={index} className="device-details-image">
								<img src={photo} width={165} alt={`Item ${index}`} />
								<Button className="device-details-image-remove-button" icon="close" type="normal" stylingMode="contained" onClick={() => onPhotoDelete(photo)} />
							</div>
						))
					) : (
						<div className="device-details-no-image">
							<i className="dx-icon-image" style={{ fontSize: "72px", color: "#585858cc" }}></i>
							<p style={{ marginBottom: 0 }}>No image(s) selected</p>
						</div>
					)}
				</div>
			</>
		);
	}, [arePhotosLoading, selectedPhotos]);

	// Function that renders state of device activation
	const renderDeviceActivation = useCallback(() => {
		switch (activationState) {
			case "Pending":
				return (
					<div className="device-details-activation" style={{}}>
						<h2>Please Wait...</h2>
						<LoadIndicator height={140} width={140} />
						<h3 style={{ paddingBottom: 35 }}>Evaluation is in progress</h3>
					</div>
				);
			case "Complete":
				return (
					<div className="device-details-activation">
						<h2>Activation complete!</h2>
						<i className="dx-icon-check device-details-activation-complete" />
					</div>
				);
			case "Standby":
			default:
				return (
					<>
						<img src={ED120Device} alt="Device Activation" width="100%" />
						<p>
							Please place the magnet at the specified <span style={{ color: "#ec1919", fontWeight: "bold" }}>point</span> for at least 2 seconds
						</p>
					</>
				);
		}
	}, [activationState]);

	// Function for custom rendering
	const renderVerificationTitle = useCallback(() => {
		return (
			<div style={{ textAlign: "center" }}>
				<h2>Evaluation Complete</h2>
				<p>Please check sensor values</p>
			</div>
		);
	}, []);

	// Function for custom rendering
	const renderCompleteIndicator = useCallback(() => {
		return (
			<div style={{ display: "flex", flexDirection: "column", padding: "7em" }}>
				<i className="dx-icon-check device-details-activation-complete" style={{ textAlign: "center" }} />
			</div>
		);
	}, []);

	useEffect(() => {
		if (selectedTabIndex === 3) {
			// setTimeout(() => {
			// 	setActivationState("Complete");
			// 	setSelectedIndex((state) => state + 1);
			// }, 4000);
		}
	}, [selectedTabIndex]);

	return (
		<Form ref={formRef} formData={formData} labelLocation="top" showColonAfterLabel={true} validationGroup="userForm">
			<TabbedItem cssClass="custom-tabbed-item">
				<TabPanelOptions deferRendering={false} selectedIndex={selectedTabIndex} />
				<Tab title="Information" disabled={selectedTabIndex !== 0}>
					<GroupItem itemType="group" colCount={2} colSpan={2}>
						<SimpleItem dataField="DeviceID" editorType="dxSelectBox" editorOptions={selectBoxSettings} colSpan={2}>
							<Label text="Device ID" />
							<RequiredRule message="DeviceID is required" />
						</SimpleItem>
						<SimpleItem dataField="ContainerType" editorType="dxSelectBox" editorOptions={selectBoxSettings} colSpan={2}>
							<Label text="Container Type" />
							<RequiredRule message="ContainerType is required" />
						</SimpleItem>
						<SimpleItem dataField="ContainerSize" editorType="dxSelectBox" editorOptions={selectBoxSettings} colSpan={2}>
							<Label text="Container Size" />
							<RequiredRule message="ContainerSize is required" />
						</SimpleItem>
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
						<EmptyItem colSpan={2} />
					</GroupItem>
					<ButtonItem buttonOptions={nextBtnOptions} colSpan={2} />
				</Tab>
				<Tab title="Position" disabled={selectedTabIndex !== 1}>
					<SimpleItem dataField="Location" render={renderMapLocation}>
						<Label text="Location" />
					</SimpleItem>

					<GroupItem colCount={2} colSpan={1}>
						<ButtonItem cssClass="previous-button-container" buttonOptions={previousBtnOptions} />
						<ButtonItem buttonOptions={nextBtnOptions} />
					</GroupItem>
				</Tab>
				<Tab title="Photos (Optional)" disabled={selectedTabIndex !== 2}>
					<SimpleItem dataField="Photos" render={renderPhotos}>
						<Label visible={false} />
					</SimpleItem>
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />

					<GroupItem colCount={2} colSpan={1}>
						<ButtonItem cssClass="previous-button-container" buttonOptions={previousBtnOptions} />
						<ButtonItem buttonOptions={nextBtnOptions} />
					</GroupItem>
				</Tab>
				<Tab title="Activation" disabled={selectedTabIndex !== 3}>
					<SimpleItem dataField="DeviceActivated" render={renderDeviceActivation}>
						<Label visible={false} />
					</SimpleItem>
					<EmptyItem colSpan={2} />

					<GroupItem colCount={1} colSpan={1}>
						<ButtonItem cssClass="device-details-center-button" buttonOptions={previousBtnOptions} />
					</GroupItem>
				</Tab>
				<Tab title="Verification" disabled={selectedTabIndex !== 4}>
					<SimpleItem render={renderVerificationTitle} />
					<GroupItem colCount={3} colSpan={1}>
						<SimpleItem dataField="DeviceLevel" editorOptions={deviceIndicator}>
							<Label text="Level" />
						</SimpleItem>
						<SimpleItem dataField="DeviceLevelComment" colSpan={2}>
							<Label text="Add comment" />
						</SimpleItem>
					</GroupItem>
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<GroupItem colCount={3} colSpan={1}>
						<SimpleItem dataField="DeviceTemp" editorOptions={deviceIndicator}>
							<Label text="Temperature" />
						</SimpleItem>
						<SimpleItem dataField="DeviceTempComment" colSpan={2}>
							<Label text="Add comment" />
						</SimpleItem>
					</GroupItem>
					<EmptyItem colSpan={2} />
					<EmptyItem colSpan={2} />
					<GroupItem colCount={2} colSpan={1}>
						<ButtonItem cssClass="previous-button-container" buttonOptions={previousBtnOptions} />
						<ButtonItem buttonOptions={nextBtnOptions} />
					</GroupItem>
				</Tab>
				<Tab title="Complete" disabled={selectedTabIndex !== 5}>
					<SimpleItem render={renderCompleteIndicator} />

					<GroupItem colCount={1} colSpan={1}>
						<ButtonItem cssClass="device-details-center-button" buttonOptions={confirmBtnOptions} />
					</GroupItem>
				</Tab>
			</TabbedItem>
		</Form>
	);
}

export default DevicesForm;
