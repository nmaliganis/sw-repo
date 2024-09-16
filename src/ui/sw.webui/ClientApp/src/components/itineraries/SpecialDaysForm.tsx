// Import React hooks
import { useState, useMemo, useCallback, useImperativeHandle, forwardRef } from "react";

// Import Devextreme components
import Button from "devextreme-react/button";
import { Popup } from "devextreme-react/popup";
import { formatDate } from "devextreme/localization";
import Form, { ButtonItem, GroupItem, SimpleItem, Label, RequiredRule, CompareRule } from "devextreme-react/form";

// Import custom tools
import { DatePatterns } from "../../utils/consts";
import { specialDayT } from "../../utils/itineraries";

// Import custom components
import { CalendarCardItem } from "./SpecialDaysCalendar";

import styles from "../../styles/itineraries/SpecialDays.module.scss";

// Object that holds settings for apply button
const applyButtonOptions = {
	text: "Apply",
	type: "success",
	useSubmitBehavior: true
};

// Object that holds settings for date box
const dateBoxOptions = {
	invalidDateMessage: "The date must have the following format: dd/MM/yyyy hh:mm",
	type: "datetime",
	displayFormat: DatePatterns.LongDateTimeNoSeconds
};

// Object that holds settings for description editor
const descriptionEditorOptions = { height: 90, maxLength: 200 };

// Initial state
const defaultSelectedItem = {
	name: "",
	startDate: new Date(),
	endDate: new Date(),
	repeat: false,
	description: ""
};

let formData = {
	Name: "",
	StartDate: new Date(),
	EndDate: new Date(),
	Repeat: false,
	Description: ""
};

const SpecialDaysForm = forwardRef((props, ref) => {
	// State that handles selected days and possible already created days
	const [selectedDate, setSelectedDate] = useState({ startDate: "", endDate: "", items: [] });
	// State that handles existing special day
	const [specialDay, setSpecialDay] = useState({ visible: false, selectedItem: defaultSelectedItem });

	// Ref that allows the parent to change state in child without re-rendering
	useImperativeHandle(ref, () => ({
		setDateState: (props) => {
			if (props.startDate !== props.endDate) {
				setSpecialDay((state) => ({ ...state, visible: true }));
			}

			setSelectedDate({ startDate: props.startDate, endDate: props.endDate, items: props.items });
		}
	}));

	// Function to handle Special Day creation
	const onCreateSpecialDay = useCallback(() => {
		// TODO: Add endpoint to create Special Day
		setSpecialDay({ visible: true, selectedItem: defaultSelectedItem });
	}, []);

	// Function to handle Special Day editing
	const onEditSpecialDay = useCallback((item) => {
		// TODO: Add endpoint to edit Special Day
		setSpecialDay({ visible: true, selectedItem: item });
	}, []);

	// Function to handle Special Day deletion
	const onDeleteSpecialDay = useCallback((item) => {
		// TODO: Add endpoint to delete Special Day
		console.log(item);
	}, []);

	// Function to handle event clearing from UI
	const onEventsHiding = useCallback(() => {
		setSelectedDate({ startDate: "", endDate: "", items: [] });
	}, []);

	// Function to handle hiding form hiding
	const onCreateHiding = useCallback(() => {
		setSpecialDay({ visible: false, selectedItem: defaultSelectedItem });
	}, []);

	// Function to handle data submission the API
	const handleSubmit = useCallback(
		(e) => {
			e.preventDefault();

			onEventsHiding();
		},
		[onEventsHiding]
	);

	// Object to handle Special Day creation cancelation
	const cancelButtonOptions = useMemo(
		() => ({
			text: "Cancel",
			type: "normal",
			stylingMode: "contained",
			onClick: () => {
				setSpecialDay({ visible: false, selectedItem: defaultSelectedItem });
			}
		}),
		[]
	);

	// Function to compare StartDate selection with EndDate to be smaller
	const startDateComparison = () => {
		return formData.EndDate;
	};

	// Function to compare EndDate selection with StartDate to be greater
	const endDateComparison = () => {
		return formData.StartDate;
	};

	// Condition that renders different Popups if user selects a range of dates from the Calendar
	if (selectedDate.startDate !== selectedDate.endDate) {
		formData = {
			Name: specialDay.selectedItem.name,
			StartDate: new Date(selectedDate.startDate),
			EndDate: new Date(selectedDate.endDate),
			Repeat: specialDay.selectedItem.repeat,
			Description: specialDay.selectedItem.description
		};

		return (
			<Popup visible={specialDay.visible} onHiding={onCreateHiding} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={true} showTitle={true} title="Special Day" width={400} height={450}>
				<form onSubmit={handleSubmit}>
					<Form formData={formData} readOnly={false} showColonAfterLabel={true} showValidationSummary={true} validationGroup="customerData">
						<GroupItem colCount={2}>
							<SimpleItem dataField="Name" colSpan={2}>
								<RequiredRule message="Name is required" />
							</SimpleItem>
							<SimpleItem dataField="StartDate" editorType="dxDateBox" editorOptions={dateBoxOptions}>
								<Label text="Start" />
								<RequiredRule message="Start date is required" />
								<CompareRule comparisonType="<=" message="Date must be lower than End date" comparisonTarget={startDateComparison} />
							</SimpleItem>
							<SimpleItem dataField="EndDate" editorType="dxDateBox" editorOptions={dateBoxOptions}>
								<Label text="End" />
								<RequiredRule message="End date is required" />
								<CompareRule comparisonType=">=" message="Date must be lower than Start date" comparisonTarget={endDateComparison} />
							</SimpleItem>
							<SimpleItem dataField="Repeat" editorType="dxSwitch" colSpan={2} />

							<SimpleItem dataField="Description" colSpan={2} editorType="dxTextArea" editorOptions={descriptionEditorOptions} />
						</GroupItem>
						<GroupItem colCount={2}>
							<ButtonItem horizontalAlignment="left" buttonOptions={cancelButtonOptions} />
							<ButtonItem buttonOptions={applyButtonOptions} />
						</GroupItem>
					</Form>
				</form>
			</Popup>
		);
	}

	formData = {
		Name: specialDay.selectedItem.name,
		StartDate: new Date(specialDay.selectedItem.startDate),
		EndDate: new Date(specialDay.selectedItem.endDate),
		Repeat: specialDay.selectedItem.repeat,
		Description: specialDay.selectedItem.description
	};

	return (
		<Popup visible={selectedDate.startDate !== ""} onHiding={onEventsHiding} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={true} showTitle={true} title={formatDate(new Date(selectedDate.startDate), DatePatterns.LongDateText)} width={700} height={480}>
			<div className={styles["special-days-form"]}>
				{selectedDate.items.length ? (
					<>
						{selectedDate.items.map((item: specialDayT, index) => (
							<div key={index} className={styles["special-days-form-container"]}>
								<CalendarCardItem data={item} />
								<div className={styles["special-days-form-buttons"]}>
									<Button icon="edit" type="default" stylingMode="text" onClick={() => onEditSpecialDay(item)} />
									<Button icon="trash" type="default" stylingMode="text" onClick={() => onDeleteSpecialDay(item)} />
								</div>
							</div>
						))}
						<hr />
					</>
				) : null}

				<Button text="Create New" type="default" stylingMode="contained" onClick={onCreateSpecialDay} />
				<Popup visible={specialDay.visible} onHiding={onCreateHiding} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={true} showTitle={true} title="Special Day" width={400} height={450}>
					<form onSubmit={handleSubmit}>
						<Form formData={formData} readOnly={false} showColonAfterLabel={true} showValidationSummary={true} validationGroup="customerData">
							<GroupItem colCount={2}>
								<SimpleItem dataField="Name" colSpan={2}>
									<RequiredRule message="Name is required" />
								</SimpleItem>
								<SimpleItem dataField="StartDate" editorType="dxDateBox" editorOptions={dateBoxOptions}>
									<Label text="Start" />
									<RequiredRule message="Start date is required" />
									<CompareRule comparisonType="<=" message="Date must be lower than End date" comparisonTarget={startDateComparison} />
								</SimpleItem>
								<SimpleItem dataField="EndDate" editorType="dxDateBox" editorOptions={dateBoxOptions}>
									<Label text="End" />
									<RequiredRule message="End date is required" />
									<CompareRule comparisonType=">=" message="Date must be lower than Start date" comparisonTarget={endDateComparison} />
								</SimpleItem>
								<SimpleItem dataField="Repeat" editorType="dxSwitch" colSpan={2} />

								<SimpleItem dataField="Description" colSpan={2} editorType="dxTextArea" editorOptions={descriptionEditorOptions} />
							</GroupItem>
							<GroupItem colCount={2}>
								<ButtonItem horizontalAlignment="left" buttonOptions={cancelButtonOptions} />
								<ButtonItem buttonOptions={applyButtonOptions} />
							</GroupItem>
						</Form>
					</form>
				</Popup>
			</div>
		</Popup>
	);
});

export default SpecialDaysForm;
