import countryCodes from "country-codes-list";

export const initMapCenter = [37.98381, 23.727539];

export const basePrefix = "/sw";

export const defaultLogo = "https://etrack.sw.gr/eadmin/AppIcons/Logo.png";

export const DatePatterns = {
	LongDateText: "EEEE dd, MMMM yyyy",
	ShortTime: "HH:mm",
	ShortTimePeriod: "h:mm a",
	ShortDateTimeNoYear: "dd/MM HH:mm",
	LongDateTimeNoSeconds: "dd/MM/yyyy HH:mm",
	LongDateTime: "dd/MM/yyyy HH:mm:ss",
	LongDateTimeYearSmall: "dd/MM/yy HH:mm",
	LongDate: "dd/MM/yyyy",
	AltLongDate: "j/n/Y",
	ShortDate: "j/n/Y",
	AltShortDate: "j/n/Y",
	ShortDateNoYear: "j/n",
	AltShortDateNoYear: "j/n",
	LongTime: "G:i:s",
	AltLongDateTime: "j/n/Y H:i:s",
	ShortDateTime: "j/n/Y G:i",
	AltShortDateTime: "j/n/Y H:i",
	LongDateTimeNoYear: "j/n G:i:s",
	ISODateTime: "c",
	NoYearDateTime: "j/n G:i:s",
	LongTimeTwoDigitYear: "j/n/y G:i:s",
	ShortDateTimeTwoDigitYear: "j/n/y G:i",
	ShortDateTwoDigitYear: "j/n/y",
	ChartShortDateTime: "%d/%m/%Y %H:%M:%S"
};

export const searchFilterData = [
	{ Id: 0, Name: "Name" },
	{ Id: 1, Name: "Location" }
];

export const BucketStatus = [
	{ Id: 1, Name: "Normal" },
	{ Id: 2, Name: "BrokenLid" },
	{ Id: 3, Name: "Broken Bucket" },
	{ Id: 4, Name: "Damaged Pedal" },
	{ Id: 5, Name: "Damaged Wheel" },
	{ Id: 6, Name: "Burned" },
	{ Id: 7, Name: "Changed Position" },
	{ Id: 8, Name: "Missing" }
];

export const MaterialType = [
	{ Id: 1, Name: "HDPE" },
	{ Id: 2, Name: "Metallic" },
	{ Id: 3, Name: "Other" }
];

export const CapacityType = [
	{ Id: 1, Name: "80" },
	{ Id: 2, Name: "120" },
	{ Id: 3, Name: "240" },
	{ Id: 4, Name: "660" },
	{ Id: 5, Name: "1100" }
];

export const PickUpRateType = [
	{ Id: 1, Name: "Daily" },
	{ Id: 2, Name: "Weekly" },
	{ Id: 3, Name: "Weekend" }
];

export const occurrenceType = [
	{
		Id: 1,
		Name: "Monday",
		ShortName: "Mon"
	},
	{
		Id: 2,
		Name: "Tuesday",
		ShortName: "Tue"
	},
	{
		Id: 3,
		Name: "Wednesday",
		ShortName: "Wed"
	},
	{
		Id: 4,
		Name: "Thursday",
		ShortName: "Thu"
	},
	{
		Id: 5,
		Name: "Friday",
		ShortName: "Fri"
	},
	{
		Id: 6,
		Name: "Saturday",
		ShortName: "Sat"
	},
	{
		Id: 7,
		Name: "Sunday",
		ShortName: "Sun"
	}
];

export const streamType = [
	{ Id: 1, Name: "Composites" },
	{ Id: 2, Name: "Organics" },
	{ Id: 3, Name: "Recycle" }
];

export const colorPalette = [
	{ BinStatus: 0, Name: "OFFLINE", Color: "#777777" },
	{ BinStatus: 1, Name: "EMPTY", Color: "#73DF70" },
	{ BinStatus: 2, Name: "NORMAL", Color: "#FBD75A" },
	{ BinStatus: 3, Name: "FULL", Color: "#FF7070" },
	{ BinStatus: 4, Name: "NO DEVICE", Color: "#C894FF" }
];

export const colorBinStatus = (BinStatus: number) => {
	switch (BinStatus) {
		case 0:
		default:
			return colorPalette[0];
		case 1:
			return colorPalette[1];
		case 2:
			return colorPalette[2];
		case 3:
			return colorPalette[3];
		case 4:
			return colorPalette[4];
	}
};

//Initial country code prefixes (e.g: +30, +45...) turned into an array because the original structure is an object
// @ts-ignore: Argument of type '"countryCode"' is not assignable to parameter of type 'CountryProperty | undefined'.
export const countryCodesObject: string[] = Object.values(countryCodes.customList("countryCode", "+{countryCallingCode}") as any) as string[];
