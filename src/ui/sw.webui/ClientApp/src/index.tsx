import React from "react";
import ReactDOM from "react-dom";

import { Provider } from "react-redux";
import { store } from "./redux/store";

import App from "./App";

import "./styles/index.scss";
import reportWebVitals from "./reportWebVitals";

import packageJson from "../package.json";

// Logging the version of the web app to the console with a styled message
console.log(`%c sw - ${packageJson.version} 📌`, "color: white; font-weight: bold; background-color: #35baf6");

// Rendering the App component wrapped in a Provider component, which gives App access to the Redux store
ReactDOM.render(
	<React.StrictMode>
		<Provider store={store}>
			<App />
		</Provider>
	</React.StrictMode>,
	document.getElementById("root")
);

reportWebVitals();
