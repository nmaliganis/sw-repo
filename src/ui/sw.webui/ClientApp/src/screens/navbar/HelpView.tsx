// Import React hooks
import { useState, useCallback, forwardRef, useImperativeHandle } from "react";

// Import Dexextreme components
import { Popup } from "devextreme-react/popup";
import DataGrid, { Column, MasterDetail, Scrolling } from "devextreme-react/data-grid";

// Import app version from packageJson
import packageJson from "../../../package.json";

// TODO: Ask business on what will be below
const dataFAQ = [
	{
		Id: 1,
		Question: "Question 1 that goes on and on and on and on and on and on and on and on and on and on",
		Answer: "Answer 1 that goes on and on and on and on and on and on and on and on and on and on and on and on and on and on and on and on"
	},
	{
		Id: 2,
		Question: "Question 2",
		Answer: "Answer 2"
	},
	{
		Id: 3,
		Question: "Question 3",
		Answer: "Answer 3"
	},
	{
		Id: 4,
		Question: "Question 4",
		Answer: "Answer 4"
	}
];

const renderQuestion = ({ value }) => {
	return <div className="question-answer-text">{value}</div>;
};

// Function that renders what will be rendered as an answer
const renderAnswer = ({ data }) => {
	return <p className="question-answer-text">{data.data.Answer}</p>;
};

const HelpView = forwardRef((props, ref) => {
	// State that handles popup visibilty
	const [helpPopupVisible, setHelpPopupVisible] = useState(false);

	// Ref Handler for showing the Popup
	useImperativeHandle(
		ref,
		() => {
			return {
				show() {
					setHelpPopupVisible(true);
				}
			};
		},
		[]
	);

	// Function to handle popup hiding
	const onHiding = useCallback(() => {
		setHelpPopupVisible(false);
	}, []);

	return (
		<Popup visible={helpPopupVisible} onHiding={onHiding} dragEnabled={false} closeOnOutsideClick={true} showCloseButton={true} showTitle={true} title="Help Center" width={400} height={600}>
			<div className="logo-container">
				<img
					src={
						"data:image/png;base64,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"
					}
					alt="logo"
					style={{ padding: 5 }}
				/>
				<p>Version: {packageJson.version}</p>
			</div>

			<DataGrid width="100%" height={385} dataSource={dataFAQ} keyExpr="Id" showBorders={true}>
				<Scrolling mode="virtual" rowRenderingMode="virtual" />
				<Column dataField="Question" caption="FAQ" cellRender={renderQuestion} />
				<MasterDetail enabled={true} component={renderAnswer} />
			</DataGrid>
		</Popup>
	);
});

export default HelpView;
