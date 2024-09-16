// Import Devextreme components
import { formatDate } from "devextreme/localization";

// Import custom tools
import { DatePatterns } from "./consts";

export const dateRender = ({ value }: { value: string }) => {
	return <div>{formatDate(new Date(value), DatePatterns.LongDateTimeYearSmall)}</div>;
};

// Array of objects used to populate the header filter options for the Activated column
export const activatedHeaderFilter = [
	{
		text: "Activated",
		value: ["Activated", "=", true]
	},
	{
		text: "Inactivated",
		value: ["Activated", "=", false]
	}
];

// Array of objects used to populate the header filter options for the Active column
export const activeHeaderFilter = [
	{
		text: "Active",
		value: ["Active", "=", true]
	},
	{
		text: "Inactive",
		value: ["Active", "=", false]
	}
];

// Array of objects used to populate the header filter options for the Activated column
export const enabledHeaderFilter = [
	{
		text: "Enabled",
		value: ["Enabled", "=", true]
	},
	{
		text: "Disabled",
		value: ["Enabled", "=", false]
	}
];

// Array of objects used to populate the header filter options for the Visible column
export const visibleHeaderFilter = [
	{
		text: "Visible",
		value: ["Visible", "=", true]
	},
	{
		text: "Not Visible",
		value: ["Visible", "=", false]
	}
];
