// import Devextreme components
import Button from "devextreme-react/button";

function FailedPage() {
	return (
		<div className="center-content" style={{ height: "100%", width: "100%" }}>
			<div className="vertical-center-content" style={{ flexDirection: "column" }}>
				<div>Failed to load. Please try again.</div>
				<Button
					text="Reload Page"
					type="default"
					stylingMode="contained"
					onClick={() => {
						window.location.replace("/");
					}}
				/>
			</div>
		</div>
	);
}

export default FailedPage;
