
import { useState } from "react";
import { AuthContext } from "./AuthContext";
import { loginUser } from "../services/AuthService";

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(null);

    async function login(email, password) {
        const response = await loginUser({ email, password });

        if (!response.token || !response.user) {
            throw new Error("Invalid login response");
        }

        setUser(response.user);
        setToken(response.token);

        return response.user;
    }

    function logout() {
        setUser(null);
        setToken(null);
    }

    return (
        <AuthContext.Provider
            value={{
                user,
                token,
                login,
                logout,
                isAuthenticated: Boolean(token)
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}
