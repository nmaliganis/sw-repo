// Import React hooks
import React, { useState, useEffect, useCallback, useRef } from "react";

// Import Devextreme components
import Button from "devextreme-react/button";
import { formatDate } from "devextreme/localization";
import PieChart, { Series, Legend, Animation, Connector, Label } from "devextreme-react/pie-chart";

// Import Fontawesome components
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDigitalTachograph, faMapMarked } from "@fortawesome/free-solid-svg-icons";

// Import custom tools
import { binStatusRenderer } from "./containerUtils";
import { colorPalette, DatePatterns } from "./consts";
import { ContainerModelT, latestActivityDigitalT, latestActivityGPST, latestActivityUltraSonicT } from "./types";

import "../styles/LineListHorizontal.scss";

// Function that handles visualization for each different type of latest activity record that comes
export function latestActivityItem(item: latestActivityDigitalT & latestActivityGPST & latestActivityUltraSonicT & ContainerModelT & any) {
	// Ultra Sonic
	// if ("Range" in item) {
	// 	return (
	// 		<div className="latest-activity-item">
	// 			<div className="latest-activity-item-icon">
	// 				<FontAwesomeIcon className="dx-icon" icon={faWifi} transform={{ rotate: 90 }} />
	// 			</div>
	// 			<div>
	// 				<div className="latest-activity-item-title">{item?.Imei}</div>
	// 				<div className="latest-activity-item-description">
	// 					<div>
	// 						Level: <span className="latest-activity-item-status">{item?.Level}</span> %
	// 					</div>
	// 					<div>
	// 						Temp: <span className="latest-activity-item-status">{Math.round(item?.Temperature * 10) / 10} </span> °C
	// 					</div>
	// 				</div>
	// 				<p className="latest-activity-item-timestamp">{formatDate(new Date(item?.Recorded), DatePatterns.LongDateTime)}</p>
	// 			</div>
	// 		</div>
	// 	);
	// }

	// Container
	if ("AssetCategoryId" in item) {
		return (
			<div className="latest-activity-item">
				<div className="latest-activity-item-icon">{binStatusRenderer({ value: item.BinStatus })}</div>
				<div>
					<div className="latest-activity-item-title">{item?.Name}</div>
					<div className="latest-activity-item-description">
						<div>
							Level: <span className="latest-activity-item-status">{item?.Level}</span> %
						</div>
					</div>
					<p className="latest-activity-item-timestamp">{formatDate(new Date(item?.LastServicedDate), DatePatterns.LongDateTime)}</p>
				</div>
			</div>
		);
	}

	// Digital
	if ("PinNumber" in item) {
		return (
			<div className="latest-activity-item">
				<div className="latest-activity-item-icon">
					<FontAwesomeIcon className="dx-icon" icon={faDigitalTachograph} />
				</div>
				<div>
					<div className="latest-activity-item-title">{item?.Imei}</div>
					<div className="latest-activity-item-description">
						<div>
							PIN: <span className="latest-activity-item-status">{item?.pinNumber}</span>
						</div>
						<div>
							Value: <span className="latest-activity-item-status">{item?.newValue}</span>
						</div>
					</div>
					<p className="latest-activity-item-timestamp">{formatDate(new Date(item?.Recorded), DatePatterns.LongDateTime)}</p>
				</div>
			</div>
		);
	}

	// GPS
	if ("Latitude" in item) {
		return (
			<div className="latest-activity-item">
				<div className="latest-activity-item-icon">
					<FontAwesomeIcon className="dx-icon" icon={faMapMarked} />
				</div>
				<div>
					<div className="latest-activity-item-title">{item?.Imei}</div>
					<div className="latest-activity-item-description">
						<div>
							Direction: <span className="latest-activity-item-status">{item?.Direction}</span>
						</div>
						<div>
							Speed: <span className="latest-activity-item-status">{item?.Speed}</span>
						</div>
						<div>
							Altitude: <span className="latest-activity-item-status">{item?.Altitude} m</span>
						</div>
					</div>
					<p className="latest-activity-item-timestamp">{formatDate(new Date(item?.Recorded), DatePatterns.LongDateTime)}</p>
				</div>
			</div>
		);
	}

	return <></>;
}

const formatNumber = new Intl.NumberFormat("en-US", {
	minimumFractionDigits: 0
}).format;

const calculateTotal = (pieChart: { getAllSeries: () => { getVisiblePoints: () => any[] }[] }) => {
	return formatNumber(
		pieChart
			.getAllSeries()[0]
			.getVisiblePoints()
			.reduce((s, p) => s + p.originalValue, 0)
	);
};

export function TooltipTemplate(pieChart: { getAllSeries: any; getInnerRadius?: any }) {
	return (
		<svg>
			<text textAnchor="middle" x="100" y="120" style={{ fontSize: 22, fill: "#494949" }}>
				<tspan x="100">Total</tspan>
				<tspan x="100" dy="20px" style={{ fontWeight: 800 }}>
					{calculateTotal(pieChart)}
				</tspan>
			</text>
		</svg>
	);
}

export function SummaryPieChart({ data, onLegendClick }: { data: any[]; onLegendClick?: ({ points }: { points: any[] }) => void }) {
	// Array of objects containing unique colors and the number of occurrences for each
	const typeOccurrenceArray = colorPalette.map((item) => {
		const containersData = data.map((mapItem: { BinStatus: number }) => mapItem.BinStatus).filter((v: number) => v === item.BinStatus).length;

		return {
			Color: item.Color,
			BinStatus: item.BinStatus,
			Name: `${item.Name} (${containersData})`,
			Total: containersData
		};
	});

	return (
		<PieChart className="pie-stats" dataSource={typeOccurrenceArray} type="pie" redrawOnResize={true} resolveLabelOverlapping="shift" innerRadius={0.55} palette={typeOccurrenceArray.map((item) => item.Color)} onLegendClick={onLegendClick}>
			<Animation enabled={false} />

			<Series argumentField="Name" valueField="Total">
				<Label visible={true}>
					<Connector visible={true} width={1} />
				</Label>
			</Series>
			<Legend horizontalAlignment="center" verticalAlignment="bottom"></Legend>
		</PieChart>
	);
}

// Component that creates horizontal list and allows dynamic actions
export const LineListHorizontal = ({ children }) => {
	// States that handle list navigation
	const [isNeedArrows, setNeedArrows] = useState<boolean>(true);
	const [isNeedLeftArrow, setNeedLeftArrow] = useState<boolean>(false);
	const [isNeedRightArrow, setNeedRightArrow] = useState<boolean>(true);

	const innerBoxRef = useRef<any>();

	const handleLeftArrowClick = useCallback(
		(e) => {
			if (innerBoxRef.current) {
				innerBoxRef.current.scrollBy({
					behavior: "smooth",
					left: -300
				});
			}
		},
		[innerBoxRef]
	);

	const handleRightArrowClick = useCallback(
		(e) => {
			if (innerBoxRef.current) {
				innerBoxRef.current.scrollBy({
					behavior: "smooth",
					left: 300
				});
			}
		},
		[innerBoxRef]
	);

	/**
	 * remove arrows if scroll position is on the edge
	 *
	 *  - use 'scroll' event handler to detect cur scroll position and remove unnecessary arrow
	 *  - initial scroll position is 0 so default values for its state is 'false' and 'true'
	 **/

	const handleScrollChangeEvent: React.EventHandler<React.UIEvent<HTMLDivElement>> = (e) => {
		const curScrollPos = Math.round(e.currentTarget.scrollLeft);
		const maxScrollPos = Math.round(e.currentTarget.scrollWidth - e.currentTarget.clientWidth);

		if (curScrollPos === 0) {
			setNeedLeftArrow(false);
		} else if (curScrollPos === maxScrollPos) {
			setNeedRightArrow(false);
		} else {
			setNeedLeftArrow(true);
			setNeedRightArrow(true);
		}
	};

	useEffect(() => {
		if (innerBoxRef.current) {
			/**
			 * this condition shows if there is any overflow of the innerBox component.
			 */
			if (innerBoxRef.current.scrollWidth > innerBoxRef.current.clientWidth) {
				// need to have scroll
				setNeedArrows(true);
			} else {
				// don't need it
				setNeedArrows(false);
			}
		}
	}, [innerBoxRef, isNeedArrows, setNeedArrows]);

	return (
		<div className="outerBox">
			<div className="innerBox" ref={innerBoxRef} onScroll={handleScrollChangeEvent}>
				{children}
			</div>
			{isNeedArrows && (
				<>
					{isNeedLeftArrow && (
						<div className="btnBox leftBtnBox">
							<Button className="btn leftBtn" icon="chevronprev" onClick={handleLeftArrowClick} />
						</div>
					)}
					{isNeedRightArrow && (
						<div className="btnBox rightBtnBox">
							<Button className="btn rightBtn" icon="chevronright" onClick={handleRightArrowClick} />
						</div>
					)}
				</>
			)}
		</div>
	);
};
