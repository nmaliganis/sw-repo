import DropDownButton from "devextreme-react/drop-down-button";

// Array of objects to visualize export options
const generateSettings = [
	{ Id: 1, Name: "EXPORT TO PDF", icon: "pdffile" },
	{ Id: 2, Name: "EXPORT TO EXCEL", icon: "xlsfile" },
	{ Id: 3, Name: "SEND TO MAIL", icon: "email" }
];

const buttonDropDownOptions = { width: 200 };

// Component that allows the user to have a dropdown button for generating reports
export const GenerateDropDownButton = ({ isLoading = false, onItemClick, onButtonClick }) => {
	return (
		<DropDownButton
			style={{ marginLeft: "auto" }}
			stylingMode="outlined"
			dropDownOptions={buttonDropDownOptions}
			splitButton={true}
			useSelectMode={false}
			disabled={isLoading}
			text={isLoading ? "LOADING..." : "GENERATE"}
			items={generateSettings}
			keyExpr="Id"
			displayExpr="Name"
			onItemClick={onItemClick}
			onButtonClick={onButtonClick}
		/>
	);
};
