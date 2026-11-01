import { Link } from "react-router-dom";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { addToCart } from "../services/cartService";

export default function ProductCard({ product }) {
    const { token, isAuthenticated, user } = useAuth();
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState("");

    async function handleAddToCart() {
        if (!isAuthenticated) {
            navigate("/login");
            return;
        }

        if (user?.userRole !== 0) {
            setMessage("Only customers can add products to cart.");
            return;
        }

        setLoading(true);
        setMessage("");

        try {
            await addToCart(token, product.id, 1);
            setMessage("Added to cart successfully!");
        } catch (err) {
            setMessage(err.message);
        } finally {
            setLoading(false);
        }
    }

    return (
        <article className="product-card">
            <span className="product-category">
                {product.category}
            </span>

            <h3>{product.name}</h3>

            <p className="product-description">
                {product.description}
            </p>

            <p className="product-price">
                ₹{product.price.toLocaleString("en-IN")}
            </p>

            {user?.userRole === 1 && (
                <Link to="/admin">
                    Edit / Manage Product
                </Link>
            )}
            <button
                type="button"
                onClick={handleAddToCart}
                disabled={loading}
            >
                {loading ? "Adding..." : "Add to Cart"}
            </button>

            {message && <p role="status">{message}</p>}
        </article>
    );
}
