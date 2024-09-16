import React from "react";

// Import Redux action creators
import { useDispatch, useSelector } from "react-redux";
import { hidePopupDetails } from "../redux/slices/modalSlice";

import { Outlet } from "react-router-dom";

import NavigationList from "./navbar/NavigationList";

import ContainerDetailsModalView from "../components/modals/ContainerDetailsModalView";

// Define the RootView component which renders the NavigationList, main content (using the React-Router Outlet), and a modal view for container details
function RootView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { popupDetailsVisible, selectedMapItem, selectedMapItemHistory } = useSelector((state: any) => state.modal);

	const dispatch = useDispatch();

	// Render the RootView component, including the NavigationList and main content, with an error handler and a modal view for container details
	return (
		<>
			<div className="root-container" style={{ display: "flex" }}>
				<NavigationList />
				<main className="main-content">
					<Outlet />
				</main>
			</div>
			<ContainerDetailsModalView
				popupVisible={popupDetailsVisible}
				hidePopup={() => {
					dispatch(hidePopupDetails());
				}}
				selectedMapItem={selectedMapItem}
				selectedMapItemHistory={selectedMapItemHistory}
			/>
		</>
	);
}

export default RootView;
