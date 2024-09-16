// Import React hooks
import { useState, useCallback, useMemo, useReducer } from "react";

// Import Redux selector
import { useSelector } from "react-redux";

// Import DevExtreme components
import DataGrid, { Column, Button as DataGridButton, Lookup, Scrolling, Editing, Selection, ColumnChooser, Sorting, HeaderFilter, FilterRow, RemoteOperations, Toolbar, Item as ToolbarItem, Summary, TotalItem, ColumnFixing } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import { Popup } from "devextreme-react/popup";
import { Form, SimpleItem, GroupItem, Label, RequiredRule, ButtonItem } from "devextreme-react/form";
import Button from "devextreme-react/button";
import notify from "devextreme/ui/notify";

// Import custom tools
import { onRowUpdating } from "../../../utils/containerUtils";
import { countryCodesObject } from "../../../utils/consts";
import { activatedHeaderFilter, dateRender } from "../../../utils/adminUtils";
import { http } from "../../../utils/http";

import UsersForm, { userGenders } from "../../../components/admin/UsersForm";

import "../../../styles/admin/Users.scss";

// Initial state of the device editing form popup
const initPopupState = {
	formData: {},
	popupVisible: false,
	popupMode: ""
};

const activationFormData = {
	ActivationCode: ""
};

// Function that that sets initial values for the data object passed as an argument. It is used to set initial values when a new row is added to the table.
const popupReducer = (state: any, action: { type: any; data: any; popupMode: any }) => {
	switch (action.type) {
		case "initPopup":
			return {
				formData: action.data,
				popupVisible: true,
				popupMode: action.popupMode
			};
		case "hidePopup":
			return {
				popupVisible: false
			};
		default:
			break;
	}
};

// Function that renders the corresponding role in accordance with the data
const rolesRenderer = (cellData: any) => {
	return cellData.value;
};

function UsersView() {
	// State that holds all cell changes on DataGrid
	const [gridChanges, setGridChanges] = useState([]);

	// State that handles user activation pop up
	const [activateUser, setActivateUser] = useState({ UserId: "", PopupVisible: false });

	// Reducer that holds data for created, edited item
	const [{ formData, popupVisible, popupMode }, dispatchPopup] = useReducer<(arg1: any, actions: any) => any>(popupReducer, initPopupState);

	// Extract the relevant state from the Redux store using the useSelector hook
	const { userData } = useSelector((state: any) => state.login);

	// Function that dispatches an action to update the popupVisible, popupMode, and formData state variables.
	const showPopup = (popupMode: any, data: any) => {
		dispatchPopup({ type: "initPopup", data, popupMode });
	};

	// Function that handles resetting the user's password
	const resetPasswordUser = (e: any) => {
		console.log(e.row.data);
	};

	// Function that calls the showPopup function with the "Add" mode and an empty data object
	const addClick = (e: any) => {
		showPopup("Add", {});
	};

	// Function that calls the showPopup function with the "Edit" mode and the data from the row that was clicked
	const editClick = (e: any) => {
		showPopup("Edit", { ...e.row.data });
	};

	// Function that dispatches an action to update the popupVisible state variable
	const onHiding = () => {
		dispatchPopup({ type: "hidePopup" });
	};

	// Object containing the dataSource that is being used for the DataGrid
	const dataSource = useMemo(() => {
		return new CustomStore({
			key: "Id",
			load: (props) => {
				return http.get(process.env.REACT_APP_AUTH_HTTP + "/v1/Users").then((response) => {
					if (response.status === 200) {
						return {
							data: response.data.Value,
							totalCount: response.data.Value.length
						};
					}

					return {
						data: [],
						totalCount: 0
					};
				});
			},
			insert: (values) => {
				const userData = {
					...values,
					GenderValue: values.Gender
				};

				return http
					.post(process.env.REACT_APP_AUTH_HTTP + "/Accounts/register", userData)
					.then((response) => {
						if (response.status === 201) {
							// Open pop up form on activation
							setActivateUser({ UserId: response.data.id, PopupVisible: true });

							return response.data;
						}
						notify("Failed to create User.", "error", 2500);
					})
					.catch(() => {
						notify("Failed to create User.", "error", 2500);
					});
			},
			update: (key, values) => {
				return http
					.put(process.env.REACT_APP_AUTH_HTTP + `/v1/Users/${key}`, values)
					.then(({ data }) => {
						notify(`${values.login} was updated successfully.`, "success", 2500);

						return data;
					})
					.catch(() => {
						notify("Failed to update User.", "error", 2500);
					});
			},
			remove: (key) => {
				return http.delete(process.env.REACT_APP_AUTH_HTTP + `/v1/Users/soft/${key}`, { data: { deletedReason: `deleted ${key} from sw` } }).then((response) => {
					if (response.status === 200) {
						notify(`User ${key} was deleted successfully.`, "success", 2500);
					} else {
						notify(`Failed to delete ${key} User.`, "error", 2500);
					}
				});
			}
		});
	}, []);

	const onActivatePopupHiding = useCallback(() => {
		setActivateUser({ UserId: "", PopupVisible: false });
	}, []);

	// Object for previous button settings. Memoization prevents flickering
	const cancelBtnOptions = useMemo(() => {
		return {
			text: "CANCEL",
			type: "normal",
			onClick: () => {
				onActivatePopupHiding();
			}
		};
	}, [onActivatePopupHiding]);

	// Object for confirm button settings. Memoization prevents flickering
	const saveBtnOptions = useMemo(() => {
		return {
			text: "SAVE",
			type: "default",
			useSubmitBehavior: true,
			onClick: () => {
				http.put(process.env.REACT_APP_AUTH_HTTP + "/Accounts/activate/" + activateUser.UserId, { activationCode: activationFormData.ActivationCode }).then((res) => {
					console.log(res);

					notify(`User was created successfully.`, "success", 2500);

					onActivatePopupHiding();
				});
			}
		};
	}, [activateUser.UserId, onActivatePopupHiding]);

	// Function that updates state whenever a cell changes
	const onEditingChange = useCallback((changes) => {
		setGridChanges(changes);
	}, []);

	return (
		<>
			<DataGrid
				className="data-grid-common"
				dataSource={dataSource}
				width="100%"
				height="100%"
				showBorders={true}
				showRowLines={true}
				showColumnLines={true}
				repaintChangesOnly={true}
				rowAlternationEnabled={true}
				allowColumnResizing={true}
				columnResizingMode="nextColumn"
				columnMinWidth={50}
				onRowUpdating={onRowUpdating}
			>
				<Editing mode="popup" changes={gridChanges} onChangesChange={onEditingChange} allowAdding={true} allowUpdating={true} allowDeleting={true} selectTextOnEditStart={false} startEditAction="click" useIcons={true} />
				<RemoteOperations sorting={false} filtering={false} paging={false} />
				<Scrolling mode="virtual" showScrollbar="always" />
				<ColumnFixing enabled={true} />
				<Sorting mode="single" />
				<Selection mode="multiple" selectAllMode="allPages" showCheckBoxesMode="always" />
				<HeaderFilter allowSearch={true} visible={true} />
				<FilterRow visible={true} applyFilter="Immediately" />
				<ColumnChooser enabled={true} allowSearch={true} mode="select" />
				<Toolbar>
					<ToolbarItem location="after">
						<Button icon="add" onClick={addClick} />
					</ToolbarItem>
					<ToolbarItem location="after" name="saveButton" />
					<ToolbarItem location="after" name="revertButton" />
					<ToolbarItem location="before" name="columnChooserButton" />
				</Toolbar>
				{/* user columns */}

				<Column dataField="Login" caption="Login" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} />
				<Column dataField="IsActivated" caption="Activated" allowSorting={true} allowFiltering={false} allowHeaderFiltering={true} allowEditing={false} type="boolean">
					<HeaderFilter dataSource={activatedHeaderFilter} />
				</Column>
				<Column dataField="ResetDate" caption="Reset Date" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} allowEditing={false} cellRender={dateRender} />
				<Column dataField="ResetKey" caption="Reset Key" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} allowEditing={false} />
				<Column dataField="ActivationKey" caption="Activation Key" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} allowEditing={false} />
				<Column dataField="LastLogin" caption="Last Login" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} cellRender={dateRender} />
				<Column dataField="Disabled" caption="Reset Key" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} allowEditing={false} />
				<Column dataField="Active" caption="Last Login" allowSorting={true} allowFiltering={false} allowHeaderFiltering={false} allowEditing={false} />
				<Column dataField="MemberEmail" caption="Member Email" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} allowEditing={false} />

				{/* register account columns */}
				<Column dataField="Password" visible={false} />
				<Column dataField="Firstname" visible={false} />
				<Column dataField="Lastname" visible={false} />
				<Column dataField="Email" visible={false} />
				<Column dataField="Gender" visible={false}>
					<Lookup dataSource={userGenders} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="ExtPhone" visible={false}>
					<Lookup dataSource={countryCodesObject} />
				</Column>
				<Column dataField="Phone" visible={false} />
				<Column dataField="ExtMobile" visible={false}>
					<Lookup dataSource={countryCodesObject} />
				</Column>
				<Column dataField="Mobile" visible={false} />

				<Column dataField="Notes" visible={false} />
				<Column dataField="Street" visible={false} />
				<Column dataField="StreetNumber" visible={false} />
				<Column dataField="AddressPostCode" visible={false} />
				<Column dataField="AddressCity" visible={false} />
				<Column dataField="AddressRegion" visible={false} />
				<Column dataField="RolesIds" caption="Roles" visible={false} cellRender={rolesRenderer}>
					<Lookup dataSource={userData.UserParams.Roles} valueExpr="Id" displayExpr="Name" />
				</Column>
				<Column dataField="Company" caption="Company" allowHeaderFiltering={true} />
				<Column dataField="DepartmentsIds" caption="Departments" allowHeaderFiltering={true} />
				<Column dataField="CodeErp" caption="Code ERP" allowSorting={true} allowFiltering={true} allowHeaderFiltering={false} visible={false} />

				<Column type="buttons">
					<DataGridButton hint="Reset Password" icon="key" onClick={resetPasswordUser} />
					<DataGridButton name="edit" onClick={editClick} />
					<DataGridButton name="delete" />
				</Column>
				<Summary>
					<TotalItem column="Id" showInColumn="Login" displayFormat="Total: {0}" summaryType="count" />
				</Summary>
			</DataGrid>
			<Popup title={popupMode} closeOnOutsideClick={true} visible={popupVisible} onHiding={onHiding} showCloseButton={true} width={1200} height={670}>
				<UsersForm formData={formData} popupMode={popupMode} dispatchPopup={dispatchPopup} dataSource={dataSource} userData={userData} />
			</Popup>
			<Popup title={"User Activation"} closeOnOutsideClick={true} visible={activateUser.PopupVisible} onHiding={onActivatePopupHiding} showCloseButton={true} width={500} height={180}>
				<Form formData={activationFormData} labelLocation="top" showColonAfterLabel={true} validationGroup="userActivation">
					<SimpleItem dataField="ActivationCode">
						<Label text="Activation Code" />
						<RequiredRule message="Activation code is required" />
					</SimpleItem>
					<GroupItem itemType="group" colCount={2} colSpan={1}>
						<ButtonItem cssClass="previous-button-container" buttonOptions={cancelBtnOptions} />
						<ButtonItem buttonOptions={saveBtnOptions} />
					</GroupItem>
				</Form>
			</Popup>
		</>
	);
}

export default UsersView;
