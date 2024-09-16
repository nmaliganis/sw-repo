// Import React hooks
import { useState, useEffect, useCallback } from "react";

// import Devextreme components
import notify from "devextreme/ui/notify";
import TagBox from "devextreme-react/tag-box";
import DateBox from "devextreme-react/date-box";
import NumberBox from "devextreme-react/number-box";

// Import custom tools
import { getZonesByCompany } from "../../../utils/apis/assets";
import { DatePatterns, streamType } from "../../../utils/consts";
import { GenerateDropDownButton } from "../../../utils/reportUtils";
import { getReportDocument } from "../../../utils/apis/report";

function ReportsOccupancyForm({ isLoading, setOccupancyReport }) {
	// State that handles form data
	const [formData, setFormData] = useState({
		dateFrom: new Date().setDate(new Date().getDate() - 5),
		dateTo: new Date().getTime(),
		selectedZones: [],
		selectedStreams: [],
		fillLevelLimit: 0.5
	});

	// State to load zones
	const [availableZones, setAvailableZones] = useState([]);

	// Function that handles start time range
	const onDateFromChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value).getTime();

			if (newDate < formData.dateTo) setFormData((state) => ({ ...state, dateFrom: newDate }));
		},
		[formData.dateTo]
	);

	// Function that handles end time range
	const onDateToChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value).getTime();

			if (newDate > formData.dateFrom) setFormData((state) => ({ ...state, dateTo: newDate }));
		},
		[formData.dateFrom]
	);

	// Function that updates selected zone on form
	const onZoneSelectionChange = useCallback(({ value }: any) => {
		setFormData((state) => ({ ...state, selectedZones: value }));
	}, []);

	// Function that updates selected stream on form
	const onStreamSelectionChange = useCallback(({ value }: any) => {
		setFormData((state) => ({ ...state, selectedStreams: value }));
	}, []);

	// Function that updates selected fill limit on form
	const onFillLimitChanged = useCallback((e) => {
		setFormData((state) => ({ ...state, fillLevelLimit: e.value }));
	}, []);

	// Validation function that checks user input
	const validationEngine = useCallback((form) => {
		if (!form.dateFrom) {
			notify("No date From selected.", "error", 3000);
			return false;
		} else if (!form.dateTo) {
			notify("No date To selected.", "error", 3000);
			return false;
		} else if (form.selectedZones.length === 0) {
			notify("No Zones were selected.", "error", 3000);
			return false;
		} else if (form.selectedStreams.length === 0) {
			notify("No Streams were selected.", "error", 3000);
			return false;
		}

		return true;
	}, []);

	const onItemClick = useCallback(
		({ itemData }) => {
			if (validationEngine(formData)) {
				getReportDocument(itemData.Id, formData);
			}
		},
		[validationEngine, formData]
	);

	const onFormSubmit = useCallback(() => {
		if (validationEngine(formData)) {
			setOccupancyReport(formData);
		}
	}, [validationEngine, formData, setOccupancyReport]);

	useEffect(() => {
		(async () => {
			const data = await getZonesByCompany();

			setAvailableZones(data);
		})();
	}, []);

	return (
		<div className="reports-bin-collection-container">
			<div className="date-range-picker">
				<DateBox type="date" value={formData.dateFrom} onValueChanged={onDateFromChanged} max={formData.dateTo} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
				<i className="dx-icon-minus" style={{ display: "flex", alignItems: "center" }} />
				<DateBox type="date" value={formData.dateTo} onValueChanged={onDateToChanged} min={formData.dateFrom} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
			</div>
			<TagBox
				placeholder="Select Zones..."
				dataSource={availableZones}
				width={200}
				valueExpr="Id"
				displayExpr="Name"
				applyValueMode="useButtons"
				value={formData.selectedZones}
				onValueChanged={onZoneSelectionChange}
				maxFilterQueryLength={900000}
				multiline={false}
				searchEnabled={true}
				showSelectionControls={true}
			/>
			<TagBox
				placeholder="Select Streams..."
				dataSource={streamType}
				width={200}
				valueExpr="Id"
				displayExpr="Name"
				applyValueMode="useButtons"
				value={formData.selectedStreams}
				onValueChanged={onStreamSelectionChange}
				maxFilterQueryLength={900000}
				multiline={false}
				searchEnabled={true}
				showSelectionControls={true}
			/>
			<span>Limit:</span>
			<NumberBox width={60} format="#0%" min={0} max={1} step={0.01} value={formData.fillLevelLimit} onValueChanged={onFillLimitChanged} />
			<GenerateDropDownButton isLoading={isLoading} onItemClick={onItemClick} onButtonClick={onFormSubmit} />
		</div>
	);
}

export default ReportsOccupancyForm;
