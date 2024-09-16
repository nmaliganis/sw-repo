// Import DevExtreme components
import { Form } from "devextreme-react/data-grid";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";

const calcPositionOptions = {
	text: "Calculate position"
};

const resetValuesOptions = {
	text: "Reset values"
};

const showAtChartOptions = {
	text: "Show at chart"
};

const showAtReportOptions = {
	text: "Show at report"
};

const showAtStatusOptions = {
	text: "Show at status"
};

const showOnMapOptions = {
	text: "Show on map"
};

const sumValuesOptions = {
	text: "Sum values"
};

function SensorTypesForm() {
	return (
		<Form>
			<FormItem itemType="group" colCount={3} colSpan={2}>
				<FormItem dataField="Name">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="StatusExpiryMinutes" />
				<FormItem dataField="Precision" editorType="dxNumberBox">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="Tunit" />
				<FormItem dataField="CodeErp">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="SensorTypeIndex" />

				<FormItem dataField="CalcPosition" editorType="dxCheckBox" editorOptions={calcPositionOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="ResetValues" editorType="dxCheckBox" editorOptions={resetValuesOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="ShowAtChart" editorType="dxCheckBox" editorOptions={showAtChartOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="ShowAtReport" editorType="dxCheckBox" editorOptions={showAtReportOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="ShowAtStatus" editorType="dxCheckBox" editorOptions={showAtStatusOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="ShowOnMap" editorType="dxCheckBox" editorOptions={showOnMapOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="SumValues" editorType="dxCheckBox" editorOptions={sumValuesOptions}>
					<Label visible={false} />
				</FormItem>
			</FormItem>
		</Form>
	);
}

export default SensorTypesForm;
