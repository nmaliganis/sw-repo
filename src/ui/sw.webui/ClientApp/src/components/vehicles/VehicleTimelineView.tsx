import React from "react";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faCircle, faExclamation, faFlag, faFlagCheckered, faTimes } from "@fortawesome/free-solid-svg-icons";

import List from "devextreme-react/list";

import "./VehicleTimeline.scss";

const TimelineItemRender = (item: { Name: string; Status: number; Location: string; RecordedDate: string }) => {
	let backgroundColor = "#ffa243";
	let icon = faExclamation;

	const statusIcon = () => {
		switch (item.Status) {
			case 0: //Start
				backgroundColor = "#FFAAAA";
				icon = faFlag;
				break;
			case 1: //Not Completed
				backgroundColor = "#CDF6FF";
				icon = faCircle;
				break;
			case 2: //Completed
				backgroundColor = "#00D1FF";
				icon = faCheck;
				break;
			case 3: //Skipped
				backgroundColor = "#CCC";
				icon = faTimes;
				break;
			case 4: //Other
				backgroundColor = "#FFA243";
				icon = faExclamation;

				break;
			case 5: //Finished
				backgroundColor = "#73DF70";
				icon = faFlagCheckered;

				break;
			default:
				backgroundColor = "#FFA243";
				icon = faExclamation;

				break;
		}

		return <FontAwesomeIcon className="timeline-status-icon" style={{ backgroundColor: backgroundColor }} icon={icon} />;
	};

	return (
		<table className="timeline-item">
			<tbody>
				<tr>
					<td>{statusIcon()}</td>
					<td className="timeline-name" style={{ fontSize: 18, fontWeight: 600 }}>
						{item.Name}
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						{item.Location} - {item.RecordedDate}
					</td>
				</tr>
			</tbody>
		</table>
	);
};

function VehicleTimelineView() {
	return (
		<div style={{ height: "100%", padding: 10 }}>
			<h3 style={{ borderBottom: "1px solid #e0e0e8" }}>Timeline</h3>
			<div className="timeline-container">
				<List dataSource={[]} itemRender={TimelineItemRender} style={{ flexGrow: 1 }} />
			</div>
		</div>
	);
}

export default VehicleTimelineView;
