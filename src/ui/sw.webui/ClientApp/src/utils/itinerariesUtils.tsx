import { occurrenceType } from "./consts";

export const ContainersRender = ({ data }) => {
	if (data?.AssignedContainers?.length) {
		const servicedContainers = data.AssignedContainers.reduce((accumulator, currentValue) => (currentValue.IsServiced === true ? accumulator + 1 : accumulator), 0);

		return (
			<div>
				{servicedContainers}/{data?.AssignedContainers?.length}
			</div>
		);
	}

	return "-";
};

export const DriverRender = ({ data }) => {
	return <div>{data.Driver?.Name}</div>;
};

export const VehicleRender = ({ data }) => {
	return <div>{data.Vehicle?.Name}</div>;
};

export const startEndLocationRender = ({ column, data }) => {
	if (data?.Locations.length) {
		const selectedItem = data.Locations.find((item) => item.IsStart === (column.dataField === "StartLocation" ? true : false));

		return selectedItem.Location.Name;
	}

	return "";
};

export const zoneContainersRender = ({ data }) => {
	if (data.Zones) return JSON.parse(data.Zones).Containers.length;
	return "";
};

export const occurrenceRender = ({ value }) => {
	if (value)
		return JSON.parse(value)
			.Occurrence.map((item) => occurrenceType[item - 1].ShortName)
			.join(" - ");

	return "";
};

export const streamRender = ({ value }) => {
	return value;
};
