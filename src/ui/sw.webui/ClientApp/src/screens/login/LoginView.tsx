// Import React hooks
import { useState, useRef } from "react";

// Import routing utils
import { useNavigate } from "react-router-dom";
import { basePrefix } from "../../utils/consts";

// Import Redux action creators
import { useDispatch } from "react-redux";
import { setIsUserLoggedIn, UserVariableSetter } from "../../redux/slices/loginSlice";

//Import Devextreme components
import notify from "devextreme/ui/notify";
import { Popup } from "devextreme-react/popup";
import { Button } from "devextreme-react/button";
import { formatMessage } from "devextreme/localization";
import LoadIndicator from "devextreme-react/load-indicator";
import { Toolbar, Item as ToolbarItem } from "devextreme-react/toolbar";
import MultiView, { Item as MultiItem } from "devextreme-react/multi-view";
import { DropDownButton, Item as DropDownButtonItem } from "devextreme-react/drop-down-button";
import { Form, GroupItem, SimpleItem, Label, RequiredRule, ButtonItem, ButtonOptions } from "devextreme-react/form";

// Import custom tools
import { getCompanies, getRoles, postUserAuth } from "../../utils/apis/auth";
import { getAssetCategories } from "../../utils/apis/assets";

// Import custom components
import ForgotPasswordView from "./ForgotPasswordView";

import "../../styles/login/Login.scss";

function LoginView() {
	const [selectedView, setSelectedView] = useState<any>(0);
	const [loadingState, setLoadingState] = useState(false);

	let navigate = useNavigate();

	const dispatch = useDispatch();

	const formRef = useRef(null);
	let formData = { username: "", password: "", remember: false, SelectView: { text: "Grid Only", value: 1 } };

	// Function to change the selected language
	const changeLocale = (item: string) => {
		localStorage.setItem("lng", item);
		window.location.reload();
	};

	// Function to handle the view changes
	const onViewChanged = (viewIndex: number) => {
		setSelectedView(viewIndex);
	};

	// Function to handle changes in the form fields
	const onFieldDataChanged = (e: { component: { option: (arg0: string) => { username: string; password: string; remember: boolean; SelectView: { text: string; value: number } } } }): void => {
		formData = e.component.option("formData");
	};

	// Function to handle form submission
	async function onSubmit(e: { preventDefault: () => void }) {
		e.preventDefault();

		setLoadingState(true);

		if (formData.remember) {
			localStorage.setItem("userName", formData.username);
		}

		// Authenticating user credentials
		const authData: {
			Token: string;
			RefreshToken: string;
			UserData: any;
		} = await postUserAuth(formData);

		if (!authData?.Token) {
			// Setting loading state to false and showing error message
			setLoadingState(false);
			notify("Something went wrong. Please try again later.", "error", 2500);
		} else {
			// Set token to local storage
			localStorage.setItem("jwtToken", "Bearer " + authData.Token);
			localStorage.setItem("refreshToken", authData.RefreshToken);

			localStorage.setItem("Username", formData.username);

			// Set up user rights according to the returning callback
			const userObject = {
				UserName: formData.username,
				Token: "Bearer " + authData.Token,
				RefreshToken: authData.RefreshToken,
				UserParams: {
					AssetCategories: await getAssetCategories(),
					Companies: await getCompanies(),
					Roles: await getRoles(),
					RefreshInterval: 60000
				}
			};

			dispatch(UserVariableSetter(userObject));

			// Login setter
			dispatch(setIsUserLoggedIn(true));

			navigate(`${basePrefix}/dashboard`);
		}
	}

	return (
		<div className="image-background">
			<Popup visible={true} dragEnabled={false} closeOnOutsideClick={false} showCloseButton={false} showTitle={false} width={380} height="auto">
				<Toolbar>
					<ToolbarItem location="after">
						<DropDownButton elementAttr={{ "data-testid": "login-language-dropdown" }} text={formatMessage("Language")} width={150} displayExpr="name" keyExpr="id" stylingMode="text" hoverStateEnabled={true}>
							<DropDownButtonItem
								onClick={() => {
									changeLocale("en");
								}}
								icon="globe"
							>
								{formatMessage("English")}
							</DropDownButtonItem>
							<DropDownButtonItem
								onClick={() => {
									changeLocale("el");
								}}
								icon="globe"
							>
								{formatMessage("Greek")}
							</DropDownButtonItem>
						</DropDownButton>
					</ToolbarItem>
				</Toolbar>
				<div style={{ display: "flex", justifyContent: "center" }}>
					<img
						src={
							"data:image/png;base64,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"
						}
						alt="logo"
						style={{ padding: 5 }}
					/>
				</div>
				<MultiView selectedIndex={selectedView} height="100%" loop={false} swipeEnabled={false} animationEnabled={true}>
					<MultiItem key={0}>
						<form action="login-action" onSubmit={onSubmit}>
							<Form ref={formRef} readOnly={false} labelLocation="top" showOptionalMark={false} showColonAfterLabel={false} showValidationSummary={true} onFieldDataChanged={onFieldDataChanged}>
								<SimpleItem dataField="username" editorType="dxTextBox" editorOptions={{ placeholder: formatMessage("Username"), inputAttr: { "data-testid": "login-username" } }}>
									<Label visible={false} />
									<RequiredRule message={formatMessage("UsernameRequired")} />
								</SimpleItem>
								<SimpleItem
									dataField="password"
									editorType="dxTextBox"
									editorOptions={{
										mode: "password",
										placeholder: formatMessage("Password"),
										inputAttr: { "data-testid": "login-password" }
									}}
								>
									<Label visible={false} />
									<RequiredRule message={formatMessage("PasswordRequired")} />
								</SimpleItem>
								<GroupItem colCount={2}>
									<SimpleItem
										dataField="remember"
										editorType="dxCheckBox"
										editorOptions={{
											text: formatMessage("StayConnected"),
											value: false,
											inputAttr: { "data-testid": "login-checkbox" }
										}}
									>
										<Label visible={false} />
									</SimpleItem>
									<SimpleItem>
										<Button onClick={() => onViewChanged(1)} elementAttr={{ "data-testid": "login-button-ForgotPassword" }} type="default" stylingMode="text" style={{ height: 24, float: "right" }}>
											{formatMessage("ForgotPassword")}
										</Button>
									</SimpleItem>
								</GroupItem>

								<ButtonItem horizontalAlignment="left">
									<ButtonOptions elementAttr={{ "data-testid": "login-button-signin" }} width="100%" type="default" useSubmitBehavior={true} disabled={loadingState}>
										<LoadIndicator className="button-indicator" visible={loadingState} height={20} width={20} />
										<span className="dx-button-text">{formatMessage("SignIn")}</span>
									</ButtonOptions>
								</ButtonItem>
							</Form>
						</form>
					</MultiItem>
					<MultiItem key={1}>
						<ForgotPasswordView onViewChanged={onViewChanged} />
					</MultiItem>
				</MultiView>
			</Popup>
		</div>
	);
}

export default LoginView;
