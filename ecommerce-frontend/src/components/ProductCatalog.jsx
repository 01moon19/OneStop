
import { useEffect, useState } from "react";
import { getProducts } from "../services/ProductService";
import ProductCard from "./ProductCard";
import { Link } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function ProductCatalog() {
    const [products, setProducts] = useState([]);
    const [category, setCategory] = useState("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const { user } = useAuth();
    const isAdmin = user?.userRole === 1;

    useEffect(() => {
        let active = true;

        async function loadProducts() {
            setLoading(true);
            setError("");

            try {
                const data = await getProducts(category);
                if (active) setProducts(data);
            } catch (err) {
                if (active) setError(err.message);
            } finally {
                if (active) setLoading(false);
            }
        }

        loadProducts();

        return () => {
            active = false;
        };
    }, [category]);

    return (
        <main>
            <h1>Our Products</h1>

            

            {isAdmin && (
                <Link to="/admin" className="cart-link">
                    Manage Products & Inventory
                </Link>
            )}

            <Link to="/cart" className="cart-link">
                View Cart
            </Link>

            <select
                value={category}
                onChange={(e) => setCategory(e.target.value)}
            >
                <option value="">All Categories</option>
                <option value="Electronics">Electronics</option>
                <option value="Accessories">Accessories</option>
            </select>

            {loading && <p>Loading products...</p>}
            {error && <p role="alert">{error}</p>}

            {!loading && !error && products.length === 0 && (
                <p>No products found.</p>
            )}

            {!loading && !error && (
                <div className="product-grid">
                    {products.map((product) => (
                        <ProductCard key={product.id} product={product} />
                    ))}
                </div>
            )}
        </main>
    );
}
