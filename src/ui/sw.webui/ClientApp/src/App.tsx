// Import React hooks
import { useState, useEffect } from "react";

// Import Redux action creators
import { useDispatch } from "react-redux";
import { setIsUserLoggedIn, UserVariableSetter } from "./redux/slices/loginSlice";

// Import React Router components, constants and custom components
import { HashRouter, Route, Routes } from "react-router-dom";
import { basePrefix } from "./utils/consts";
import RequireAuth from "./utils/RequireAuth";

// Import DevExtreme components and localization
import * as elMessages from "devextreme/localization/messages/el.json";
import { locale, loadMessages } from "devextreme/localization";
import { LoadIndicator } from "devextreme-react/load-indicator";
import transformedResources from "./utils/formatLocales";

// Import custom components
import { LoginView, PageNotFoundView, RootView, DashboardView, ContainersView, AdminView, NotificationsView, ItinerariesRootView, ItinerariesView, TemplateView, RestrictionsView, SpecialDaysView, ReportsView } from "./screens";

// Import style sheets
import "devextreme/dist/css/dx.material.blue.light.compact.css";
// import "./styles/dx.material.sw-theme.css";
import "./styles/App.scss";
import { getAssetCategories } from "./utils/apis/assets";
import { getCompanies, getRoles } from "./utils/apis/auth";

// Define the main component of the application
function App() {
	// Define the state of the loading indicator
	const [loadVisible, setLoadVisible] = useState(true);

	const dispatch = useDispatch();

	// Get the user data from the local storage and update the Redux store
	useEffect(() => {
		const localeLang = localStorage.getItem("lng");

		// Set the locale and the messages for DevExtreme components
		locale(localeLang ? localeLang : "en");
		loadMessages(elMessages);
		loadMessages(transformedResources);

		(async () => {
			const jwtToken = localStorage.getItem("jwtToken");

			if (jwtToken) {
				// Set up user rights according to the returning callback
				const userObject = {
					UserName: localStorage.getItem("Username") || "",
					Token: jwtToken,
					RefreshToken: localStorage.getItem("refreshToken") || "",
					UserParams: {
						AssetCategories: await getAssetCategories(),
						Companies: await getCompanies(),
						Roles: await getRoles(),
						RefreshInterval: 60000
					}
				};

				dispatch(UserVariableSetter(userObject));

				// Login setter
				dispatch(setIsUserLoggedIn(true));
			}

			// Hide the loading indicator
			setLoadVisible(false);
		})();
	}, [dispatch]);

	// Render the application structure based on the URL routing
	if (loadVisible) {
		// Render the loading indicator
		return (
			<div className="center-content" style={{ width: "100%", height: "100%" }}>
				<LoadIndicator id="large-indicator" height={80} width={80} style={{ textAlign: "center" }} />
			</div>
		);
	} else
		return (
			<>
				{/* TODO: Change to BrowserRouter */}
				<HashRouter>
					<Routes>
						<Route path={`/`} element={<LoginView />} />
						<Route path={`login`} element={<LoginView />} />
						<Route path={`${basePrefix}/login`} element={<LoginView />} />
						<Route path={basePrefix} element={<RootView />}>
							<Route
								path="dashboard"
								element={
									<RequireAuth redirectTo={`${basePrefix}/login`}>
										<DashboardView />
									</RequireAuth>
								}
							/>
							<Route
								path="containers"
								element={
									<RequireAuth redirectTo={`${basePrefix}/login`}>
										<ContainersView />
									</RequireAuth>
								}
							/>
							<Route
								path="admin"
								element={
									<RequireAuth redirectTo={`${basePrefix}/login`}>
										<AdminView />
									</RequireAuth>
								}
							/>
							<Route
								path="itineraries"
								element={
									<RequireAuth redirectTo={`${basePrefix}/login`}>
										<ItinerariesRootView />
									</RequireAuth>
								}
							>
								<Route index element={<ItinerariesView />} />
								<Route path="template" element={<TemplateView />} />
								<Route path="restrictions" element={<RestrictionsView />} />
								<Route path="special-days" element={<SpecialDaysView />} />
							</Route>
							<Route
								path="reports"
								element={
									<RequireAuth redirectTo={`${basePrefix}/login`}>
										<ReportsView />
									</RequireAuth>
								}
							/>
							<Route
								path="notifications"
								element={
									<RequireAuth redirectTo={`${basePrefix}/login`}>
										<NotificationsView />
									</RequireAuth>
								}
							/>
						</Route>
						<Route path="/*" element={<PageNotFoundView />} />
					</Routes>
				</HashRouter>
			</>
		);
}

export default App;
