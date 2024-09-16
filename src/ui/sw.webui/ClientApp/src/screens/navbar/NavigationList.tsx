// Import React hooks
import { useState, useEffect, useMemo, useCallback, useRef } from "react";

// import Fontawesome icons
import { faBell, faFileInvoice, faMapMarked, faRoad, faSignOutAlt } from "@fortawesome/free-solid-svg-icons";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { setSelectedItemKey } from "../../redux/slices/navBarSlice";
import { setIsUserLoggedIn } from "../../redux/slices/loginSlice";

// Import routing utils
import { useNavigate } from "react-router-dom";
import { basePrefix } from "../../utils/consts";

// Import DevExtreme components
import { Popover, ToolbarItem, Position, Offset } from "devextreme-react/popover";
import Box, { Item } from "devextreme-react/box";
import TreeView from "devextreme-react/tree-view";
import List from "devextreme-react/list";

// Import custom tools
import { navBarItemRenderer, NotificationListTemplate } from "../../utils/navUtils";

// Import custom components
// import WebSocketClient from "../../components/websocket/WebSocketClient";

import "../../styles/NavigationList.scss";
import HelpView from "./HelpView";
import UserInfoView from "./UserInfoView";

const popOverContainer = { class: "popover-container" };

// Array of objects holding indicators for navigation menu
const navigation: any = [
	{
		id: "0",
		name: "Dashboard",
		icon: faMapMarked,
		url: "/dashboard"
	},
	{
		id: "1",
		name: "Container",
		icon: "box",
		url: "/containers"
	},
	{
		id: "2",
		name: "Admin",
		icon: "card",
		url: "/admin"
	},
	{
		id: "3",
		name: "Itineraries",
		icon: faRoad,
		url: "/itineraries",
		items: [
			{
				id: "3_1",
				name: "Template",
				icon: "",
				url: "/itineraries/template"
			},
			{
				id: "3_2",
				name: "Restrictions",
				icon: "",
				url: "/itineraries/restrictions"
			},
			{
				id: "3_3",
				name: "Special Days",
				icon: "",
				url: "/itineraries/special-days"
			}
		]
	},
	{
		id: "4",
		name: "Reports",
		icon: faFileInvoice,
		url: "/reports"
	},
	{
		id: "5",
		name: "Notifications",
		icon: "",
		url: "/notifications",
		visible: false
	}
	// {
	// 	id: 3,
	// 	head_id: 2,
	// 	name: "Container types",
	// 	url: "/container-types"
	// },
	// {
	// 	id: 4,
	// 	head_id: 2,
	// 	name: "Content types",
	// 	url: "/content-types"
	// }
];

// Boilerplate data for notifications tab
const latestUpdate = [
	{
		Id: 1,
		Name: "Notification 1",
		Description: "Some text in here that goes and goes and goes and goes",
		Timestamp: "2022-05-26T13:45:30"
	},
	{
		Id: 2,
		Name: "Notification 2",
		Description: "Some text in here",
		Timestamp: "2022-05-25T13:45:30"
	},
	{
		Id: 3,
		Name: "Notification 3",
		Description: "Some text in here",
		Timestamp: "2022-04-15T13:45:30"
	},
	{
		Id: 4,
		Name: "Notification 4",
		Description: "Some text in here",
		Timestamp: "2021-05-15T13:45:30"
	},
	{
		Id: 5,
		Name: "Notification 5",
		Description: "Some text in here",
		Timestamp: "2021-05-15T13:45:30"
	},
	{
		Id: 6,
		Name: "Notification 6",
		Description: "Some text in here",
		Timestamp: "2009-06-15T13:45:30"
	},
	{
		Id: 7,
		Name: "Notification 7",
		Description: "Some text in here",
		Timestamp: "2009-06-15T13:45:30"
	},
	{
		Id: 8,
		Name: "Notification 8",
		Description: "Some text in here",
		Timestamp: "2009-06-15T13:45:30"
	}
];

function NavigationList() {
	// States that handle visibility of navbar and notification popup
	const [sideBar, setSideBar] = useState({ extended: false, extendable: false });
	const [notificationPopoverVisible, setNotificationPopoverVisible] = useState(false);

	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedItemKey } = useSelector((state: any) => state.navbar);

	const navigate = useNavigate();

	const dispatch = useDispatch();

	const userInfoRef = useRef<any>();
	const helpPopupRef = useRef<any>();

	// Array of objects to visualize items on user menu
	const userMenu = useMemo(
		() => [
			{
				id: 0,
				icon: faBell,
				name: "Notifications",
				badge: 5,
				visible: false
			},
			{
				id: 1,
				icon: "user",
				name: "Account"
			},
			{
				id: 2,
				icon: "help",
				name: "Help Center"
			},
			{
				id: 3,
				icon: faSignOutAlt,
				name: "Logout"
			}
		],
		[]
	);

	// Function that updates store and navigates route to the selected item
	const onSelectedNavChange = useCallback(
		(e) => {
			const selectedItem = e.itemData;

			dispatch(setSelectedItemKey(selectedItem.id));

			localStorage.setItem("currentswNavKey", selectedItem.id);

			navigate(`${basePrefix}${selectedItem.url}`);
		},
		[dispatch, navigate]
	);

	const onNavItemExpanded = useCallback((e) => {
		setSideBar({ extended: true, extendable: true });
	}, []);

	const onNavItemCollapsed = useCallback((e) => {
		setSideBar({ extended: false, extendable: false });
	}, []);

	// Objects for popover settings and actions
	const popoverSeeAllButton = useMemo(
		() => ({
			text: "See all",
			onClick: () => {
				const notificationKey = "3";

				dispatch(setSelectedItemKey(notificationKey));

				navigate(`${basePrefix}${navigation[notificationKey].url}`);
			}
		}),
		[dispatch, navigate]
	);

	// Function that handles actions on user menu
	const onUserItemClick = useCallback(
		(e: any) => {
			switch (e.itemData.id) {
				case 0:
					setNotificationPopoverVisible(true);
					break;
				case 1:
					// Open Account window

					userInfoRef.current.show();

					break;
				case 2:
					helpPopupRef.current.show();
					break;
				case 3:
					localStorage.removeItem("jwtToken");
					// Logout user
					dispatch(setIsUserLoggedIn(false));
					break;
				default:
					break;
			}
		},
		[dispatch]
	);

	// Update local storage when selected view is changed
	useEffect(() => {
		const currentNavKey = localStorage.getItem("currentswNavKey");

		if (currentNavKey) {
			dispatch(setSelectedItemKey(currentNavKey));

			// When user selects item in extended stretch
			setSideBar((state) => ({ ...state, extended: currentNavKey.includes("_") }));
		}
	}, [dispatch]);

	return (
		<aside className={`sidebar ${sideBar.extended ? "open" : ""}`} data-sidebar>
			<div style={{ width: "100%", padding: "14px 17px 0px" }}>
				<button
					className="sidebar-icon-btn"
					data-menu-icon-btn
					onClick={() => {
						setSideBar((state) => ({ ...state, extended: !state.extended }));
					}}
					disabled={sideBar.extendable}
				>
					<svg viewBox="0 0 24 24" preserveAspectRatio="xMidYMid meet" focusable="false" className="sidebar-icon">
						<g>
							<path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"></path>
						</g>
					</svg>
				</button>
			</div>
			<hr />
			<div className="middle-sidebar">
				<Box direction="col" width="100%" height="100%">
					<Item baseSize="auto">
						<div className="top-sidebar">
							<div className="channel-logo">
								<img src="https://etrack.sw.gr/etrack/AppIcons/Logo.png" alt="Logo" />
							</div>
						</div>
					</Item>
					<Item ratio={1}>
						<hr />
						<TreeView
							className="navigation-tree"
							height="100%"
							items={navigation}
							keyExpr="id"
							selectByClick={true}
							animationEnabled={false}
							selectionMode="single"
							showCheckBoxesMode="none"
							selectedItem={selectedItemKey}
							onItemClick={onSelectedNavChange}
							onItemExpanded={onNavItemExpanded}
							onItemCollapsed={onNavItemCollapsed}
							itemRender={navBarItemRenderer}
						/>
					</Item>
					<Item baseSize="auto">
						{/* TODO: Show when web socket works */}
						{/* <WebSocketClient /> */}
						<hr />
						<List dataSource={userMenu} width="100%" height="100%" itemRender={navBarItemRenderer} onItemClick={onUserItemClick} />
						<UserInfoView ref={userInfoRef} />
						<HelpView ref={helpPopupRef} />
					</Item>
				</Box>
			</div>
			<Popover
				wrapperAttr={popOverContainer}
				showTitle={true}
				closeOnOutsideClick={true}
				width={300}
				height={450}
				visible={notificationPopoverVisible}
				onHiding={() => {
					setNotificationPopoverVisible(false);
				}}
			>
				<ToolbarItem text="Notifications" location="before"></ToolbarItem>
				<ToolbarItem widget="dxButton" location="after" options={popoverSeeAllButton}></ToolbarItem>
				<Position my="left" at="right" of="#notification-tab">
					<Offset x={30} y={-60} />
				</Position>
				<Box direction="col" width="100%" height="100%">
					<Item ratio={1}>
						<div className="notification-list-container">
							<List dataSource={latestUpdate} height="100%" itemRender={NotificationListTemplate} style={{ flexGrow: 1 }} />
						</div>
					</Item>
					<Item baseSize="auto">
						<hr />
						<div style={{ textAlign: "center" }}>
							<b>{latestUpdate.length} New Notifications</b>
						</div>
					</Item>
				</Box>
			</Popover>
		</aside>
	);
}

export default NavigationList;
