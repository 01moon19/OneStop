
import { useEffect, useState } from "react";
import { useAuth } from "../hooks/useAuth";
import { getOrders } from "../services/orderService";

export default function Orders() {
    const { token } = useAuth();

    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let cancelled = false;

        async function fetchOrders() {
            try {
                const data = await getOrders(token);

                if (!cancelled) {
                    setOrders(Array.isArray(data) ? data : []);
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

        fetchOrders();

        return () => {
            cancelled = true;
        };
    }, [token]);

    if (loading) {
        return <main>Loading orders...</main>;
    }

    return (
        <main className="orders-page">
            <h1>My Orders</h1>

            {error && <p role="alert">{error}</p>}

            {!error && orders.length === 0 && (
                <p>You haven't placed any orders yet.</p>
            )}

            {!error && (
                <div className="orders-list">
                    {orders.map((order) => (
                        <article className="order-card" key={order.orderId}>
                            <h3>Order #{order.orderId}</h3>

                            <p>
                                Date:{" "}
                                {new Date(order.orderDate).toLocaleDateString("en-IN")}
                            </p>

                            <p>
                                Total: ₹
                                {Number(order.totalAmount ?? 0).toLocaleString("en-IN")}
                            </p>

                            <p>
                                Status: {String(order.orderStatus ?? "Pending")}
                            </p>

                            <h4>Items</h4>

                            {(order.items ?? order.orderItems ?? []).map(
                                (item, index) => (
                                    <div key={item.orderItemId ?? index}>
                                        <span>
                                            {item.productName ??
                                                `Product #${item.productId}`}
                                        </span>
                                        {" — "}
                                        <span>Qty: {item.quantity ?? 1}</span>
                                        {" — "}
                                        <span>
                                            ₹
                                            {Number(item.unitPrice ?? 0)
                                                .toLocaleString("en-IN")}
                                        </span>
                                    </div>
                                )
                            )}
                        </article>
                    ))}
                </div>
            )}
        </main>
    );
}
