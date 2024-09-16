import { http } from "../http";

export const postForgetPasswordInit = async (email) => {
	return await http
		.post(process.env.REACT_APP_AUTH_HTTP + "/api/Accounts/forget-password/init/", {
			email: email
		})
		.then((response) => {
			return response.data;
		})
		.catch(() => {});
};

export const postUserAuth = async (formData) => {
	return await http
		.post(process.env.REACT_APP_AUTH_HTTP + "/UserJwt/authtoken", {
			login: formData.username,
			password: formData.password
		})
		.then((response) => {
			return response.data;
		})
		.catch(() => {});
};

export const getCompanies = async () => {
	return await http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Companies").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getDepartments = async () => {
	return await http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Departments").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getRoles = async () => {
	return await http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Roles").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};

export const getDrivers = async () => {
	return await http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Users/departments/4").then((response) => {
		if (response.status === 200) {
			return response.data.Value;
		}
		return [];
	});
};
