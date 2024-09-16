import "../styles/LoadingPage.scss";

function LoadingPage() {
	return (
		<div className="loading-wrapper">
			<div className="loading-container">
				<span className="dot"></span>
				<div className="dots">
					<span></span>
					<span></span>
					<span></span>
				</div>
			</div>
		</div>
	);
}

export default LoadingPage;
