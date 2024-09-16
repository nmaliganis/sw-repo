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

test("Is token string", async () => {
	// ASSERT
	expect(typeof token).toBe("string");
});

test("GET Containers", async () => {
	const response = await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Containers`, { headers: { Authorization: "Bearer " + token } }).then((response) => response);

	expect(response.status).toBe(200);
});

test("GET Polygon", async () => {
	const response = await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/Polygon`, { headers: { Authorization: "Bearer " + token } }).then((response) => response);

	expect(response.status).toBe(200);
});

test("GET EventHistory records", async () => {
	const response = await http.get(`${process.env.REACT_APP_ASSET_HTTP}/v1/EventHistory/records/5`, { headers: { Authorization: "Bearer " + token } }).then((response) => response);

	expect(response.status).toBe(200);
});
