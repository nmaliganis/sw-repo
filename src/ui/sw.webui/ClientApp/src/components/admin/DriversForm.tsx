import { useState, useEffect, useMemo } from "react";

// Import DevExtreme components
import { Form } from "devextreme-react/data-grid";
import { Item as FormItem, Label, RequiredRule } from "devextreme-react/form";
import "devextreme-react/tag-box";
import "devextreme-react/text-area";

// Import custom tools
import { getVehicles } from "../../utils/apis/assets";

function DriversForm(userData) {
	const [vehiclesData, setVehiclesData] = useState([]);

	const companyEditorOptions = useMemo(
		() => ({
			valueExpr: "Id",
			displayExpr: "Name",
			dataSource: userData?.UserParams?.Companies
		}),
		[userData]
	);

	const vehiclesEditorOptions = useMemo(() => {
		return {
			dataSource: vehiclesData,
			displayExpr: "Name",
			valueExpr: "Id",
			showSelectionControls: true,
			applyValueMode: "useButtons"
		};
	}, [vehiclesData]);

	useEffect(() => {
		(async () => {
			const data = await getVehicles();

			setVehiclesData(data);
		})();
	}, []);

	return (
		<Form>
			<FormItem itemType="group" colCount={2} colSpan={2}>
				<FormItem dataField="FirstName">
					<Label text="First name" />
					<RequiredRule />
				</FormItem>
				<FormItem dataField="LastName">
					<Label text="Last name" />
					<RequiredRule />
				</FormItem>
				<FormItem dataField="CompanyId" editorOptions={companyEditorOptions}>
					<Label text="Company" />
					<RequiredRule />
				</FormItem>
				<FormItem dataField="AssignedVehicles" editorType="dxTagBox" editorOptions={vehiclesEditorOptions}>
					<Label text="Vehicles" />
					<RequiredRule />
				</FormItem>
				<FormItem dataField="Description" editorType="dxTextArea" colSpan={2}>
					<Label text="Description" />
				</FormItem>
			</FormItem>
		</Form>
	);
}

export default DriversForm;
