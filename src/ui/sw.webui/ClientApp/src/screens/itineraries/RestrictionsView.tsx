// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import { RestrictionsTable, RestrictionsMap } from "../../components/itineraries";

function RestrictionsView() {
	return (
		<Box direction="row" width="100%" height="100%">
			<Item ratio={1}>
				<RestrictionsTable />
			</Item>
			<Item ratio={1}>
				<RestrictionsMap />
			</Item>
		</Box>
	);
}

export default RestrictionsView;
