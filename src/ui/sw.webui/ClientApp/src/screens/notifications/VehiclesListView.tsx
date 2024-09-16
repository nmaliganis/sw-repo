import React, { useState, useEffect } from "react";

import { Button } from "devextreme-react/button";
import List from "devextreme-react/list";

import { relativeTime } from "../../utils/navUtils";

const latestUpdate = [
	{
		Id: 1,
		Name: "Notification 1",
		Description: "Some text in here that goes and goes and goes and goes",
		Timestamp: "2022-05-26T13:45:30",
		IsRead: false
	},
	{
		Id: 2,
		Name: "Notification 2",
		Description: "Some text in here",
		Timestamp: "2022-05-25T13:45:30",
		IsRead: false
	},
	{
		Id: 3,
		Name: "Notification 3",
		Description: "Some text in here",
		Timestamp: "2022-04-15T13:45:30",
		IsRead: false
	},
	{
		Id: 4,
		Name: "Notification 4",
		Description: "Some text in here",
		Timestamp: "2021-05-15T13:45:30",
		IsRead: false
	},
	{
		Id: 5,
		Name: "Notification 5",
		Description: "Some text in here",
		Timestamp: "2021-05-15T13:45:30",
		IsRead: true
	},
	{
		Id: 6,
		Name: "Notification 6",
		Description: "Some text in here",
		Timestamp: "2009-06-15T13:45:30",
		IsRead: true
	},
	{
		Id: 7,
		Name: "Notification 7",
		Description: "Some text in here",
		Timestamp: "2009-06-15T13:45:30",
		IsRead: true
	},
	{
		Id: 8,
		Name: "Notification 8",
		Description: "Some text in here",
		Timestamp: "2009-06-15T13:45:30",
		IsRead: true
	}
];

function VehiclesListView() {
	const [latestData, setLatestData] = useState<any[]>([]);

	const markAsRead = (id: number) => {
		console.log("Send request");
	};

	const VehiclesListTemplate = (item: { Id: number; Name: string; Description: string | number; Timestamp: string; IsRead: boolean }) => {
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
			<List dataSource={latestData} height="100%" itemRender={VehiclesListTemplate} style={{ flexGrow: 1 }} />
		</div>
	);
}

export default VehiclesListView;
