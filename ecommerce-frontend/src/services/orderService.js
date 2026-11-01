
const API_URL = import.meta.env.VITE_API_BASE_URL;

async function orderRequest(path, token, options = {}) {
    const response = await fetch(`${API_URL}/api/orders${path}`, {
        ...options,
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
            ...options.headers
        }
    });

    if (!response.ok) {
        const message = await response.text();
        throw new Error(
            `Order request failed (${response.status}): ${message}`
        );
    }

    if (response.status === 204) return null;

    const text = await response.text();
    return text ? JSON.parse(text) : null;
}

export function placeOrder(token) {
    return orderRequest("", token, {
        method: "POST"
    });
}

export function getOrders(token) {
    return orderRequest("/my-orders", token);
}

export function getOrderById(token, orderId) {
    return orderRequest(`/${orderId}`, token);
}
