// Import Redux selector
import { useSelector } from "react-redux";

// Import react-router handler
import { Navigate } from "react-router-dom";

// Component wrapper that handles user verification
function RequireAuth({ children, redirectTo }: { children: any; redirectTo: string }) {
	// Extract the relevant state from the Redux store using the useSelector hook
	const { isUserLoggedIn } = useSelector((state: any) => state.login);

	return isUserLoggedIn ? children : <Navigate to={redirectTo} />;
}

export default RequireAuth;
