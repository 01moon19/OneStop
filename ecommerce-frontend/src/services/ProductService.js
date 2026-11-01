
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getProducts(category = null) {
    const url = new URL(`${API_BASE_URL}/api/products`);

    if (category) {
        url.searchParams.set("category", category);
    }

    const response = await fetch(url);

    if (!response.ok) {
        throw new Error(`Failed to load products: ${response.status}`);
    }

    return response.json();
}
