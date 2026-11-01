
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { getProducts } from "../services/productService";
import {
    createProduct,
    updateProduct,
    deleteProduct
} from "../services/adminProductService";

const emptyProduct = {
    name: "",
    price: "",
    description: "",
    category: "",
    image: "",
    stockQuantity: 0
};

export default function AdminDashboard() {
    const { token } = useAuth();

    const [products, setProducts] = useState([]);
    const [form, setForm] = useState(emptyProduct);
    const [editingId, setEditingId] = useState(null);
    const [loading, setLoading] = useState(true);
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState("");
    const [message, setMessage] = useState("");

    async function refreshProducts() {
        const data = await getProducts();
        setProducts(data);
    }

    useEffect(() => {
        let cancelled = false;

        async function fetchProducts() {
            try {
                const data = await getProducts();
                if (!cancelled) setProducts(data);
            } catch (err) {
                if (!cancelled) setError(err.message);
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchProducts();

        return () => {
            cancelled = true;
        };
    }, []);

    function handleChange(e) {
        const { name, value } = e.target;

        setForm(previous => ({
            ...previous,
            [name]: value
        }));
    }

    function startEdit(product) {
        setEditingId(product.id);

        setForm({
            name: product.name,
            price: product.price,
            description: product.description,
            category: product.category,
            stockQuantity: product.stockQuantity,
            image: ""
        });

        setError("");
        setMessage("");
        window.scrollTo({ top: 0, behavior: "smooth" });
    }

    function resetForm() {
        setForm({ ...emptyProduct });
        setEditingId(null);
    }

    async function handleSubmit(e) {
        e.preventDefault();

        if (busy) return;

        setBusy(true);
        setError("");
        setMessage("");

        const productData = {
            name: form.name,
            price: Number(form.price),
            description: form.description,
            category: form.category,
            stockQuantity: Number(form.stockQuantity)
        };

        try {
            if (editingId !== null) {
                await updateProduct(token, editingId, productData);
                setMessage("Product updated successfully.");
            } else {
                await createProduct(token, {
                    ...productData,
                    image: form.image
                });
                setMessage("Product created successfully.");
            }

            resetForm();
            await refreshProducts();
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    async function handleDelete(id) {
        if (busy) return;

        const confirmed = window.confirm(
            "Are you sure you want to delete this product?"
        );

        if (!confirmed) return;

        setBusy(true);
        setError("");
        setMessage("");

        try {
            await deleteProduct(token, id);
            await refreshProducts();

            if (editingId === id) resetForm();

            setMessage("Product deleted successfully.");
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    return (
        <main className="admin-dashboard">
            <h1>Admin Dashboard</h1>

            <p>
                <Link to="/products">Back to Store</Link>
            </p>

            {error && <p role="alert" className="error">{error}</p>}
            {message && <p role="status" className="success">{message}</p>}

            <section className="admin-section">
                <h2>
                    {editingId !== null ? "Edit Product" : "Add Product"}
                </h2>

                <form onSubmit={handleSubmit} className="product-form">
                    <label htmlFor="product-name">Product Name</label>
                    <input
                        id="product-name"
                        name="name"
                        value={form.name}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="product-price">Price</label>
                    <input
                        id="product-price"
                        name="price"
                        type="number"
                        min="0.01"
                        step="0.01"
                        value={form.price}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="product-description">Description</label>
                    <textarea
                        id="product-description"
                        name="description"
                        value={form.description}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="product-category">Category</label>
                    <input
                        id="product-category"
                        name="category"
                        value={form.category}
                        onChange={handleChange}
                        required
                    />

                    {editingId === null && (
                        <>
                            <label htmlFor="product-image">Image URL</label>
                            <input
                                id="product-image"
                                name="image"
                                value={form.image}
                                onChange={handleChange}
                                required
                            />
                        </>
                    )}

                    <label htmlFor="product-stock">Stock Quantity</label>
                    <input
                        id="product-stock"
                        name="stockQuantity"
                        type="number"
                        min="0"
                        step="1"
                        value={form.stockQuantity}
                        onChange={handleChange}
                        required
                    />

                    <div className="form-actions">
                        <button type="submit" disabled={busy}>
                            {busy
                                ? "Saving..."
                                : editingId !== null
                                    ? "Update Product"
                                    : "Add Product"}
                        </button>

                        {editingId !== null && (
                            <button
                                type="button"
                                onClick={resetForm}
                                disabled={busy}
                            >
                                Cancel Edit
                            </button>
                        )}
                    </div>
                </form>
            </section>

            <section className="admin-section">
                <h2>Manage Products</h2>

                {loading ? (
                    <p>Loading products...</p>
                ) : (
                    <div className="table-wrapper">
                        <table className="admin-table">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Category</th>
                                    <th>Price</th>
                                    <th>Stock</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>

                            <tbody>
                                {products.map(product => (
                                    <tr key={product.id}>
                                        <td>{product.id}</td>
                                        <td>{product.name}</td>
                                        <td>{product.category}</td>
                                        <td>₹{product.price.toLocaleString("en-IN")}</td>
                                        <td>{product.stockQuantity}</td>

                                        <td>
                                            <div className="table-actions">
                                                <button
                                                    type="button"
                                                    onClick={() => startEdit(product)}
                                                    disabled={busy}
                                                >
                                                    Edit
                                                </button>

                                                <button
                                                    type="button"
                                                    className="delete-btn"
                                                    onClick={() => handleDelete(product.id)}
                                                    disabled={busy}
                                                >
                                                    Delete
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </section>
        </main>
    );
}
