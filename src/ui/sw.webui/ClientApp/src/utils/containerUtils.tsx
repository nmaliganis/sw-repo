// import Leaflet components
import L from "leaflet";

// Import Devextreme components
import { formatDate } from "devextreme/localization";

// import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
// import { faSatelliteDish } from "@fortawesome/free-solid-svg-icons";

// Import custom tools
import { BucketStatus, CapacityType, colorBinStatus, DatePatterns, MaterialType, PickUpRateType, streamType } from "./consts";
import {
	imageCompositesHDPE1100,
	imageCompositesHDPE120,
	imageCompositesHDPE240,
	imageCompositesHDPE660,
	imageCompositesHDPE80,
	imageCompositesMetallicGrey240,
	imageCompositesMetallicGrey80,
	imageOrganicsBrown240,
	imageOrganicsBrown660,
	imageOrganicsBrown80,
	imageRecycleBlue1100,
	imageRecycleBlue240,
	imageRecycleBlue660,
	imageRecycleBlue80
} from "./base64Images";

export const bucketStatusOps = [
	{
		text: "Normal",
		value: ["Status", "=", 1]
	},
	{
		text: "BrokenLid",
		value: ["Status", "=", 2]
	},
	{
		text: "Broken Bucket",
		value: ["Status", "=", 3]
	},
	{
		text: "Damaged Pedal",
		value: ["Status", "=", 4]
	},
	{
		text: "Damaged Wheel",
		value: ["Status", "=", 5]
	},
	{
		text: "Burned",
		value: ["Status", "=", 6]
	},
	{
		text: "Changed Position",
		value: ["Status", "=", 7]
	},
	{
		text: "Missing",
		value: ["Status", "=", 8]
	}
];

export const materialTypeOps = [
	{
		text: "HDPE",
		value: ["Material", "=", 1]
	},
	{
		text: "Metallic",
		value: ["Material", "=", 2]
	},
	{
		text: "Other",
		value: ["Material", "=", 3]
	}
];

export const streamTypeOps = [
	{
		text: "Composites",
		value: ["WasteType", "=", 1]
	},
	{
		text: "Organics",
		value: ["WasteType", "=", 2]
	},
	{
		text: "Recycle",
		value: ["WasteType", "=", 3]
	}
];

export const capacityTypeOps = [
	{
		text: "80",
		value: ["Capacity", "=", 1]
	},
	{
		text: "120",
		value: ["Capacity", "=", 2]
	},
	{
		text: "240",
		value: ["Capacity", "=", 3]
	},
	{
		text: "660",
		value: ["Capacity", "=", 4]
	},
	{
		text: "1100",
		value: ["Capacity", "=", 5]
	}
];

export const highlightIcon = L.divIcon({
	iconAnchor: [15, 15],
	popupAnchor: [1, -13],
	html: `<span class="highlight-indicator"/>`,
	className: ""
});

// Function that sets initial values for Create Window
export const onInitNewRow = (e: { data: { CompanyId: number; AssetCategoryId: number; Status: number; Capacity: number; Material: number; WasteType: number; PickUpOn: number; MandatoryPickupActive: boolean; LocationMap: { Latitude: number; Longitude: number } } }) => {
	e.data = { CompanyId: 5, AssetCategoryId: 1, Status: BucketStatus[0].Id, Capacity: CapacityType[0].Id, Material: MaterialType[0].Id, WasteType: streamType[0].Id, PickUpOn: PickUpRateType[0].Id, MandatoryPickupActive: true, LocationMap: { Latitude: 37.98381, Longitude: 23.727539 } };
};

// Function that merges current with new data in order for the backend to get all required key values and not return 400
export const onRowUpdating = (options: { newData: any; oldData: any }) => {
	options.newData = { ...options.oldData, ...options.newData };
};

export const binStatusRenderer = ({ value }: { value: number }) => {
	return (
		<svg className="container-icon-state" height="25" width="25">
			<circle cx="13" cy="13" r="10" stroke="white" strokeWidth="2" fill={colorBinStatus(value).Color} />
		</svg>
	);
};

export const fillLevelRenderer = ({ data }) => {
	return `${data.Level} %`;

	// if (data.Level === data.PrevLevel) return `${data.Level} %`;

	// const levelIncreased = data.Level > data.PrevLevel;

	// return (
	// 	<div style={{ color: levelIncreased ? "#2ab71b" : "#f00" }}>
	// 		<div className="diff">
	// 			<i className={`dx-icon-${levelIncreased ? "arrowup" : "arrowdown"}`}></i> {data.Level} %
	// 		</div>
	// 	</div>
	// );
};

export const fillLevelPredRenderer = ({ value }: { value: string | number }) => {
	return `${value} %`;
};

export const dateRenderer = ({ value }: { value: string | number }) => {
	if (value) return formatDate(new Date(value), DatePatterns.LongDateTimeYearSmall);

	return "";
};

export const lastUpdatedRenderer = (e) => {
	if (e.value) {
		const isLive = Math.floor(Math.abs(new Date().getTime() - new Date(e.value).getTime()) / 36e5) > 6;

		return (
			<div className={isLive ? "outdated-cell-container" : ""} title={isLive ? "Outdated" : ""}>
				{formatDate(new Date(e.value), DatePatterns.LongDateTimeYearSmall)}
			</div>
		);
	}

	return "";
};

export const containerTypeImage = (streamType, materialType, capacityType) => {
	switch (streamType) {
		case 1: // Composites
		default:
			switch (materialType) {
				case 1: // HDPE
				default:
					// Green
					switch (capacityType) {
						case 80:
						default:
							return imageCompositesHDPE80;
						case 120:
							return imageCompositesHDPE120;
						case 240:
							return imageCompositesHDPE240;
						case 660:
							return imageCompositesHDPE660;
						case 1100:
							return imageCompositesHDPE1100;
					}

				case 2: // Metallic
					// Grey
					switch (capacityType) {
						case 80:
						case 120:
						default:
							return imageCompositesMetallicGrey80;
						case 240:
						case 660:
						case 1100:
							return imageCompositesMetallicGrey240;
					}
			}
		case 2: // Organics
			// Brown
			switch (capacityType) {
				case 80:
				case 120:
				default:
					return imageOrganicsBrown80;
				case 240:
					return imageOrganicsBrown240;
				case 660:
				case 1100:
					return imageOrganicsBrown660;
			}
		case 3: // Recycle
			// Blue
			switch (capacityType) {
				case 80:
				case 120:
				default:
					return imageRecycleBlue80;
				case 240:
					return imageRecycleBlue240;
				case 660:
					return imageRecycleBlue660;
				case 1100:
					return imageRecycleBlue1100;
			}
	}
};
