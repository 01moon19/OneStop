
import { useEffect, useState } from "react";
import { useAuth } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import { placeOrder } from "../services/orderService";
import {
    getCart,
    updateCartItem,
    removeCartItem
} from "../services/cartService";

export default function Cart() {
    const { token } = useAuth();

    const [cart, setCart] = useState(null);
    const [loading, setLoading] = useState(true);
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState("");

    
    const navigate = useNavigate();

    const items = cart?.items ?? [];

    const total = items.reduce(
        (sum, item) => sum + item.unitPrice * item.quantity,
        0
    );

    async function handlePlaceOrder() {
        if (busy || items.length === 0) return;

        setBusy(true);
        setError("");

        try {
            await placeOrder(token);
            navigate("/orders");
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }



    useEffect(() => {
        let cancelled = false;

        async function fetchCart() {
            try {
                const data = await getCart(token);

                if (!cancelled) {
                    setCart(data);
                    setError("");
                }
            } catch (err) {
                if (!cancelled) {
                    setError(err.message);
                }
            } finally {
                if (!cancelled) {
                    setLoading(false);
                }
            }
        }

        fetchCart();

        return () => {
            cancelled = true;
        };
    }, [token]);


    async function loadCart() {
        const data = await getCart(token);
        setCart(data);
    }

    async function changeQuantity(productId, quantity) {
        if (quantity < 1 || busy) return;

        setBusy(true);
        setError("");

        try {
            await updateCartItem(token, productId, quantity);
            await loadCart();
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    async function removeItem(productId) {
        if (busy) return;

        setBusy(true);
        setError("");

        try {
            await removeCartItem(token, productId);
            await loadCart();
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <main>Loading cart...</main>;

    return (
        <main className="cart-page">
            <h1>Shopping Cart</h1>

            {error && <p role="alert">{error}</p>}

            {items.length === 0 ? (
                <p>Your cart is empty.</p>
            ) : (
                <>
                    {items.map((item) => (
                        <div className="cart-item" key={item.productId}>
                            <h3>{item.productName}</h3>

                            <p>
                                ₹{item.unitPrice.toLocaleString("en-IN")}
                            </p>

                            <div className="quantity-controls">
                                <button
                                    disabled={busy || item.quantity <= 1}
                                    onClick={() =>
                                        changeQuantity(item.productId, item.quantity - 1)
                                    }
                                >
                                    -
                                </button>

                                <span>{item.quantity}</span>

                                <button
                                    disabled={busy}
                                    onClick={() =>
                                        changeQuantity(item.productId, item.quantity + 1)
                                    }
                                >
                                    +
                                </button>
                            </div>

                            <p>
                                Subtotal: ₹{(
                                    item.unitPrice * item.quantity
                                ).toLocaleString("en-IN")}
                            </p>

                            <button
                                disabled={busy}
                                onClick={() => removeItem(item.productId)}
                            >
                                Remove
                            </button>
                        </div>
                    ))}

                    <h2>Total: ₹{total.toLocaleString("en-IN")}</h2>

                        <button
                            type="button"
                            onClick={handlePlaceOrder}
                            disabled={busy || items.length === 0}
                        >
                            {busy ? "Placing Order..." : "Place Order"}
                        </button>
                </>
            )}
        </main>
    );
}
