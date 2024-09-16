// Import React hooks
import { useState, useCallback, useMemo, useRef, forwardRef, useImperativeHandle } from "react";

// Redux selector
import { useSelector } from "react-redux";

// Import Devextreme components
import { Popup } from "devextreme-react/popup";
import Form, { Item, GroupItem, RequiredRule, ButtonItem } from "devextreme-react/form";

// Object for description editor settings
const descriptionEditorOptions = { height: 50, maxLength: 200 };

// Object for button update settings
const buttonOptions = {
	text: "Update",
	type: "default",
	useSubmitBehavior: true
};

const UserInfoView = forwardRef((props, ref) => {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	// State that handles user popup visibility
	const [userPopupVisible, setUserPopupVisible] = useState(false);

	const formRef = useRef<any>();

	// TODO: Remove Email and Description when they return from user data
	const formData = useMemo(() => ({ Email: "", Description: "", ...userData }), [userData]);

	// Ref Handler for showing the Popup
	useImperativeHandle(
		ref,
		() => {
			return {
				show() {
					setUserPopupVisible(true);
				}
			};
		},
		[]
	);

	// Function to handle popup hiding
	const onHiding = useCallback(() => {
		setUserPopupVisible(false);
	}, []);

	// Update password visibility state
	const changePasswordMode = (name) => {
		const editor = formRef.current.instance.getEditor(name);
		editor.option("mode", editor.option("mode") === "text" ? "password" : "text");
	};

	// On User submit send data for validation and update user accordingly
	const handleUserSubmit = useCallback(
		(e) => {
			// Prevent page reload
			e.preventDefault();

			// TODO: Add endpoint to update user data and validators
			console.log(formData);
		},
		[formData]
	);

	const passwordEditorOptions = useMemo(
		() => ({
			mode: "password",
			showClearButton: true,
			buttons: [
				{
					name: "password",
					location: "after",
					options: {
						icon: "https://js.devexpress.com/Demos/WidgetsGallery/JSDemos/images/icons/eye.png",
						type: "default",
						onClick: () => changePasswordMode("Password")
					}
				}
			]
		}),
		[]
	);

	return (
		<Popup visible={userPopupVisible} onHiding={onHiding} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={true} showTitle={true} title="User Info" width={400} height={380}>
			<div className="logo-container">
				<i className="dx-icon-user" style={{ fontSize: 72, color: "#3d3d3d" }} />
				<div>{userData.UserName}</div>
			</div>
			<form action="form-user" onSubmit={handleUserSubmit}>
				<Form ref={formRef} formData={formData}>
					<GroupItem colCount={2}>
						<Item dataField="Email">
							<RequiredRule message="Email is required" />
						</Item>
						<Item dataField="Password" editorOptions={passwordEditorOptions}>
							<RequiredRule message="Password is required" />
						</Item>
						<Item dataField="Description" colSpan={2} editorType="dxTextArea" editorOptions={descriptionEditorOptions}></Item>
					</GroupItem>

					<ButtonItem horizontalAlignment="right" buttonOptions={buttonOptions} />
				</Form>
			</form>
		</Popup>
	);
});

export default UserInfoView;
