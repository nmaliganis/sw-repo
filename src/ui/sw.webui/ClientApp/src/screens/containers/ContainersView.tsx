// Import React hooks
import { useState, useEffect } from "react";

// Import Redux action creators
import { useDispatch } from "react-redux";
import { setSelectedContainer } from "../../redux/slices/containerSlice";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom components
import ContainersTableView from "./ContainersTableView";
import ContainerReportView from "./ContainerReportView";

function ContainersView() {
	const [multiEditMode, setMultiEditMode] = useState(false);

	const dispatch = useDispatch();

	// Clear selected container when ContainersView is initiated
	useEffect(() => {
		dispatch(setSelectedContainer(null));
	}, [dispatch]);

	return (
		<Box direction="row" width="100%" height="100%">
			<Item ratio={1}>
				<ContainersTableView multiEditMode={multiEditMode} setMultiEditMode={setMultiEditMode} />
			</Item>
			<Item ratio={1} visible={!multiEditMode}>
				<ContainerReportView />
			</Item>
		</Box>
	);
}

export default ContainersView;
