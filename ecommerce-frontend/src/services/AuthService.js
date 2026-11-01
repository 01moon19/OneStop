
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

async function postJson(path, data) {
    const response = await fetch(`${API_BASE_URL}${path}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });

    const result = await response.json().catch(() => null);

    if (!response.ok) {
        throw new Error(
            result?.message || "Authentication failed"
        );
    }

    return result;
}

export const loginUser = (data) =>
    postJson("/api/auth/login", data);

export const registerUser = (data) =>
    postJson("/api/auth/register", data);
