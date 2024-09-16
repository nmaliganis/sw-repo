// Import DevExtreme components
import { Form } from "devextreme-react/data-grid";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";

const isActiveOptions = {
	text: "Active"
};

const isVisibleOptions = {
	text: "Visible"
};

function SensorsForm() {
	return (
		<Form>
			<FormItem itemType="group" colCount={2} colSpan={2}>
				<FormItem dataField="AssetId">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="DeviceId">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="SensorTypeId">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="Name">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="IsActive" editorType="dxCheckBox" editorOptions={isActiveOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="IsVisible" editorType="dxCheckBox" editorOptions={isVisibleOptions}>
					<Label visible={false} />
				</FormItem>
				<FormItem dataField="Order" editorType="dxNumberBox" />
				<FormItem dataField="samplingInterval">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="ReportingInterval">
					<RequiredRule />
				</FormItem>
				<FormItem dataField="CodeErp">
					<RequiredRule />
				</FormItem>
				<FormItem itemType="group" caption="Min - Max Value" colCount={2} colSpan={1}>
					<FormItem dataField="MinValue" editorType="dxNumberBox">
						<Label visible={false} />
					</FormItem>
					<FormItem dataField="MaxValue" editorType="dxNumberBox">
						<Label visible={false} />
					</FormItem>
				</FormItem>
				<FormItem itemType="group" caption="Min - Max Notify Value" colCount={2} colSpan={1}>
					<FormItem dataField="MinNotifyValue" editorType="dxNumberBox">
						<Label visible={false} />
					</FormItem>
					<FormItem dataField="MaxNotifyValue" editorType="dxNumberBox">
						<Label visible={false} />
					</FormItem>
				</FormItem>
				<FormItem itemType="group" caption="Low - High Threshold" colCount={2} colSpan={1}>
					<FormItem dataField="LowThreshold" editorType="dxNumberBox">
						<Label visible={false} />
					</FormItem>
					<FormItem dataField="HighThreshold" editorType="dxNumberBox">
						<Label visible={false} />
					</FormItem>
				</FormItem>
			</FormItem>
		</Form>
	);
}

export default SensorsForm;
