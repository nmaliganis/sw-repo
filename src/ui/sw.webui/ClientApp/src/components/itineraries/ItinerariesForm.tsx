// Import React hooks
import React, { useState, useEffect, useCallback, useRef, forwardRef, useImperativeHandle } from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import Leaflet and map tools
import { Marker } from "react-leaflet";
import MapProvider from "../MapProvider";
import { MarkerCustomIcon } from "../../utils/mapUtils";

// Import Devextreme components
import Popup from "devextreme-react/popup";
import Button from "devextreme-react/button";
import Box, { Item } from "devextreme-react/box";
import LoadPanel from "devextreme-react/load-panel";
import { formatDate } from "devextreme/localization";
import Form, { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";
import DataGrid, { Column, ColumnChooser, Lookup, Scrolling, Selection, Sorting } from "devextreme-react/data-grid";

// Import custom tools
import { getTemplates } from "../../utils/apis/routing";
import { getContainersByZone } from "../../utils/apis/assets";
import { fillLevelRenderer, binStatusRenderer, highlightIcon } from "../../utils/containerUtils";
import { occurrenceRender, startEndLocationRender, streamRender, zoneContainersRender } from "../../utils/itinerariesUtils";
import { occurrenceType, BucketStatus, CapacityType, MaterialType, streamType, DatePatterns } from "../../utils/consts";
import styles from "../../styles/itineraries/Itineraries.module.scss";

let formData = {};

const position = { of: "#form-container" };

const descriptionOptions = { height: 100, maxLength: 200 };

const startTimeOptions = { type: "time", showClearButton: true };

const occurrenceOptions = {
	dataSource: occurrenceType,
	displayExpr: "Name",
	valueExpr: "Id",
	height: 32
};

const streamOptions = {
	dataSource: streamType,
	displayExpr: "Name",
	valueExpr: "Name"
};

const firstNameRender = ({ data }) => {
	return data.Member.Firstname;
};

const lastNameRender = ({ data }) => {
	return data.Member.Lastname;
};

// Function that changes title on current step
const stepTitle = (step) => {
	switch (step) {
		case 1:
		default:
			return "Basic Info";

		case 2:
			return "Select Containers";

		case 3:
			return "Select Vehicle";

		case 4:
			return "Select Driver";
	}
};

const StepController = React.memo(
	forwardRef(function StepController({ onPopUpShown, step }: any, ref) {
		// Enable ref actions from parents and update data
		useImperativeHandle(ref, () => ({
			getFormData: () => {
				return {
					selectedTemplate,
					selectedContainers,
					selectedVehicle,
					selectedDriver
				};
			}
		}));

		// Extract the relevant state from the Redux store using the useSelector hook
		const { userData } = useSelector((state: any) => state.login);
		const { startEndPoints, zonesByCompany, vehicles, drivers } = useSelector((state: any) => state.itinerary);

		// States that sets options for  editors
		const [templatesData, setTemplateData] = useState<any[]>([]);
		const [containersData, setContainersData] = useState<any[]>([]);

		// States that handle editor selections
		const [selectedTemplate, setSelectedTemplate] = useState<any>(null);
		const [selectedContainers, setSelectedContainers] = useState<any[]>([]);
		const [selectedVehicle, setSelectedVehicle] = useState<any>(null);
		const [selectedDriver, setSelectedDriver] = useState<any>(null);

		const formRef = useRef<any>();

		const mapRef = useRef<any>();

		// Function that sets selected templates
		const onFocusedTemplateChanged = useCallback(async (e) => {
			if (e.row?.data) {
				const zoneId = JSON.parse(e.row.data?.Zones)?.ZoneId;

				(async () => {
					const data = await getContainersByZone(zoneId);

					setContainersData(data);
				})();

				const { startFrom, endTo } = { startFrom: e.row.data.Locations.find((item) => item.IsStart === true), endTo: e.row.data.Locations.find((item) => item.IsStart === false) };

				const occurrence = JSON.parse(e.row.data.Occurrence).Occurrence;

				formData = { ...e.row.data, Occurrence: occurrence, ZoneId: zoneId, StartFrom: startFrom.Id, EndTo: endTo.Id };

				setSelectedTemplate(e.row.data);
			}
		}, []);

		// Function that sets selected containers
		const onContainersSelectionChange = ({ selectedRowsData }) => {
			setSelectedContainers(selectedRowsData);
		};

		// Function that sets selected vehicle
		const onVehicleSelectionChanged = (e) => {
			if (e.row?.data) setSelectedVehicle(e.row.data);
		};

		// Function that sets selected driver
		const onDriverSelectionChanged = (e) => {
			if (e.row?.data) setSelectedDriver(e.row.data);
		};

		// Get templates on init
		useEffect(() => {
			(async () => {
				const dataTemplates = await getTemplates();

				setTemplateData(dataTemplates);
			})();
		}, []);

		// Update form options on load
		useEffect(() => {
			if (formRef.current) {
				const formInstance = formRef.current.instance;

				// Create the new settings for the select box
				const zoneOptions = { dataSource: zonesByCompany, keyExpr: "Id", displayExpr: "Name" };

				const startEndOptions = {
					dataSource: startEndPoints,
					valueExpr: "Id",
					displayExpr: "Name"
				};

				// Change the options using itemOption property
				formInstance.itemOption("ZoneId", "editorOptions", zoneOptions);

				formInstance.itemOption("StartFrom", "editorOptions", startEndOptions);

				formInstance.itemOption("EndTo", "editorOptions", startEndOptions);
			}
		}, [startEndPoints, zonesByCompany]);

		// Re-render map to show correctly on map
		useEffect(() => {
			mapRef.current.invalidateSize();
		}, [onPopUpShown]);

		// This allows navigation on selection of containers
		let focusedData: [number, number][] = [];

		switch (step) {
			case 1:
			default:
				focusedData = selectedTemplate?.Zones ? containersData.map((item) => [item.Latitude, item.Longitude]) : [];
				break;
			case 2:
				focusedData = selectedContainers.length ? selectedContainers?.map((item) => [item.Latitude, item.Longitude]) : selectedTemplate?.Zones ? containersData?.map((item) => [item.Latitude, item.Longitude]) : [];
				break;
		}

		return (
			<>
				<LoadPanel shadingColor="rgba(0,0,0,0.4)" position={position} visible={templatesData.length === 0} showIndicator={true} shading={true} showPane={true} closeOnOutsideClick={false} />

				<div id="form-container" className={styles["form-itinerary-body"]}>
					<Box direction="row" width="50%" height="100%" style={{ paddingBottom: "1rem" }}>
						<Item ratio={1} visible={step === 1}>
							<div className={styles["form-template-container"]}>
								<span className={styles["form-template-title"]}>Select Itinerary Template</span>
								<DataGrid
									id="template-data-grid"
									width={534}
									height={200}
									dataSource={templatesData}
									keyExpr="Id"
									showBorders={true}
									showColumnLines={true}
									allowColumnResizing={true}
									rowAlternationEnabled={true}
									allowColumnReordering={true}
									focusedRowEnabled={true}
									hoverStateEnabled={true}
									autoNavigateToFocusedRow={false}
									focusedRowKey={selectedTemplate?.Id}
									onFocusedRowChanged={onFocusedTemplateChanged}
								>
									<Sorting mode="single" />
									<Scrolling mode="virtual" rowRenderingMode="virtual" />
									<ColumnChooser enabled={true} allowSearch={true} mode="select" />
									<Column dataField="Name" caption={"Name"} width={80} />
									<Column dataField="StartFrom" caption={"Start Location"} width={100} cellRender={startEndLocationRender} visible={false}>
										<Lookup dataSource={startEndPoints} displayExpr="Name" valueExpr="Id" />
									</Column>
									<Column dataField="EndTo" caption={"End Location"} width={100} cellRender={startEndLocationRender} visible={false}>
										<Lookup dataSource={startEndPoints} displayExpr="Name" valueExpr="Id" />
									</Column>
									<Column dataField="Zones" caption={"Containers"} width={90} minWidth={85} alignment="right" cellRender={zoneContainersRender}>
										<Lookup dataSource={zonesByCompany} displayExpr="Name" valueExpr="Id" />
									</Column>
									<Column dataField="Description" caption={"Description"} visible={false} showInColumnChooser={false} />
									<Column dataField="Occurrence" caption={"Occurrence"} width={100} cellRender={occurrenceRender}>
										<Lookup dataSource={occurrenceType} displayExpr="Name" valueExpr="Id" />
									</Column>
									<Column dataField="StartTime" caption={"Start Time"} width={90} minWidth={85} />
									<Column dataField="Stream" caption={"Stream"} width={90} minWidth={85} cellRender={streamRender}>
										<Lookup dataSource={streamType} displayExpr="Name" valueExpr="Id" />
									</Column>
									<Column dataField="MinFillLevel" caption={"Min. Level"} width={85} minWidth={85} />
								</DataGrid>
								<span className={styles["form-template-footer"]}> or create custom Itinerary</span>
							</div>
							<Form ref={formRef} formData={formData}>
								<FormItem itemType="group" colCount={2} colSpan={1}>
									<FormItem dataField="Name">
										<Label text="Name" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="ZoneId" editorType="dxSelectBox">
										<Label text="Zone" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="Description" editorType="dxTextArea" colSpan={2} editorOptions={descriptionOptions}>
										<Label text="Description" />
									</FormItem>
									<FormItem dataField="Occurrence" editorType="dxTagBox" editorOptions={occurrenceOptions}>
										<Label text="Occurrence" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="StartTime" editorType="dxDateBox" editorOptions={startTimeOptions}>
										<Label text="Start Time" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="StartFrom" editorType="dxSelectBox">
										<Label text="Start from" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="EndTo" editorType="dxSelectBox">
										<Label text="End to" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="Stream" editorType="dxSelectBox" editorOptions={streamOptions}>
										<Label text="Stream" />
										<RequiredRule />
									</FormItem>
									<FormItem dataField="MinFillLevel" editorType="dxNumberBox">
										<Label text="Min. Fill Level" />
										<RequiredRule />
									</FormItem>
								</FormItem>
							</Form>
						</Item>
						<Item ratio={1} visible={step === 2}>
							<DataGrid
								width={534}
								height={700}
								dataSource={selectedTemplate?.Zones ? containersData : []}
								keyExpr="Id"
								onSelectionChanged={onContainersSelectionChange}
								showBorders={true}
								showColumnLines={true}
								allowColumnResizing={true}
								rowAlternationEnabled={true}
								allowColumnReordering={true}
								hoverStateEnabled={true}
								autoNavigateToFocusedRow={false}
							>
								<Selection mode="multiple" />
								<Scrolling mode="virtual" rowRenderingMode="virtual" />
								<ColumnChooser enabled={true} allowSearch={true} mode="select" />
								<Column dataField="BinStatus" caption={"State"} cellRender={binStatusRenderer} width={80} alignment="center" allowFiltering={false} allowResizing={false} />
								<Column dataField="Name" caption={"Name"} filterOperations={["contains"]} width={80} />
								<Column dataField="Level" caption={"Fill Level"} cellRender={fillLevelRenderer} width={120} />
								<Column dataField="CompanyId" caption={"Company"} width={100} visible={false}>
									<Lookup dataSource={userData.UserParams.Companies} valueExpr="Id" displayExpr="Name" />
								</Column>
								<Column dataField="AssetCategoryId" caption={"Asset Category"} width={50} visible={false}>
									<Lookup dataSource={userData.UserParams.AssetCategories} valueExpr="Id" displayExpr="Name" />
								</Column>
								<Column dataField="Status" caption={"Status"} width={80}>
									<Lookup dataSource={BucketStatus} valueExpr="Id" displayExpr="Name" />
								</Column>
								<Column dataField="Material" caption={"Material"} width={80}>
									<Lookup dataSource={MaterialType} valueExpr="Id" displayExpr="Name" />
								</Column>
								<Column dataField="WasteType" caption={"Waste Type"} width={100}>
									<Lookup dataSource={streamType} valueExpr="Id" displayExpr="Name" />
								</Column>
								<Column dataField="Capacity" caption={"Capacity"} width={80}>
									<Lookup dataSource={CapacityType} valueExpr="Id" displayExpr="Name" />
								</Column>
							</DataGrid>
						</Item>
						<Item ratio={1} visible={step === 3}>
							<DataGrid
								width={534}
								height={700}
								dataSource={vehicles}
								keyExpr="Id"
								focusedRowEnabled={true}
								focusedRowKey={selectedVehicle?.Id}
								onFocusedRowChanged={onVehicleSelectionChanged}
								showBorders={true}
								showColumnLines={true}
								allowColumnResizing={true}
								rowAlternationEnabled={true}
								allowColumnReordering={true}
								hoverStateEnabled={true}
								autoNavigateToFocusedRow={false}
							>
								<Column dataField="Name" caption={"Name"} filterOperations={["contains"]} width={80} />
								<Column dataField="Brand" caption={"Brand"} width={80} />
								<Column dataField="NumPlate" caption={"Num Plate"} width={120} />
								<Column dataField="Type" caption={"Type"} width={80} />
								<Column dataField="Status" caption={"Status"} width={80} />
								<Column dataField="Gas" caption={"Gas"} width={80} />
								<Column dataField="Width" caption={"Width"} width={80} />
								<Column dataField="Height" caption={"Height"} width={80} />
								<Column dataField="Axels" caption={"Axels"} width={80} />
								<Column dataField="MinTurnRadius" caption={"Min. Turn Radius"} width={150} />
								<Column dataField="Length" caption={"Length"} width={80} />
								<Column dataField="CompanyId" caption={"Company"} width={120} visible={false} />
								<Column dataField="AssetCategory" caption={"Asset Category"} width={100} visible={false} />
								<Column dataField="Description" caption={"Description"} visible={false} showInColumnChooser={false} />
							</DataGrid>
						</Item>
						<Item ratio={1} visible={step === 4}>
							<DataGrid
								width={534}
								height={700}
								dataSource={drivers}
								keyExpr="Id"
								focusedRowEnabled={true}
								focusedRowKey={selectedDriver?.Id}
								onFocusedRowChanged={onDriverSelectionChanged}
								showBorders={true}
								showColumnLines={true}
								allowColumnResizing={true}
								rowAlternationEnabled={true}
								allowColumnReordering={true}
								hoverStateEnabled={true}
								autoNavigateToFocusedRow={false}
							>
								<Selection mode="single" />
								<Column dataField="MemberId" caption={"Member Id"} width={60} />
								<Column dataField="FirstName" caption={"First Name"} width={150} cellRender={firstNameRender} />
								<Column dataField="LastName" caption={"Last Name"} width={150} cellRender={lastNameRender} />
							</DataGrid>
						</Item>
					</Box>
					<MapProvider ref={mapRef} focusedData={focusedData} boxZoom={false} keyboard={false} dragging={false} touchZoom={false} trackResize={false} doubleClickZoom={false} scrollWheelZoom={false} style={{ padding: "0 0 1rem 1rem" }}>
						{selectedContainers?.map((item: any, index) => {
							return <Marker key={index} zIndexOffset={99998} interactive={false} position={[item.Latitude, item.Longitude]} icon={highlightIcon} />;
						})}
						{selectedTemplate?.Zones &&
							containersData.map((item: any, index) => {
								const markerIcon = MarkerCustomIcon({ binStatus: item.BinStatus, iconSrc: item.Icon, width: item.Width, height: item.Height });

								return <Marker key={index} position={[item.Latitude, item.Longitude]} icon={markerIcon} alt={item}></Marker>;
							})}
					</MapProvider>
				</div>
			</>
		);
	})
);

function ItinerariesForm({ formState, dispatchForm }) {
	// State that handles form visualization and steps
	const [currentStep, setCurrentStep] = useState(1);
	const [onPopUpShown, setOnPopUpShown] = useState(false);

	const stepsFormRef = useRef<any>();

	const onShown = useCallback(() => {
		setOnPopUpShown((state) => !state);
	}, []);

	// Function that resets state on cancel
	const onFormCancel = useCallback(() => {
		setCurrentStep(1);
		dispatchForm({ type: "CANCEL" });
	}, [dispatchForm]);

	// Function that updates draft on save draft
	const onSaveDraft = useCallback(() => {
		const formData = stepsFormRef.current.getFormData();

		const data = {
			Name: formData.selectedTemplate.Name,
			StartTime: formData.selectedTemplate.StartTime,
			Vehicles: [formData.selectedVehicle],
			Containers: formData.selectedContainers,
			DriverId: formData.selectedDriver.Id,
			ItineraryTemplateId: formData.selectedTemplate.Id,
			CorrelationItineraryId: 0
		};

		dispatchForm({ type: "DRAFT", data: data });
	}, [dispatchForm]);

	const onBackClick = useCallback(() => {
		setCurrentStep((state) => state - 1);
	}, []);

	const onNextClick = useCallback(() => {
		setCurrentStep((state) => state + 1);
	}, []);

	const onFinishClick = useCallback(() => {
		const formData = stepsFormRef.current.getFormData();

		const data = {
			Name: `${formData.selectedTemplate.Name} - ${formatDate(new Date(), DatePatterns.LongDateTimeNoSeconds)}`,
			StartTime: formData.selectedTemplate.StartTime,
			Vehicles: [formData.selectedVehicle],
			Containers: formData.selectedContainers.map((item) => ({ ...item, Seq: formData.selectedContainers.map((item) => item.Id) })),
			DriverId: formData.selectedDriver.Id,
			ItineraryTemplateId: formData.selectedTemplate.Id,
			CorrelationItineraryId: 0
		};

		setCurrentStep(1);
		dispatchForm({ type: "FINISH", data: data });
	}, [dispatchForm]);

	return (
		<Popup visible={formState.visible} onShown={onShown} dragEnabled={false} closeOnOutsideClick={false} showCloseButton={false} showTitle={true} title={formState.title} width={1100} height={880}>
			<Box direction="col" width="100%" height="100%">
				<Item baseSize="auto">
					<span className={styles["form-itinerary-title"]}>{stepTitle(currentStep)}</span>
				</Item>
				<Item ratio={1}>{formState ? <StepController ref={stepsFormRef} step={currentStep} onPopUpShown={onPopUpShown} /> : <></>}</Item>
				<Item baseSize="auto">
					<div className={styles["form-itinerary-buttons"]}>
						<div className={styles["form-itinerary-buttons-step"]}>
							<Button text="Cancel" type="normal" stylingMode="contained" onClick={onFormCancel} />
							<Button text="Save as Draft" type="normal" stylingMode="text" onClick={onSaveDraft} />
						</div>

						<div className={styles["form-itinerary-buttons-step"]}>
							<Button text="Back" visible={currentStep > 1} type="default" stylingMode="contained" onClick={onBackClick} />
							<Button text={currentStep === 4 ? "Finish" : "Next"} type={currentStep === 4 ? "success" : "default"} stylingMode="contained" onClick={currentStep === 4 ? onFinishClick : onNextClick} />
						</div>
					</div>
				</Item>
			</Box>
		</Popup>
	);
}

export default ItinerariesForm;
