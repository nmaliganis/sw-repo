import React from "react";

// Redux
import { useSelector } from "react-redux";

import Box, { Item } from "devextreme-react/box";
import { formatDate } from "devextreme/localization";

import Truck from "../../images/truck.png";

import { DatePatterns } from "../../utils/consts";

import "./VehicleSummaryView.scss";

function VehicleSummaryView() {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { selectedVehicle } = useSelector((state: any) => state.vehicles);

	const registeredDate = formatDate(new Date(selectedVehicle.RegisteredDate), DatePatterns.LongDateTimeYearSmall);

	return (
		<Box className="summary-container" direction="col" width="100%" height="100%">
			<Item ratio={1}>
				<Box className="summary-container-inner" direction="row" width="100%" height="100%">
					<Item ratio={2}>
						<img src={Truck} alt="Truck" width="100%" height="100%" />
					</Item>
					<Item baseSize={120}>
						<div
							className="bar-container"
							style={{
								backgroundImage: `linear-gradient( to top,
                                    rgba(0, 255, 0, 0.45), 
                                    rgba(0, 255, 0, 0.45) ${selectedVehicle?.Gas}%,
                                    rgba(0,0,0,0) ${selectedVehicle?.Gas}%,
                                    rgba(0,0,0,0)
                                )`
							}}
						>
							<div className="bar-indicator">{selectedVehicle?.Gas}%</div>
						</div>
						<div style={{ textAlign: "center" }}>
							<b>Gas</b>
						</div>
					</Item>
					<Item baseSize={120}>
						<div
							className="bar-container"
							style={{
								backgroundImage: `linear-gradient( to top,
                                    rgba(0, 255, 235, 0.45), 
                                    rgba(0, 255, 235, 0.45) ${selectedVehicle?.Gas}%,
                                    rgba(0,0,0,0) ${selectedVehicle?.Gas}%,
                                    rgba(0,0,0,0)
                                )`
							}}
						>
							<div className="bar-indicator">{selectedVehicle?.Gas}%</div>
						</div>
						<div style={{ textAlign: "center" }}>
							<b>Range</b>
						</div>
					</Item>
				</Box>
			</Item>
			<Item ratio={1}>
				<table style={{ width: "100%", height: "100%", padding: 10 }}>
					<tbody>
						<tr>
							<th>Width:</th>
							<td>{selectedVehicle?.Width} cm</td>
							<th>Registered at:</th>
							<td>{registeredDate}</td>
						</tr>
						<tr>
							<th>Height:</th>
							<td>{selectedVehicle?.Height} cm</td>
							<th>Plate:</th>
							<td>{selectedVehicle?.Plate}</td>
						</tr>
						<tr>
							<th>Axels:</th>
							<td>{selectedVehicle?.Axels}</td>
							<th>Brand:</th>
							<td>{selectedVehicle?.Brand}</td>
						</tr>
						<tr>
							<th>Length:</th>
							<td>{selectedVehicle?.Length}</td>
							<th>Company:</th>
							<td>{selectedVehicle?.Company}</td>
						</tr>
						<tr>
							<th>Min Turn Radius:</th>
							<td>{selectedVehicle?.MinTurnRadius}</td>
							<th>Asset Category:</th>
							<td>{selectedVehicle?.AssetCategoryId}</td>
						</tr>

						<tr>
							<th>Description:</th>
							<td colSpan={3}>Text that goes and goes and goes and goes and goes and goes and goes and goes and goes and goes and goes</td>
						</tr>
					</tbody>
				</table>
			</Item>
		</Box>
	);
}

export default React.memo(VehicleSummaryView);
