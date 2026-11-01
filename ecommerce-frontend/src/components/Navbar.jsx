
import { Link, NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function Navbar() {
    const { user, isAuthenticated, logout } = useAuth();
    const navigate = useNavigate();

    const isAdmin = user?.userRole === 1;
    const canShop = user?.userRole === 0;

    function handleLogout() {
        logout();
        navigate("/login", { replace: true });
    }

    return (
        <header className="site-header">
            <nav className="navbar">
                <Link to="/products" className="brand">
                    E-Commerce
                </Link>

                <div className="nav-links">
                    <NavLink to="/products">Products</NavLink>

                    {canShop && (
                        <>
                            <NavLink to="/cart">Cart</NavLink>
                            <NavLink to="/orders">My Orders</NavLink>
                        </>
                    )}

                    {isAdmin && (
                        <NavLink to="/admin">Manage Products</NavLink>
                    )}
                </div>

                <div className="nav-account">
                    {isAuthenticated ? (
                        <>
                            <span className="welcome">
                                {user?.userName}
                            </span>

                            <button type="button" onClick={handleLogout}>
                                Logout
                            </button>
                        </>
                    ) : (
                        <>
                            <NavLink to="/login">Login</NavLink>
                            <NavLink to="/register">Register</NavLink>
                        </>
                    )}
                </div>
            </nav>
        </header>
    );
}
