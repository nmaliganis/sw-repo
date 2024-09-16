/**
 * @jest-environment node
 */

import { http } from "../utils/http";

import "@testing-library/jest-dom";

let token = "";

test("Get token from UserJWT", async () => {
	const response = await http
		.post(process.env.REACT_APP_AUTH_HTTP + "/UserJwt/authtoken", {
			login: "su",
			password: "su"
		})
		.then((response) => response);

	// ASSERT
	expect(response.status).toBe(200);

	token = response.data.Token;
});

const selectedItem = 11;

test("GET Container with ID:11", async () => {
	const response = await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Containers/${selectedItem}`, { headers: { Authorization: "Bearer " + token } }).then((response) => response);

	expect(response.status).toBe(200);
});

test("GET Container history with ID:11", async () => {
	const start = new Date(new Date().setDate(new Date().getDate() - 1));
	const end = new Date();

	const response = await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/EventHistory/container/${selectedItem}/start_date/${start}/end_date/${end}`, { headers: { Authorization: "Bearer " + token } }).then((response) => response.data);

	expect(response.status).toBe(200);
});
