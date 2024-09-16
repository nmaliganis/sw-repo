// Import React hooks
import { useState, useCallback } from "react";

// Import Devextreme components
import notify from "devextreme/ui/notify";
import TagBox from "devextreme-react/tag-box";
import DateBox from "devextreme-react/date-box";

// Import custom tools
import { DatePatterns, streamType } from "../../../utils/consts";
import { GenerateDropDownButton } from "../../../utils/reportUtils";
import { getReportDocument } from "../../../utils/apis/report";

function ReportsRouteIndicatorsForm({ isLoading, setRouteIndicatorsParams }) {
	// State that handles form values
	const [formData, setFormData] = useState({
		dateFrom: new Date().setDate(new Date().getDate() - 5),
		dateTo: new Date().getTime(),
		selectedStreams: []
	});

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

	// Function that updates selected stream on form
	const onStreamSelectionChange = useCallback(({ value }: any) => {
		setFormData((state) => ({ ...state, selectedStreams: value }));
	}, []);

	// Validation function that checks user input
	const validationEngine = useCallback((form) => {
		if (!form.dateFrom) {
			notify("No date From selected.", "error", 3000);
			return false;
		} else if (!form.dateTo) {
			notify("No date To selected.", "error", 3000);
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

	// Function that handles form validation
	const onFormSubmit = useCallback(() => {
		if (validationEngine(formData)) {
			setRouteIndicatorsParams(formData);
		}
	}, [validationEngine, formData, setRouteIndicatorsParams]);

	return (
		<div className="reports-bin-collection-container">
			<div className="date-range-picker">
				<DateBox type="date" value={formData.dateFrom} onValueChanged={onDateFromChanged} max={formData.dateTo} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
				<i className="dx-icon-minus" style={{ display: "flex", alignItems: "center" }} />
				<DateBox type="date" value={formData.dateTo} onValueChanged={onDateToChanged} min={formData.dateFrom} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
			</div>
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
			<GenerateDropDownButton isLoading={isLoading} onItemClick={onItemClick} onButtonClick={onFormSubmit} />
		</div>
	);
}

export default ReportsRouteIndicatorsForm;
