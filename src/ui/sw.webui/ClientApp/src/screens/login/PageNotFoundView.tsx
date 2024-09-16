import React from "react";

import classes from "../../styles/login/PageNotFound.module.scss";

function PageNotFoundView() {
	return (
		<div className={classes.notFound}>
			<div className={classes.notFoundPanel}>
				<div className={classes.notFoundId}>
					<h3>Oops! Page not found</h3>
					<h1>
						<span>4</span>
						<span>0</span>
						<span>4</span>
					</h1>
				</div>
				<h2>we are sorry, but the page you requested was not found</h2>
			</div>
		</div>
	);
}

export default PageNotFoundView;
