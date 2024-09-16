// Import React hooks
import { useState, useEffect, useCallback } from "react";

// Import Redux action creators
import { useSelector, useDispatch } from "react-redux";
import { setSelectedZones } from "../../redux/slices/containerSlice";

// Import Devextreme components
import TagBox from "devextreme-react/tag-box";

// Import custom tools
import { ZoneI } from "../../utils/types";
import { getZonesByCompany } from "../../utils/apis/assets";

function TemplateToolbar() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedZones } = useSelector((state: any) => state.container);

	const [availableZones, setAvailableZones] = useState<ZoneI[]>([]);

	const dispatch = useDispatch();

	// Update store with the selected Zone
	const onZoneSelectionChange = useCallback(
		({ value }: any) => {
			dispatch(setSelectedZones(value));
		},
		[dispatch]
	);

	// Get zone date from the API
	useEffect(() => {
		(async () => {
			const data = await getZonesByCompany();

			if (data.length) {
				dispatch(setSelectedZones([data[0] as never]));
				setAvailableZones(data);
			}
		})();
	}, [dispatch]);

	return (
		<div>
			<TagBox
				disabled={availableZones.length === 0}
				dataSource={availableZones}
				value={selectedZones}
				onValueChanged={onZoneSelectionChange}
				displayExpr="Name"
				placeholder="Select Zones..."
				maxFilterQueryLength={900000}
				searchEnabled={true}
				showSelectionControls={true}
				multiline={false}
				applyValueMode="useButtons"
			/>
		</div>
	);
}

export default TemplateToolbar;
