
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { registerUser } from "../services/AuthService";

export default function Register() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        userName: "",
        userEmail: "",
        password: ""
    });

    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    function handleChange(e) {
        const { name, value } = e.target;

        setForm((previous) => ({
            ...previous,
            [name]: value
        }));
    }

    async function handleSubmit(e) {
        e.preventDefault();
        setError("");
        setLoading(true);

        try {
            await registerUser(form);

            // Registration is successful.
            // The user must now log in.
            navigate("/login", {
                replace: true,
                state: { registered: true }
            });
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    return (
        <main className="auth-page">
            <h1>Create Account</h1>

            <form onSubmit={handleSubmit} className="auth-form">
                <label htmlFor="userName">Username</label>
                <input
                    id="userName"
                    type="text"
                    name="userName"
                    value={form.userName}
                    onChange={handleChange}
                    autoComplete="username"
                    required
                />

                <label htmlFor="userEmail">Email</label>
                <input
                    id="userEmail"
                    type="email"
                    name="userEmail"
                    value={form.userEmail}
                    onChange={handleChange}
                    autoComplete="email"
                    required
                />

                <label htmlFor="password">Password</label>
                <input
                    id="password"
                    type="password"
                    name="password"
                    value={form.password}
                    onChange={handleChange}
                    autoComplete="new-password"
                    minLength={8}
                    required
                />

                <button type="submit" disabled={loading}>
                    {loading ? "Creating Account..." : "Register"}
                </button>

                {error && <p role="alert">{error}</p>}

                <p>
                    Already registered? <Link to="/login">Login</Link>
                </p>
            </form>
        </main>
    );
}
