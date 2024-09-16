// Import React hooks
import { useEffect } from "react";

// Import Redux action creators
import { useDispatch } from "react-redux";
import { setStartEndPoints, setZonesByCompany, setDrivers, setVehicles } from "../../redux/slices/itinerarySlice";

// Import custom tools
import { getVehicles, getZonesByCompany } from "../../utils/apis/assets";
import { getStartEndLocations } from "../../utils/apis/routing";
import { getDrivers } from "../../utils/apis/auth";

// Import react-router-dom handler for multiple views
import { Outlet } from "react-router-dom";

function ItinerariesRootView() {
	const dispatch = useDispatch();

	// Fetch data from the API to be used on all routes
	useEffect(() => {
		(async () => {
			const dataStartEnd = await getStartEndLocations();
			const dataZones = await getZonesByCompany();
			const dataVehicles = await getVehicles();
			const dataDrivers = await getDrivers();

			dispatch(setStartEndPoints(dataStartEnd));
			dispatch(setZonesByCompany(dataZones));
			dispatch(setVehicles(dataVehicles));
			dispatch(setDrivers(dataDrivers));
		})();
	}, [dispatch]);

	return <Outlet />;
}

export default ItinerariesRootView;
