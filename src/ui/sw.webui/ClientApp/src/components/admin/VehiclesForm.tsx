// Import React hooks
import { useState } from "react";

// Import DevExtreme components
import { Form } from "devextreme-react/data-grid";
import { Item as FormItem, RequiredRule } from "devextreme-react/form";
import FileUploader from "devextreme-react/file-uploader";
import ProgressBar from "devextreme-react/progress-bar";
import "devextreme-react/text-area";

// Define allowed file extensions
const allowedFileExtensions = [".jpg", ".jpeg", ".gif", ".png"];

// Function for custom form picture item
export const editPictureRenderer = (cellInfo: { value: any; setValue: any }) => {
	return <EditPictureForm cellInfo={cellInfo} />;
};

const EditPictureForm = ({ cellInfo }: { cellInfo: { value: any; setValue: any } }) => {
	const [isDropZoneActive, setDropZoneActive] = useState(false);
	const [imageSource, setImageSource] = useState("");
	const [progressValue, setProgressValue] = useState(0);

	// Function for when mouse enters dropzone
	const onDropZoneEnter = (e: { dropZoneElement: { id: string } }) => {
		if (e.dropZoneElement.id === "dropzone-external") {
			setDropZoneActive(true);
		}
	};

	// Function for when mouse leaves dropzone
	const onDropZoneLeave = (e: { dropZoneElement: { id: string } }) => {
		if (e.dropZoneElement.id === "dropzone-external") {
			setDropZoneActive(false);
		}
	};

	// Function for when file is uploaded
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

	// Function for when file upload starts
	const onUploadStarted = () => {
		setImageSource("");
		setProgressValue(1);
	};

	// Function for when file upload encounters an error
	const onUploadError = (e: { request: any; message: string; error: { responseText: any } }) => {
		let httpRequest = e.request;

		if (httpRequest.status === 400) {
			e.message = e.error.responseText;
		}
		if (httpRequest.readyState === 4 && httpRequest.status === 0) {
			e.message = "Connection failed";
		}
	};

	// Function for updating progress value during file upload
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

function VehiclesForm() {
	return (
		<Form>
			<FormItem itemType="group" colCount={2} colSpan={2}>
				<FormItem dataField="NumPlate">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="Name">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="Brand">
					<RequiredRule />
				</FormItem>

				<FormItem dataField="Status" />
				<FormItem dataField="CompanyId">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="AssetCategoryId">
					<RequiredRule />
				</FormItem>
				<FormItem itemType="group" colCount={1} colSpan={1} cssClass="image-item-container">
					<FormItem dataField="Image" cssClass="image-item-container">
						<RequiredRule />
					</FormItem>
				</FormItem>
				<FormItem itemType="group" colCount={1} colSpan={1} cssClass="image-item-container">
					<FormItem dataField="Type" />
					<FormItem dataField="RegisteredDate" />
					<FormItem dataField="Gas" />
					<FormItem dataField="Width" />
					<FormItem dataField="Height" />
					<FormItem dataField="Axels" />
				</FormItem>
				<FormItem dataField="MinTurnRadius" />
				<FormItem dataField="Length" />
				<FormItem dataField="CodeErp">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="Description" colSpan={2} editorType="dxTextArea" />
			</FormItem>
		</Form>
	);
}

export default VehiclesForm;
