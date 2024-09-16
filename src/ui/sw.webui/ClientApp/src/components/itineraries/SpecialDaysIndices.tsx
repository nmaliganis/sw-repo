import styles from "../../styles/itineraries/SpecialDays.module.scss";

function SpecialDaysIndices() {
	return (
		<div className={styles["special-days-indices-container"]}>
			<div className={styles["special-days-indices-current"]}>Current day</div>
			<div className={styles["special-days-indices-public-holiday"]}>Public Holiday</div>
			<div className={styles["special-days-indices-unavailable"]}>Unavailable</div>
			<div className={styles["special-days-indices-custom"]}>Custom Special Day</div>
		</div>
	);
}

export default SpecialDaysIndices;
