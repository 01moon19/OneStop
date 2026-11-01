
const API_URL = import.meta.env.VITE_API_BASE_URL;

async function request(path, token, options = {}) {
    const response = await fetch(`${API_URL}/api/products${path}`, {
        ...options,
        headers: {
            ...(options.body ? { "Content-Type": "application/json" } : {}),
            Authorization: `Bearer ${token}`,
            ...options.headers
        }
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(
            `Operation failed (${response.status}): ${error}`
        );
    }

    if (response.status === 204) return null;

    return response.json();
}

export function createProduct(token, product) {
    return request("", token, {
        method: "POST",
        body: JSON.stringify(product)
    });
}

export function updateProduct(token, id, product) {
    return request(`/${id}`, token, {
        method: "PUT",
        body: JSON.stringify(product)
    });
}

export function deleteProduct(token, id) {
    return request(`/${id}`, token, {
        method: "DELETE"
    });
}
