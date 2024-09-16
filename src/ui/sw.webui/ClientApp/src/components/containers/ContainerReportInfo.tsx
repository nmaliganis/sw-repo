// Import React hooks
import React from "react";

// Import Redux action creators
import { useSelector } from "react-redux";

// import custom tools
import { ContainerModelT } from "../../utils/types";
import { CapacityType, MaterialType, streamType } from "../../utils/consts";
import { binStatusRenderer, containerTypeImage, dateRenderer } from "../../utils/containerUtils";

import classes from "../../styles/containers/ContainerReportInfo.module.scss";

function ContainerReportInfo({ selectedContainer }: { selectedContainer: ContainerModelT }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	return (
		<div className={classes.gridContainer}>
			<div className={classes.gridHeader}>
				<div style={{ height: "100%", display: "flex", alignItems: "center" }}>
					{/* <div className={classes.gridHeaderTitle}>{selectedContainer?.Name}</div> */}
					<div className={classes.gridHeaderContainer}>
						{binStatusRenderer({ value: selectedContainer?.BinStatus })}
						<div className={classes.gridHeaderTitle} style={{ fontWeight: 600 }}>
							LEVEL {selectedContainer?.Level} %
						</div>
					</div>
					<div>
						<div className={classes.gridItemTitle}>LAST UPDATED</div>
						<div>{dateRenderer({ value: selectedContainer?.LastUpdated })}</div>
					</div>
				</div>
				<hr />
			</div>
			<div className={classes.gridItem1}>
				<div className={classes.gridItemTitle}>CONTAINER TYPE</div>
				<div>{MaterialType.find((item: { Id: string | number }) => item.Id === selectedContainer?.Material)?.Name}</div>
			</div>
			<div className={classes.gridItem2}>
				<div className={classes.gridItemTitle}>COMPANY</div>
				<div>{userData.UserParams.Companies.length ? userData.UserParams.Companies?.find((item: { Id: string | number }) => item.Id === selectedContainer?.CompanyId)?.Name : "Vari, Voulia, Vouliagmeni"}</div>
			</div>
			<div className={classes.gridItem3}>
				<div className={classes.gridItemTitle}>CAPACITY</div>
				<div>{CapacityType.find((item: { Id: string | number }) => item.Id === selectedContainer?.Capacity)?.Name}</div>
			</div>
			<div className={classes.gridItem4}>
				<div className={classes.gridItemTitle}>ASSET</div>
				<div>{userData.UserParams.AssetCategories?.find((item: { Id: string | number }) => item.Id === selectedContainer?.AssetCategoryId)?.Name}</div>
			</div>
			<div className={classes.gridItem5}>
				<div className={classes.gridItemTitle}>CONTENT TYPE</div>
				<div>{streamType.find((item: { Id: string | number }) => item.Id === selectedContainer?.WasteType)?.Name}</div>
			</div>
			{new Date(selectedContainer?.MandatoryPickupDate).getTime() > new Date().getTime() ? (
				<div className={classes.gridItem6}>
					<div className={classes.gridItemTitle}>MANDATORY PICKUP ON</div>
					<div>{dateRenderer({ value: selectedContainer?.MandatoryPickupDate })}</div>
				</div>
			) : (
				<></>
			)}
			<div className={classes.gridItemImage}>
				{/* TODO: Change logic to set image from API */}
				<img src={containerTypeImage(selectedContainer.WasteType, selectedContainer.Material, selectedContainer.Capacity)} alt="Container" style={{ height: "100%" }} loading="lazy" />
			</div>
		</div>
	);
}

export default React.memo(ContainerReportInfo);
