
import {
    BrowserRouter,
    Navigate,
    Route,
    Routes
} from "react-router-dom";

import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";
import ProductCatalog from "./components/ProductCatalog";

import Login from "./pages/Login";
import Register from "./pages/Register";
import Cart from "./pages/Cart";
import Orders from "./pages/Orders";
import AdminDashboard from "./pages/AdminDashboard";

import "./App.css";

export default function App() {
    return (
        <BrowserRouter>
            <Navbar />

            <Routes>
                <Route
                    path="/"
                    element={<Navigate to="/products" replace />}
                />

                <Route
                    path="/products"
                    element={<ProductCatalog />}
                />

                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                <Route element={<ProtectedRoute allowedRoles={[0]} />}>
                    <Route path="/cart" element={<Cart />} />
                    <Route path="/orders" element={<Orders />} />
                </Route>

                <Route element={<ProtectedRoute allowedRoles={[1]} />}>
                    <Route path="/admin" element={<AdminDashboard />} />
                </Route>

                <Route
                    path="*"
                    element={<Navigate to="/products" replace />}
                />
            </Routes>
        </BrowserRouter>
    );
}
