// Import React hooks
import { useState, useEffect } from "react";

// Import DevExtreme components
import { Button } from "devextreme-react/button";
import List from "devextreme-react/list";

// Import custom tools
import { relativeTime } from "../../utils/navUtils";

const latestUpdate: any = [];

function UsersListView() {
	const [latestData, setLatestData] = useState<any[]>([]);

	// Function to mark a notification as read
	const markAsRead = (id: number) => {
		console.log("Send request");
	};

	// Template for rendering a notification item in the list
	const UsersListTemplate = (item: { Id: number; Name: string; Description: string | number; Timestamp: string; IsRead: boolean }) => {
		return (
			<div className="list-item">
				<div className="item-title">{item.Name}</div>
				{!item.IsRead && (
					<Button className="item-mark-as-read" type="default" stylingMode="text" onClick={() => markAsRead(item.Id)}>
						<b>MARK AS READ</b>
					</Button>
				)}
				<div className="item-description">{item.Description}</div>
				<p className="item-timestamp">{relativeTime(new Date(item.Timestamp))}</p>
			</div>
		);
	};

	// Initialize state with latest notification data
	useEffect(() => {
		setLatestData(latestUpdate);
	}, []);

	if (!latestData.length)
		return (
			<div className="list-container" style={{ justifyContent: "center", alignItems: "center", width: 500 }}>
				<h3>No notifications</h3>
				<Button type="default" stylingMode="contained">
					RELOAD
				</Button>
			</div>
		);

	return (
		<div className="list-container">
			<List dataSource={latestData} height="100%" itemRender={UsersListTemplate} style={{ flexGrow: 1 }} />
		</div>
	);
}

export default UsersListView;
