// Import React hooks
import { useEffect, useRef } from "react";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";
import Toolbar, { Item as ToolbarItem } from "devextreme-react/toolbar";
import DataGrid, { Column, Scrolling, Sorting } from "devextreme-react/data-grid";

// import custom tools
import { dateRenderer } from "../../../utils/containerUtils";

const excelButtonOptions = {
	icon: "exportxlsx",
	type: "normal",
	stylingMode: "text",
	hint: "Export to Excel"
};

const printButtonOptions = {
	icon: "print",
	type: "normal",
	stylingMode: "text",
	hint: "Print"
};

const emailButtonOptions = {
	icon: "email",
	type: "normal",
	stylingMode: "text",
	hint: "Send to e-mail"
};

const levelRenderer = ({ value }: { value: string | number }) => {
	return `${value} %`;
};

// TODO: Remove if unnecessary
// const tempRenderer = ({ value }: { value: string | number }) => {
// 	if (value) return `${Math.round((value as number) * 10) / 10} °C`;

// 	return "";
// };

// const distanceRenderer = ({ value }: { value: string | number }) => {
// 	if (value) return `${value} mm`;

// 	return "";
// };

function ContainerLog({ popupShown, selectedMapItemHistory }: { popupShown?: boolean; selectedMapItemHistory: any[] }) {
	const dataGridRef = useRef<any>(null);

	// Refresh the DataGrid when popup is shown. Workaround to avoid wrong width, height when pop up is shown
	useEffect(() => {
		if (popupShown) dataGridRef.current?.instance.refresh();
	}, [popupShown]);

	return (
		<Box direction="col" width="100%" height="100%">
			<Item ratio={1}>
				<DataGrid ref={dataGridRef} dataSource={selectedMapItemHistory} width="100%" height="100%" keyExpr="Id" allowColumnReordering={true} allowColumnResizing={true} showBorders={true}>
					<Sorting mode="single" />
					<Scrolling mode="virtual" />

					<Column dataField="EventValue" caption={"Level"} alignment="left" cellRender={levelRenderer} />
					<Column dataField="IsWastePickUp" caption={"Waste Pick Up"} dataType="boolean" width={120} allowFiltering={false} allowResizing={false} />
					<Column dataField="Recorded" caption={"Recorded"} cellRender={dateRenderer} width={120} allowFiltering={false} allowResizing={false} />
				</DataGrid>
			</Item>
			<Item baseSize="auto">
				<Toolbar style={{ backgroundColor: "#03a9f4", paddingLeft: 5 }}>
					<ToolbarItem visible={false} cssClass="excel-footer-icon" location="before" widget="dxButton" options={excelButtonOptions} />
					<ToolbarItem visible={false} cssClass="print-footer-icon" location="before" widget="dxButton" options={printButtonOptions} />
					<ToolbarItem visible={false} cssClass="email-footer-icon" location="before" widget="dxButton" options={emailButtonOptions} />
				</Toolbar>
			</Item>
		</Box>
	);
}

export default ContainerLog;
