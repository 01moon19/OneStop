
const API_URL = import.meta.env.VITE_API_BASE_URL;

async function cartRequest(path, token, options = {}) {
    const response = await fetch(`${API_URL}/api/Cart${path}`, {
        ...options,
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
            ...options.headers,
        },
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(
            `Cart request failed (${response.status}): ${errorText}`
        );
    }

    if (response.status === 204) return null;

    const text = await response.text();
    return text ? JSON.parse(text) : null;
}

export const getCart = (token) =>
    cartRequest("", token);

export const addToCart = (token, productId, quantity = 1) =>
    cartRequest("/items", token, {
        method: "POST",
        body: JSON.stringify({ productId, quantity }),
    });


export const updateCartItem = (token, productId, quantity) =>
    cartRequest(
        `/items/${productId}?quantity=${encodeURIComponent(quantity)}`,
        token,
        {
            method: "PUT"
        }
    );


export const removeCartItem = (token, productId) =>
    cartRequest(`/items/${productId}`, token, {
        method: "DELETE",
    });
