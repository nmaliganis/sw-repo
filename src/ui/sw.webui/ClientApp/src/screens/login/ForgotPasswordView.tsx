// Import React hooks
import { useRef } from "react";

//Import Devextreme components
import { formatMessage } from "devextreme/localization";
import { Form, SimpleItem, Label, EmailRule, RequiredRule, ButtonItem, ButtonOptions } from "devextreme-react/form";
import { Button } from "devextreme-react/button";
import { postForgetPasswordInit } from "../../utils/apis/auth";

// Object holding the data being sent to the API
const formData = { email: "" };

function ForgotPasswordView({ onViewChanged }: { onViewChanged: (viewIndex: number) => void }) {
	const formRef = useRef(null);

	// Function that handles data verification and submission
	const onSubmit = (e: { preventDefault: () => void }): void => {
		e.preventDefault();

		postForgetPasswordInit(formData.email);
	};

	return (
		<form action="forgot-password" onSubmit={onSubmit}>
			<Form ref={formRef} formData={formData} readOnly={false} labelLocation="top" showOptionalMark={false} showColonAfterLabel={false} showValidationSummary={true}>
				<SimpleItem>
					<Button onClick={() => onViewChanged(0)} elementAttr={{ "data-testid": "login-button-Back" }} type="default" stylingMode="text" style={{ height: 24 }}>
						<div style={{ display: "flex", alignItems: "center" }}>
							<i className="dx-icon-chevronleft" style={{ marginRight: 4 }}></i>
							{formatMessage("Back")}
						</div>
					</Button>
				</SimpleItem>
				<SimpleItem dataField="email" editorType="dxTextBox" editorOptions={{ placeholder: formatMessage("Email"), inputAttr: { "data-testid": "login-email" } }}>
					<Label visible={false} />
					<EmailRule message={formatMessage("WrongEmails")} />
					<RequiredRule message={formatMessage("EmailRequired")} />
				</SimpleItem>

				<ButtonItem horizontalAlignment="left">
					<ButtonOptions elementAttr={{ "data-testid": "login-button-send" }} width="100%" text={formatMessage("Send")} type="default" useSubmitBehavior={true} />
				</ButtonItem>
			</Form>
		</form>
	);
}

export default ForgotPasswordView;
