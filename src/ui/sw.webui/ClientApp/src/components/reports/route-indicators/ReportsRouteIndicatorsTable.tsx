import React from "react";

function ReportsRouteIndicatorsTable({ routeIndicatorsReport }) {
	return (
		<table className="route-table">
			<tbody>
				<tr>
					<th>Metrics</th>
					<th className="route-table-value">Value</th>
				</tr>
				<tr>
					<th>Vehicles</th>
					<td className="route-table-value">{routeIndicatorsReport.vehicles}</td>
				</tr>
				<tr>
					<th>Total number of routes</th>
					<td className="route-table-value">{routeIndicatorsReport.totalNumberOfRoutes}</td>
				</tr>
				<tr>
					<th>Total distance traveled</th>
					<td className="route-table-value">{routeIndicatorsReport.totalDistanceTraveled}</td>
				</tr>
				<tr>
					<th>Total duration</th>
					<td className="route-table-value">{routeIndicatorsReport.totalDuration}</td>
				</tr>
				<tr>
					<th>Total collections</th>
					<td className="route-table-value">{routeIndicatorsReport.totalCollections}</td>
				</tr>
				<tr>
					<th>M. The collection point/route</th>
					<td className="route-table-value">{routeIndicatorsReport.collectionPointPerRoute}</td>
				</tr>
				<tr>
					<th>Average distance/route</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDistancePerRoute}</td>
				</tr>
				<tr>
					<th>Average duration/route</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDurationPerRoute}</td>
				</tr>
				<tr>
					<th>Average distance from depot to 1st pickup</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDistanceFromDepotTo1stPickup}</td>
				</tr>
				<tr>
					<th>Average distance from 1st to last collection</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDistanceFrom1stToLastCollection}</td>
				</tr>
				<tr>
					<th>Average distance from last pickup to KDAU/SMA</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDistanceFromLastPickup}</td>
				</tr>
				<tr>
					<th>Average distance from KDAU/SMA to depot</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDistanceFromDepot}</td>
				</tr>
				<tr>
					<th>Average duration from depot to 1st pickup</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDurationFromDepotToPickup}</td>
				</tr>
				<tr>
					<th>Average duration from 1st to last collection</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDurationFrom1stToLastCollection}</td>
				</tr>
				<tr>
					<th>Average duration from last collection to KDAU/SMA</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDurationFromLastCollection}</td>
				</tr>
				<tr>
					<th>Average duration of service at KDAU/SMA</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDurationOfService}</td>
				</tr>
				<tr>
					<th>Average duration from KDAU/SMA to depot</th>
					<td className="route-table-value">{routeIndicatorsReport.avgDurationToDepot}</td>
				</tr>
			</tbody>
		</table>
	);
}

export default ReportsRouteIndicatorsTable;
