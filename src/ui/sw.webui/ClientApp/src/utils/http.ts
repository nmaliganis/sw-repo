import Axios from "axios";

// Object for default axios options
export const http = Axios.create({
	timeout: 30000,
	params: { _dc: null },
	headers: {
		Authorization: "",
		"Access-Control-Allow-Origin": "*",
		"Access-Control-Allow-Methods": "GET,PUT,POST,DELETE,PATCH,OPTIONS",
		Accept: "application/json",
		"Content-Type": "application/json"
	},
	validateStatus: () => {
		return true;
	}
});

http.interceptors.request.use((request) => {
	if (request.headers) request.headers.Authorization = localStorage.getItem("jwtToken") || "";

	if (!request.params) request.params = {};
	request.params._dc = new Date().getTime();
	return request;
});

//Response interceptor
http.interceptors.response.use(
	(response) => {
		// Show status of each call back on the console
		console.log(`%c ${response.config?.url} returned ${response?.status}`, `color: white; font-weight: bold; background-color: ${response?.status === 200 || response?.status === 201 ? "green" : "red"};`);

		if (response.headers) response.headers.Authorization = localStorage.getItem("jwtToken") || "";

		return response;
	},
	(error) => {
		console.log(error);

		return error;
	}
);
