// Import React hooks
import { useState } from "react";

// Import Redux action creators
import { useDispatch } from "react-redux";

// Import Fontawesome icons
import { faAppleAlt, faRecycle, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

function ContainersTotalIndicators({ totalTrash, totalRecycle, totalCompost, addStreamFilters, removeStreamFilters }) {
	const [showTrash, setShowTrash] = useState(true);
	const [showRecycle, setShowRecycle] = useState(true);
	const [showCompost, setShowCompost] = useState(true);

	const dispatch = useDispatch();

	const onTrashClick = () => {
		setShowTrash((state) => {
			if (state) dispatch(addStreamFilters(1));
			else dispatch(removeStreamFilters(1));

			return !state;
		});
	};

	const onRecycleClick = () => {
		setShowRecycle((state) => {
			if (state) dispatch(addStreamFilters(2));
			else dispatch(removeStreamFilters(2));

			return !state;
		});
	};

	const onCompostClick = () => {
		setShowCompost((state) => {
			if (state) dispatch(addStreamFilters(3));
			else dispatch(removeStreamFilters(3));

			return !state;
		});
	};

	return (
		<div className="tiles-container">
			<div className="tiles-item" onClick={onTrashClick}>
				<div className="tiles-item-inner">
					<FontAwesomeIcon className={`dx-icon ${showTrash ? "tiles-icon" : "tiles-icon-disabled"}`} icon={faTrashAlt} />
					<div>
						<div className={showTrash ? "tiles-total" : "tiles-total-disabled"}>{totalTrash}</div>
						<div className={showTrash ? "tiles-sub-title" : "tiles-sub-title-disabled"}>TRASH</div>
					</div>
					{/* <FontAwesomeIcon className="dx-icon tiles-icon" icon={faTrashAlt} />
								<div>
									<div className="tiles-total">{totalContainers}</div>
									<div className="tiles-sub-title">CONTAINERS</div>
								</div> */}
				</div>
			</div>
			<div className="tiles-item" onClick={onRecycleClick}>
				<div className="tiles-item-inner">
					<FontAwesomeIcon className={`dx-icon ${showRecycle ? "tiles-icon" : "tiles-icon-disabled"}`} icon={faRecycle} />
					<div>
						<div className={showRecycle ? "tiles-total" : "tiles-total-disabled"}>{totalRecycle}</div>
						<div className={showRecycle ? "tiles-sub-title" : "tiles-sub-title-disabled"}>RECYCLE</div>
					</div>
					{/* <FontAwesomeIcon className="dx-icon tiles-icon" icon={faExpandAlt} />
								<div>
									<div className="tiles-total">{totalVolume} L</div>
									<div className="tiles-sub-title">VOLUME</div>
								</div> */}
				</div>
			</div>
			<div className="tiles-item" onClick={onCompostClick}>
				<div className="tiles-item-inner">
					<FontAwesomeIcon className={`dx-icon ${showCompost ? "tiles-icon" : "tiles-icon-disabled"}`} icon={faAppleAlt} />
					<div>
						<div className={showCompost ? "tiles-total" : "tiles-total-disabled"}>{totalCompost}</div>
						<div className={showCompost ? "tiles-sub-title" : "tiles-sub-title-disabled"}>COMPOST</div>
					</div>
					{/* <FontAwesomeIcon className="dx-icon tiles-icon" icon={faClock} />
								<div>
									<div className="tiles-total">{timeToFull} H</div>
									<div className="tiles-sub-title">TO FULL</div>
								</div> */}
				</div>
			</div>
		</div>
	);
}

export default ContainersTotalIndicators;
