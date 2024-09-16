// Import React hooks
import { useState, useEffect } from "react";

// Import DevExtreme components
import Box, { Item } from "devextreme-react/box";

// Import custom tools
import LoadingPage from "../../utils/LoadingPage";
import { specialDayT } from "../../utils/itineraries";

// Import custom components
import { SpecialDaysIndices, SpecialDaysCalendar } from "../../components/itineraries";

function SpecialDaysView() {
	// States that handle loaded data and current selected year
	const [specialDayData, setSpecialDayData] = useState<specialDayT[]>([]);
	const [currentYear, setCurrentYear] = useState(new Date().getFullYear());

	// Load data on init
	useEffect(() => {
		const dataSource = [
			{
				ID: 1,
				Name: "Current date",
				Description: "",
				StartDate: new Date(),
				EndDate: new Date(),
				Repeat: true,
				Color: "#03a9f4"
			},
			{
				ID: 2,
				Name: "Google I/O",
				Description: "San Francisco, CA",
				StartDate: new Date(2019, 4, 28),
				EndDate: new Date(2019, 4, 29),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 3,
				Name: "Microsoft Convergence",
				Description: "New Orleans, LA",
				StartDate: new Date(2019, 2, 16),
				EndDate: new Date(2019, 2, 19),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 4,
				Name: "Microsoft Build Developer Conference",
				Description: "San Francisco, CA",
				StartDate: new Date(2019, 3, 29),
				EndDate: new Date(2019, 4, 1),
				Repeat: true,
				Color: "#ff5252"
			},
			{
				ID: 5,
				Name: "Apple Special Event",
				Description: "San Francisco, CA",
				StartDate: new Date(2019, 8, 1),
				EndDate: new Date(2019, 8, 1),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 6,
				Name: "Apple Keynote",
				Description: "San Francisco, CA",
				StartDate: new Date(2019, 8, 9),
				EndDate: new Date(2019, 8, 9),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 7,
				Name: "Chrome Developer Summit",
				Description: "Mountain View, CA",
				StartDate: new Date(2019, 10, 17),
				EndDate: new Date(2019, 10, 18),
				Repeat: true,
				Color: "#f7cb5a"
			},
			{
				ID: 8,
				Name: "F8 2015",
				Description: "San Francisco, CA",
				StartDate: new Date(2019, 2, 25),
				EndDate: new Date(2019, 2, 26),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 9,
				Name: "Yahoo Mobile Developer Conference",
				Description: "New York",
				StartDate: new Date(2019, 7, 25),
				EndDate: new Date(2019, 7, 26),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 10,
				Name: "Android Developer Conference",
				Description: "Santa Clara, CA",
				StartDate: new Date(2019, 11, 1),
				EndDate: new Date(2019, 11, 4),
				Repeat: true,
				Color: "#66bb6a"
			},
			{
				ID: 11,
				Name: "LA Tech Summit",
				Description: "Los Angeles, CA",
				StartDate: new Date(2019, 10, 17),
				EndDate: new Date(2019, 10, 17),
				Repeat: true,
				Color: "#66bb6a"
			}
		];

		setSpecialDayData(
			dataSource.map((item) => {
				return {
					id: item.ID,
					name: item.Name,
					description: item.Description,
					startDate: item.Repeat ? new Date(item.StartDate).setFullYear(currentYear) : item.StartDate,
					endDate: item.Repeat ? new Date(item.EndDate).setFullYear(currentYear) : item.EndDate,
					repeat: item.Repeat,
					color: item.Color
				};
			})
		);
	}, [currentYear]);

	if (specialDayData.length)
		return (
			<Box direction="col" width="100%" height="100%">
				<Item baseSize="auto">
					<SpecialDaysIndices />
				</Item>
				<Item ratio={1}>
					<SpecialDaysCalendar dataSource={specialDayData} currentYear={currentYear} setCurrentYear={setCurrentYear} />
				</Item>
			</Box>
		);

	return <LoadingPage />;
}

export default SpecialDaysView;
