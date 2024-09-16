// Import React hooks
import { useState, useCallback, useEffect, useRef } from "react";

// Import Redux action creators
import { setSelectedMapItemHistory } from "../../../redux/slices/modalSlice";
import { useDispatch } from "react-redux";

// Import Devextreme components
import Button from "devextreme-react/button";
import DateBox from "devextreme-react/date-box";
import Toolbar, { Item as ToolbarItem } from "devextreme-react/toolbar";
import { Chart, Series, ArgumentAxis, CommonAxisSettings, Grid, Export, Legend, Margin, Tooltip, Label, ValueAxis, ConstantLine, ZoomAndPan } from "devextreme-react/chart";
import { formatDate } from "devextreme/localization";

// import custom tools
import { DatePatterns } from "../../../utils/consts";
import { getEventHistoryData } from "../../../utils/apis/assets";
import { ContainerModelT } from "../../../utils/types";

const customizeYAxisText = ({ valueText }: { valueText: number | string }) => {
	return `${valueText} %`;
};

const customizeXAxisText = ({ value }: { value: string | number }) => {
	return `${formatDate(new Date(value), DatePatterns.LongDateTimeYearSmall)}`;
};

const customTooltipRender = (pointInfo) => {
	return (
		<div style={{ display: "flex", flexDirection: "column", gap: "0.5rem", padding: "0.5rem" }}>
			<div>Level {pointInfo.valueText} %</div>
			<div>Recorded: {formatDate(new Date(pointInfo.originalArgument), DatePatterns.LongDateTimeYearSmall)}</div>
		</div>
	);
};

const customizePoint = (e) => {
	if (e.data?.IsWastePickUp) {
		return {
			image: {
				url: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAAcCAQAAAA+LdxbAAAACXBIWXMAAACdAAAAnQGPcuduAAAFF2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDUgNzkuMTYzNDk5LCAyMDE4LzA4LzEzLTE2OjQwOjIyICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxOSAoV2luZG93cykiIHhtcDpDcmVhdGVEYXRlPSIyMDIzLTA3LTA1VDIxOjQzOjAxKzAzOjAwIiB4bXA6TW9kaWZ5RGF0ZT0iMjAyMy0wNy0wNVQyMTo0Mzo0MSswMzowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAyMy0wNy0wNVQyMTo0Mzo0MSswMzowMCIgZGM6Zm9ybWF0PSJpbWFnZS9wbmciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjEiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJEb3QgR2FpbiAyMCUiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6YTEyZWFhMDctYTBhMi1mODQ4LTgwMjAtMWJhMjEzNjJmM2NlIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOmExMmVhYTA3LWEwYTItZjg0OC04MDIwLTFiYTIxMzYyZjNjZSIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOmExMmVhYTA3LWEwYTItZjg0OC04MDIwLTFiYTIxMzYyZjNjZSI+IDx4bXBNTTpIaXN0b3J5PiA8cmRmOlNlcT4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNyZWF0ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTEyZWFhMDctYTBhMi1mODQ4LTgwMjAtMWJhMjEzNjJmM2NlIiBzdEV2dDp3aGVuPSIyMDIzLTA3LTA1VDIxOjQzOjAxKzAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxOSAoV2luZG93cykiLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+DZHLWwAAASZJREFUOI3VlDtLA0EUhb99QUIgYGnsjIWQ0iCIIj4QIZ0gio3/IYV1mlT+gGCrjSBYWigWqSwkP0CwMiZC2o1LgpFdm4noPNbZ0jvNnXPvOfdwBwbkaJEQERIS8k7CudzgUKZCJG4DVjlTROrcUhJ5gSd4Icl0ei4zimp6FF2GGSmRwyLz9AHHoj2mRC/jCIT2Eh0mjP/sDcixSdsHPCAgsBrhgZvdmIkyoEnXTFvWPNgGUCVW8G3TlF3aQIc1W2N73InsgR0bYwdStSYbkynH2qkplBu9DS5/UvxfpSoNPpjjkQsADlmnj6OuQV3yvahc2y85Nf4ZZYp6uqKvA1nhlC6zbJkonwqa48Tg6rv32fIPeyM/NbbPFQuMiA3a4JLnlSNG8AXEM5/COmc37gAAAABJRU5ErkJggg==",
				width: 17,
				height: 20
			},
			visible: true
		};
	}

	return e;
};

function ContainerTimelineView({ popupShown, selectedContainer, selectedMapItemHistory }: { popupShown?: boolean; selectedContainer: ContainerModelT; selectedMapItemHistory: any[] }) {
	// State that handles date range on timeline
	const [dateFrom, setDateFrom] = useState(new Date(new Date().setDate(new Date().getDate() - 1)));
	const [dateTo, setDateTo] = useState(new Date());

	const chartRef = useRef<Chart | any>();

	const dispatch = useDispatch();

	const updateItemHistory = useCallback(
		(startDate, endDate) => {
			(async () => {
				const historyData = await getEventHistoryData({ selectedItem: selectedContainer.Id, startDate: startDate, endDate: endDate });

				dispatch(setSelectedMapItemHistory(historyData));
			})();
		},
		[dispatch, selectedContainer.Id]
	);

	// Function that handles start time range
	const onDateFromChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value);

			if (newDate < dateTo) {
				setDateFrom(newDate);
				updateItemHistory(newDate, dateTo);
			}
		},
		[dateTo, updateItemHistory]
	);

	// Function that handles end time range
	const onDateToChanged = useCallback(
		(e) => {
			const newDate = new Date(e.value);

			if (newDate > dateFrom) {
				setDateTo(newDate);
				updateItemHistory(dateFrom, newDate);
			}
		},
		[dateFrom, updateItemHistory]
	);

	// Function that changes event history data in accordance to selected time range
	const onSetTimeRange = useCallback(
		(selectedTime) => {
			let startDate = new Date();
			let endDate = new Date();

			switch (selectedTime) {
				case "Year":
					startDate.setFullYear(endDate.getFullYear() - 1);
					break;
				case "Month":
					startDate.setMonth(endDate.getMonth() - 1);
					break;
				case "Week":
					startDate.setDate(endDate.getDate() - 7);
					break;
				case "Day":
				default:
					startDate.setDate(endDate.getDate() - 1);
					break;
			}

			setDateFrom(startDate);
			setDateTo(endDate);

			updateItemHistory(startDate, endDate);
		},
		[updateItemHistory]
	);

	// Work-around to show chart correctly on popup
	useEffect(() => {
		if (popupShown) chartRef.current?.instance.render();
	}, [popupShown]);

	// Get chart reference and reset view
	const resetZoom = useCallback(() => {
		chartRef.current.instance.resetVisualRange();
	}, [chartRef]);

	// Update chart when data changes
	useEffect(() => {
		if (chartRef.current?.instance) chartRef.current?.instance.render();
	}, [selectedMapItemHistory]);

	// Reset date range when selected container loaded
	useEffect(() => {
		setDateFrom(new Date(new Date().setDate(new Date().getDate() - 1)));
		setDateTo(new Date());
	}, [selectedContainer.Id]);

	return (
		<div className="container-timeline-container">
			<Toolbar style={{ padding: 10 }}>
				<ToolbarItem location="before">
					<div className="date-range-picker">
						<DateBox width={120} stylingMode="underlined" type="date" value={dateFrom} onValueChanged={onDateFromChanged} max={dateTo} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
						<i className="dx-icon-minus" style={{ display: "flex", alignItems: "center" }} />
						<DateBox width={120} stylingMode="underlined" type="date" value={dateTo} onValueChanged={onDateToChanged} min={dateFrom} displayFormat={DatePatterns.LongDate} pickerType="calendar" />
					</div>
				</ToolbarItem>
				<ToolbarItem location="before">
					<Button text="Day" type="default" stylingMode="text" focusStateEnabled onClick={() => onSetTimeRange("Day")} />
				</ToolbarItem>
				<ToolbarItem location="before">
					<Button text="Week" type="default" stylingMode="text" focusStateEnabled onClick={() => onSetTimeRange("Week")} />
				</ToolbarItem>
				<ToolbarItem location="before">
					<Button text="Month" type="default" stylingMode="text" focusStateEnabled onClick={() => onSetTimeRange("Month")} />
				</ToolbarItem>
				<ToolbarItem location="after">
					<Button text="Reset" type="normal" stylingMode="text" onClick={resetZoom} />
				</ToolbarItem>
			</Toolbar>
			<Chart id="chart" ref={chartRef} dataSource={selectedMapItemHistory} customizePoint={customizePoint}>
				<Series valueField="EventValue" argumentField="Recorded" name={selectedContainer.Name} color="#03a9f4" type="spline" />
				<ValueAxis discreteAxisDivisionMode="crossLabels" name="primary" position="left" visualRange={[0, 95]}>
					<Label customizeText={customizeYAxisText} />
					<ConstantLine width="2" value={20} color="#8c8cff" dashStyle="dash">
						<Label text="Min" />
					</ConstantLine>
					<ConstantLine width="2" value={80} color="#ff1717" dashStyle="dash">
						<Label text="Max" />
					</ConstantLine>
				</ValueAxis>
				<ArgumentAxis minValueMargin={0.1} maxValueMargin={0.5}>
					<Label customizeText={customizeXAxisText} overlappingBehavior="stagger"></Label>
				</ArgumentAxis>
				<ZoomAndPan argumentAxis="both" dragToZoom={true} allowMouseWheel={false} />
				<CommonAxisSettings>
					<Grid visible={true} />
				</CommonAxisSettings>
				<Margin bottom={20} />
				<Legend visible={false} />
				<Export enabled={false} />
				<Tooltip enabled={true} contentRender={customTooltipRender} zIndex={50000} />
			</Chart>
		</div>
	);
}

export default ContainerTimelineView;
