// import Fontawesome components
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const relativeTimePeriods = [
	[31536000, "year"],
	[2419200, "month"],
	[604800, "week"],
	[86400, "day"],
	[3600, "hour"],
	[60, "minute"],
	[1, "second"]
];

// Calculate time like Youtube. e.g: 5 minutes ago, 1 hour ago
export function relativeTime(date: any) {
	if (!(date instanceof Date)) date = new Date(date * 1000);

	const seconds = (Number(new Date()) - date) / 1000;

	for (let [secondsPer, name] of relativeTimePeriods) {
		if (seconds >= Number(secondsPer)) {
			const amount = Math.floor(seconds / Number(secondsPer));

			return `${amount} ${name}${amount !== 1 ? "s" : ""} ago`;
		}
	}
	return "Just now";
}

export function NotificationListTemplate(item: { Name: string; Description: string | number; Timestamp: string }) {
	return (
		<div className="latest-activity-item">
			<div className="item-title">{item.Name}</div>
			<div className="item-description">{item.Description}</div>
			<p className="item-timestamp">{relativeTime(new Date(item.Timestamp))}</p>
		</div>
	);
}

export const navBarItemRenderer = (item: any) => {
	return (
		<div className="vertical-center-content" style={{ paddingLeft: item?.url ? 0 : 6 }} id={item.badge ? "notification-tab" : ""}>
			{typeof item.icon === "string" ? <i className={`sidebar-icon dx-icon dx-icon-${item.icon}`}></i> : <FontAwesomeIcon className="sidebar-icon dx-icon" icon={item.icon} />}

			<div className="hidden-sidebar" style={{ marginLeft: "1rem" }}>
				{item.name}
			</div>
		</div>
	);
};
