// import { useState } from "react";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function Navbar() {
    const { user, isAuthenticated, logout } = useAuth();
    const navigate = useNavigate();
    // const [open, setOpen] = useState(false);

    const isAdmin = user?.userRole === 1;
    const canShop = user?.userRole === 0;

    function handleLogout() {
        // setOpen(false);
        logout();
        navigate("/login", { replace: true });
    }

    return (
        <header className="site-header">
            <nav className="navbar" aria-label="Main navigation">
                <Link to="/" className="brand"><span className="brand-icon">✦</span>One<span>Stop</span><span className="brand-period">.</span></Link>

                {/* <Link to="/products" className="brand">
                    E-Commerce
                </Link> */}

                <div className="nav-links">
                    {/* <NavLink end to="/">Home</NavLink> */}
                    <NavLink to="/products">Products</NavLink>
                    {/* <NavLink to="/contact">Contact us</NavLink> */}

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
