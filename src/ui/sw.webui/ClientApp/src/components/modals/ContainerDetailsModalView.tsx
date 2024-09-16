// Import React hooks
import { useState } from "react";

// Import Devextreme components
import { Popup } from "devextreme-react/popup";
import Box, { Item } from "devextreme-react/box";

// Import custom components
import ContainerReportInfo from "../containers/ContainerReportInfo";
import ContainerTimelineView from "../containers/tabs/ContainerTimeline";
import ContainerLog from "../containers/tabs/ContainerLog";

import "../../styles/containers/ContainerDetailsModal.scss";

function ContainerDetailsModalView({ popupVisible, hidePopup, selectedMapItem, selectedMapItemHistory }) {
	const [popupShown, setPopupShown] = useState(false);

	return (
		<Popup
			visible={popupVisible}
			onHiding={hidePopup}
			onShown={() => setPopupShown(true)}
			onHidden={() => setPopupShown(false)}
			style={{ padding: 0 }}
			animation={undefined}
			width="80%"
			minWidth={1100}
			height="90%"
			minHeight={600}
			dragEnabled={false}
			closeOnOutsideClick={true}
			showCloseButton={true}
			showTitle={true}
			title={selectedMapItem?.Name}
		>
			<Box direction="col" width="100%" height="100%">
				<Item baseSize={190}>
					<ContainerReportInfo selectedContainer={selectedMapItem} />
				</Item>
				<Item ratio={1}>
					<Box direction="row" width="100%" height="100%">
						<Item ratio={1}>
							<ContainerTimelineView popupShown={popupShown} selectedContainer={selectedMapItem} selectedMapItemHistory={selectedMapItemHistory} />
						</Item>
						<Item ratio={1}>
							<ContainerLog popupShown={popupShown} selectedMapItemHistory={selectedMapItemHistory} />
						</Item>
					</Box>
				</Item>
			</Box>
		</Popup>
	);
}

export default ContainerDetailsModalView;
