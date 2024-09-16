// Import Fontawesome
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChartLine, faExclamationTriangle, faRedoAlt, faTrashAlt, faTrashRestoreAlt } from "@fortawesome/free-solid-svg-icons";

import styles from "../../styles/itineraries/Itineraries.module.scss";

// TODO: Add stats from endpoint
function ItinerariesStats({ itineraryData }) {
	return (
		<div className={styles["tiles-wrapper"]}>
			<div className={styles["tiles-container"]}>
				<div className={styles["tiles-item"]}>
					<div className={styles["tiles-item-inner"]}>
						<FontAwesomeIcon className={`dx-icon ${styles["tiles-icon"]}`} icon={faTrashAlt} />
						<div>
							<div className={styles["tiles-total"]}>{itineraryData.length ? itineraryData?.reduce((item) => item?.AssignedContainers?.length || 0) : 0}</div>
							<div className={styles["tiles-sub-title"]}>TOTAL CONTAINERS</div>
						</div>
					</div>
				</div>
			</div>
			<div className={styles["tiles-container"]} style={{ justifyContent: "space-around" }}>
				<div className={styles["tiles-item"]}>
					<div className={styles["tiles-item-inner"]}>
						<FontAwesomeIcon className={`dx-icon ${styles["tiles-icon"]}`} icon={faTrashRestoreAlt} />
						<div>
							<div className={styles["tiles-total"]}>{itineraryData.length ? itineraryData?.reduce((item) => item?.ServicedContainers?.length || 0) : 0}</div>
							<div className={styles["tiles-sub-title"]}>CONTAINERS SERVICED</div>
						</div>
					</div>
				</div>
				<div className={styles["tiles-item"]}>
					<div className={styles["tiles-item-inner"]}>
						<FontAwesomeIcon className={`dx-icon ${styles["tiles-icon"]}`} icon={faRedoAlt} />
						<div>
							<div className={styles["tiles-total"]}>{itineraryData.length ? itineraryData?.reduce((item) => item?.SkippedContainers?.length || 0) : 0}</div>
							<div className={styles["tiles-sub-title"]}>CONTAINERS SKIPPED</div>
						</div>
					</div>
				</div>
			</div>
			<div className={styles["tiles-container"]}>
				<div className={styles["tiles-item"]}>
					<div className={styles["tiles-item-inner"]}>
						<FontAwesomeIcon className={`dx-icon ${styles["tiles-icon"]}`} icon={faChartLine} />
						<div>
							<div className={styles["tiles-total"]}>{itineraryData?.CompletionRate || 0} %</div>
							<div className={styles["tiles-sub-title"]}>COMPLETION RATE</div>
						</div>
					</div>
				</div>
				<div className={styles["tiles-item"]}>
					<div className={styles["tiles-item-inner"]}>
						<FontAwesomeIcon className={`dx-icon ${styles["tiles-icon"]}`} icon={faExclamationTriangle} />
						<div>
							<div className={styles["tiles-total"]}>{itineraryData?.Alerts || 0}</div>
							<div className={styles["tiles-sub-title"]}>ALERTS</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}

export default ItinerariesStats;
