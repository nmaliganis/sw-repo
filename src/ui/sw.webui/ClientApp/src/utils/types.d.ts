export interface loginDataObjectT {
	UserName: string;
	Token: string;
	RefreshToken: string;
	UserParams: {
		AssetCategories: any[];
		Companies: any[];
		Roles: any[];
		RefreshInterval: number;
	};
}

export interface StateLoginT {
	isUserLoggedIn: boolean;
	userData: loginDataObjectT;
}

export interface StateNavBarT {
	selectedItemKey: string;
}

export interface ContainerModelT {
	AssetCategoryId: number | string;
	BinStatus: number;
	Capacity: number | string;
	CodeErp: number | string;
	CompanyId: number | string;
	Description: string;
	Id: number | string;
	Image: string;
	IsVisible: boolean;
	LastServicedDate: string;
	LastUpdated: string;
	Latitude: number;
	Level: number;
	Longitude: number;
	MandatoryPickupActive: true;
	MandatoryPickupDate: string;
	Material: number;
	Message: string;
	Name: string;
	Status: number | string;
	TimeToFull: number | string;
	WasteType: number;
}

export interface SelectedContainerT {
	BrokenRules: any[];
	ExtraProp: null;
	Model: ContainerModelT | any;
	Status: number;
}

export interface StateContainerT {
	containersData: any[];
	// totalContainers: number;
	// totalVolume: number;
	// timeToFull: String;
	totalTrash: number;
	totalRecycle: number;
	totalCompost: number;
	selectedContainer: ContainerModelT | any;
	availableZones: ZoneI[];
	selectedZones: any[];
	selectedFilterBinStatus: any[];
	selectedStreamFilters: number[];
}

export interface InitialDataT {
	ContentTypes: any[];
	DeclaredType: any;
	Formatters: any[];
	StatusCode: any;
	Value: any[];
}

export interface latestActivityUltraSonicT {
	Id: number;
	Imei: string;
	Range: number;
	Level: number;
	Recorded: string;
	Status: number;
	Temperature: number;
}

export interface latestActivityGPST {
	Id: number;
	Imei: string;
	Recorded: string;
	Latitude: double;
	Longitude: double;
	Altitude: double;
	Speed: double;
	Direction: double;
	FixMode: number;
	Hdop: double;
	SatellitesUsed: number;
}

export interface latestActivityDigitalT {
	Id: number;
	Imei: string;
	Recorded: dateTime;
	pinNumber: number;
	newValue: number;
}

export interface StateDashboardT {
	mapData: any[];
	mapTotalTrash: number;
	mapTotalRecycle: number;
	mapTotalCompost: number;
	mapDataFilter: string[];
	selectedStreamFilters: number[];
	selectedMapItem: ContainerModelT | null;
	selectedZones: any[] | null;
}

export interface SelectedVehicleT {
	AssetCategoryId: number;
	Axels: number;
	Brand: string;
	CodeErp: string;
	CompanyId: number;
	Description: string;
	Gas: number;
	Height: number;
	Id: number;
	Image: string;
	Length: number;
	Message: string | null;
	MinTurnRadius: number;
	Name: string;
	NumPlate: string;
	RegisteredDate: string;
	Status: number;
	Type: number;
	Width: number;
}

export interface StateModalT {
	popupDetailsVisible: boolean;
	selectedMapItem: ContainerModelT | any;
	selectedMapItemHistory: any[];
}

export interface StateActivityT {
	latestActivityData: Array<latestActivityUltraSonicT | latestActivityGPST | latestActivityDigitalT>;
}

export interface MapCenterT {
	center: { Latitude: number; Longitude: number };
	changeBounds?: boolean;
}

export interface InitialStateVehicleT {
	vehiclesData: any[];
	selectedVehicle: {} | SelectedVehicleT;
}

export interface ZoneI {
	Id: number;
	Name: string;
	Positions: [number, number][];
}
