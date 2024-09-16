import React, { useState } from "react";

// Import DevExtreme components
import TabPanel, { Item } from "devextreme-react/tab-panel";

// ImportFontAwesome components
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBuilding, faIdBadge, faMicrochip, faRss, faSuitcase, faUserNurse, faVectorSquare } from "@fortawesome/free-solid-svg-icons";

import { UsersView, RolesView, DevicesView, SensorsView, SensorTypesView, CompaniesView, DepartmentsView, VehiclesView, DriversView, ContainersView, ZonesView } from "./tabs";

import "../../styles/admin/Admin.scss";

const itemTitleRender = (item: { icon: any; title: boolean | React.ReactChild | React.ReactFragment | React.ReactPortal | null | undefined }) => {
	return (
		<>
			{typeof item.icon === "string" ? <i className={`dx-icon dx-icon-${item.icon}`}></i> : <FontAwesomeIcon className="dx-icon" icon={item.icon} />}
			<span>{item.title}</span>
		</>
	);
};

function AdminView() {
	const [selectedAdminTab, setSelectedAdminTab] = useState();

	const onSelectionChanged = (args: any) => {
		if (args.name === "selectedIndex") {
			setSelectedAdminTab(args.value);
		}
	};

	return (
		<TabPanel style={{ border: "1px solid #e0e0e0" }} width="100%" height="100%" selectedIndex={selectedAdminTab} onOptionChanged={onSelectionChanged} itemTitleRender={itemTitleRender} loop={false} animationEnabled={false} swipeEnabled={false}>
			<Item title="Companies" icon={faSuitcase} visible={true}>
				<CompaniesView />
			</Item>
			<Item title="Departments" icon={faBuilding}>
				<DepartmentsView />
			</Item>
			<Item title="Devices" icon={faMicrochip}>
				<DevicesView />
			</Item>
			<Item title="Sensors" icon={faRss}>
				<SensorsView />
			</Item>
			<Item title="Sensor Types" icon="toolbox">
				<SensorTypesView />
			</Item>
			<Item title="Vehicles" icon="car">
				<VehiclesView />
			</Item>
			<Item title="Drivers" icon={faUserNurse}>
				<DriversView />
			</Item>
			<Item title="Users" icon="group">
				<UsersView />
			</Item>
			<Item title="Roles" icon={faIdBadge}>
				<RolesView />
			</Item>
			<Item title="Containers" icon="box">
				<ContainersView />
			</Item>
			<Item title="Zones" icon={faVectorSquare}>
				<ZonesView />
			</Item>
		</TabPanel>
	);
}

export default AdminView;
