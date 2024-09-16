// Import React hooks
import React, { useState, useCallback, useRef, forwardRef, useImperativeHandle } from "react";

// Import Devextreme components
import { formatDate } from "devextreme/localization";
import "devextreme-react/switch";

// Import rc-year-calendar
import Calendar from "rc-year-calendar";

// Import custom tools
import { DatePatterns } from "../../utils/consts";
import { specialDayT } from "../../utils/itineraries";

// Import custom components
import SpecialDaysForm from "./SpecialDaysForm";

import styles from "../../styles/itineraries/SpecialDays.module.scss";

// Component that renders Calendar items in a list
export const CalendarCardItem = React.memo(({ data }: { data: specialDayT }) => {
	return (
		<div className={styles["special-days-card-item"]} style={{ borderLeft: `4px solid ${data.color}` }}>
			<b>{data.name}</b>

			<div className={styles["special-days-card-item-description"]}>{data.description}</div>
			<div>
				{formatDate(new Date(data.startDate), DatePatterns.ShortDateTimeNoYear)} - {formatDate(new Date(data.endDate), DatePatterns.ShortDateTimeNoYear)}
			</div>
		</div>
	);
});

// Component that visualizes unique days on calendar
const CalendarCard = forwardRef((props, ref) => {
	// State that handles visualization of hovered day
	const [hoveredItem, setHoveredItem] = useState({ date: null, items: [] });

	// Ref actions passed to parents that enable hover actions
	useImperativeHandle(ref, () => ({
		onEnter: (props) => {
			setHoveredItem(props);
		},
		onLeave: () => {
			setHoveredItem({ date: null, items: [] });
		}
	}));

	if (hoveredItem.date)
		return (
			<div className={styles["special-days-card"]}>
				<div className={styles["special-days-card-title"]}>{formatDate(new Date(hoveredItem.date), DatePatterns.LongDateText)}</div>
				<div className={styles["special-days-card-content"]}>
					{hoveredItem.items.map((item: specialDayT, index) => (
						<CalendarCardItem key={index} data={item} />
					))}
				</div>
			</div>
		);

	return null;
});

function SpecialDaysCalendar({ dataSource, currentYear, setCurrentYear }: { dataSource: specialDayT[]; currentYear: number; setCurrentYear: React.Dispatch<React.SetStateAction<number>> }) {
	const calendarCarRef = useRef<any>();

	const CalendarFormRef = useRef<any>();

	// Function that handles hover actions on days with data
	const onDayHover = useCallback(({ date, item, mouseEvent }) => {
		if (item.length) {
			if (mouseEvent === "mouseenter") calendarCarRef.current.onEnter({ date: date, items: item });
			if (mouseEvent === "mouseleave") calendarCarRef.current.onLeave();
		}
	}, []);

	// Function that handles click actions on selected day/s
	const onDayClick = useCallback(({ date, events }) => {
		CalendarFormRef.current.setDateState({ startDate: date, endDate: date, items: events });
	}, []);

	// Function that handles range selection from calendar
	const onCalendarRangeSelected = useCallback(({ startDate, endDate }) => {
		if (new Date(endDate).getTime() !== new Date(startDate).getTime()) {
			CalendarFormRef.current.setDateState({ startDate: startDate, endDate: endDate, items: [] });
		}
	}, []);

	return (
		<div className={styles["special-days-container"]}>
			<Calendar
				language="en"
				year={currentYear}
				dataSource={dataSource}
				allowOverlap={true}
				enableContextMenu={false}
				enableRangeSelection={true}
				onDayEnter={({ date, events }) => onDayHover({ date: date, item: events, mouseEvent: "mouseenter" })}
				onDayLeave={({ date, events }) => onDayHover({ date: date, item: events, mouseEvent: "mouseleave" })}
				onYearChanged={(e) => setCurrentYear(e.currentYear)}
				onDayClick={onDayClick}
				onRangeSelected={onCalendarRangeSelected}
			/>
			<CalendarCard ref={calendarCarRef} />
			<SpecialDaysForm ref={CalendarFormRef} />
		</div>
	);
}

export default React.memo(SpecialDaysCalendar);
