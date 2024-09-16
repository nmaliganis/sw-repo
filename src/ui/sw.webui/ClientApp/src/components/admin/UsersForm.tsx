// Import React hooks
import { useState, useCallback, useMemo, useRef, useEffect } from "react";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";
import { Form, SimpleItem, GroupItem, EmailRule, Label, RequiredRule, PatternRule, StringLengthRule, ButtonItem } from "devextreme-react/form";
import Validator from "devextreme-react/validator";
import { SelectBox } from "devextreme-react/select-box";
import TextBox from "devextreme-react/text-box";
import "devextreme-react/tag-box";

// Import custom tools
import { countryCodesObject } from "../../utils/consts";
import { getDepartments } from "../../utils/apis/auth";

export const userGenders = [
	{ Id: 0, Name: "Male" },
	{ Id: 1, Name: "Female" }
];

const maskRules = { X: /[01-9]/ };

// Regex for password filter
const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z\d]{8,}$/;

// Regex for address filter
const addressCodePattern = /^[a-z0-9][a-z0-9\- ]{0,10}[a-z0-9]$/;

function UsersForm({ formData, popupMode, dispatchPopup, dataSource, userData }: any) {
	const [departmentsData, setDepartmentsData] = useState([]);

	const formRef = useRef<any>();

	// Object options for select companies
	const companySelectBoxOptions = useMemo(() => {
		return {
			valueExpr: "Id",
			displayExpr: "Name",
			items: userData.UserParams.Companies
		};
	}, [userData.UserParams.Companies]);

	// Object options for select departments
	const departmentsTagBoxOptions = useMemo(() => {
		return {
			dataSource: departmentsData,
			displayExpr: "Name",
			valueExpr: "Id",
			showSelectionControls: true,
			applyValueMode: "useButtons"
		};
	}, [departmentsData]);

	// Object options for select roles
	const rolesTagBoxOptions = useMemo(() => {
		return {
			dataSource: userData.UserParams.Roles,
			displayExpr: "Name",
			valueExpr: "Id",
			showSelectionControls: true,
			applyValueMode: "useButtons"
		};
	}, [userData.UserParams.Roles]);

	// Object options for select gender
	const genderSelectOptions = useMemo(() => {
		return {
			dataSource: userGenders,
			displayExpr: "Name",
			valueExpr: "Id"
		};
	}, []);

	// Renderer for for custom phone form
	const renderPhone = (data: { editorOptions: { value: any } }) => {
		const onSelectExtPhoneChanged = (e: any) => {
			formRef.current.instance.updateData("ExtPhone", e.value);
		};

		const onPhoneTextChange = (e: any) => {
			formRef.current.instance.updateData("Phone", e.value);
		};

		return (
			<Box direction="row" width="100%">
				<Item baseSize={100}>
					<SelectBox searchEnabled={true} searchMode="contains" dataSource={countryCodesObject} onValueChanged={onSelectExtPhoneChanged}>
						<Validator validationGroup="userForm">
							<RequiredRule message="Code is required" />
						</Validator>
					</SelectBox>
				</Item>
				<Item ratio={1}>
					<TextBox mask="X00 0000000" maskRules={maskRules} onValueChanged={onPhoneTextChange} defaultValue={data.editorOptions.value}>
						<Validator validationGroup="userForm">
							<RequiredRule message="Phone is required" />
						</Validator>
					</TextBox>
				</Item>
			</Box>
		);
	};

	// Renderer for for custom mobile form
	const renderMobile = (data: { editorOptions: { value: any } }) => {
		const onSelectExtMobileChanged = (e: any) => {
			formRef.current.instance.updateData("ExtMobile", e.value);
		};

		const onMobileTextChange = (e: any) => {
			formRef.current.instance.updateData("Mobile", e.value);
		};

		return (
			<Box direction="row" width="100%">
				<Item baseSize={100}>
					<SelectBox searchEnabled={true} searchMode="contains" dataSource={countryCodesObject} onValueChanged={onSelectExtMobileChanged}>
						<Validator validationGroup="userForm">
							<RequiredRule message="Code is required" />
						</Validator>
					</SelectBox>
				</Item>
				<Item ratio={1}>
					<TextBox mask="X00 0000000" maskRules={maskRules} onValueChanged={onMobileTextChange} defaultValue={data.editorOptions.value}>
						<Validator validationGroup="userForm">
							<RequiredRule message="Mobile is required" />
						</Validator>
					</TextBox>
				</Item>
			</Box>
		);
	};

	// Function that handles form validation
	const confirmClick = useCallback(
		(e) => {
			const result = formRef.current?.instance.validate();

			if (result.isValid) {
				if (popupMode === "Add") {
					dataSource.insert(formData);
				} else if (popupMode === "Edit") {
					dataSource.push(formData["Id"], formData);
				}

				dispatchPopup({ type: "hidePopup" });
			} else {
				formRef.current?.instance.validate();
			}
		},
		[formData, popupMode, dataSource, dispatchPopup]
	);

	// Object for previous button settings. Memoization prevents flickering
	const cancelBtnOptions = useMemo(() => {
		return {
			text: "CANCEL",
			type: "normal",
			onClick: () => {
				dispatchPopup({ type: "hidePopup" });
			}
		};
	}, [dispatchPopup]);

	// Object for confirm button settings. Memoization prevents flickering
	const saveBtnOptions = useMemo(() => {
		return {
			text: "SAVE",
			type: "default",
			onClick: confirmClick
		};
	}, [confirmClick]);

	// Initialize departments data
	useEffect(() => {
		(async () => {
			const data = await getDepartments();

			setDepartmentsData(data);
		})();
	}, []);

	return (
		<Form ref={formRef} formData={formData} labelLocation="top" showColonAfterLabel={true} validationGroup="userForm">
			<GroupItem itemType="group" colCount={2} colSpan={2}>
				<GroupItem caption="User" itemType="group" colCount={2} colSpan={1}>
					<SimpleItem dataField="Login">
						<RequiredRule message="Login is required" />
					</SimpleItem>
					<SimpleItem dataField="Password">
						<RequiredRule message="Password is required" />
						<StringLengthRule min={8} max={16} message="Password must have a minimum length of 8-16." />
						<PatternRule message="No number, uppercase and lowercase letter" pattern={passwordPattern} />
					</SimpleItem>
					<SimpleItem dataField="RolesIds" editorType="dxTagBox" editorOptions={rolesTagBoxOptions}>
						<Label text="Roles" />
						<RequiredRule message="Roles is required" />
					</SimpleItem>
					<SimpleItem dataField="Email">
						<EmailRule message="Email is invalid" />
						<RequiredRule message="Email is required" />
					</SimpleItem>
					<SimpleItem dataField="Company" editorType="dxSelectBox" editorOptions={companySelectBoxOptions}>
						<Label text="Company" />
						<RequiredRule message="Company is required" />
					</SimpleItem>
					<SimpleItem dataField="DepartmentsIds" editorType="dxTagBox" editorOptions={departmentsTagBoxOptions}>
						<Label text="Departments" />
						<RequiredRule message="Departments are required" />
					</SimpleItem>
				</GroupItem>
				<GroupItem caption="Personal Information" itemType="group" colCount={2} colSpan={1}>
					<SimpleItem dataField="Firstname">
						<RequiredRule message="Firstname is required" />
					</SimpleItem>
					<SimpleItem dataField="Lastname">
						<RequiredRule message="Lastname is required" />
					</SimpleItem>
					<SimpleItem dataField="Gender" editorType="dxSelectBox" editorOptions={genderSelectOptions}>
						<RequiredRule message="Gender is required" />
					</SimpleItem>
					<SimpleItem dataField="CodeErp">
						<RequiredRule message="CodeErp is required" />
					</SimpleItem>
					<SimpleItem dataField="Phone" render={renderPhone}>
						<Label text="Phone" />
					</SimpleItem>
					<SimpleItem dataField="Mobile" render={renderMobile}>
						<Label text="Mobile" />
						<RequiredRule message="Mobile is required" />
					</SimpleItem>
				</GroupItem>
				<GroupItem caption="Address" itemType="group" colCount={2} colSpan={2}>
					<SimpleItem dataField="Street">
						<Label text="Street" />
						<RequiredRule message="Street is required" />
					</SimpleItem>
					<GroupItem itemType="group" colCount={2} colSpan={2}>
						<SimpleItem dataField="StreetNumber" editorType="dxNumberBox">
							<Label text="Street Number" />
						</SimpleItem>
						<SimpleItem dataField="AddressPostCode">
							<Label text="Post Code" />
							<RequiredRule message="AddressPostCode is required" />
							<PatternRule message="Invalid address code." pattern={addressCodePattern} />
						</SimpleItem>
					</GroupItem>
					<SimpleItem dataField="AddressCity">
						<Label text="Adress City" />
						<RequiredRule message="AddressCity is required" />
					</SimpleItem>
					<SimpleItem dataField="AddressRegion">
						<Label text="Region" />
					</SimpleItem>
				</GroupItem>

				<SimpleItem dataField="Notes" editorType="dxTextArea" colSpan={2}>
					<Label text="Description" />
				</SimpleItem>
			</GroupItem>
			<GroupItem itemType="group" colCount={2} colSpan={1}>
				<ButtonItem cssClass="previous-button-container" buttonOptions={cancelBtnOptions} />
				<ButtonItem buttonOptions={saveBtnOptions} />
			</GroupItem>
		</Form>
	);
}

export default UsersForm;
