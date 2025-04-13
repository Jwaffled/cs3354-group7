import { createContext, useState, useEffect, useContext } from "react";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    const fetchUser = async () => {
        try {
            const res = await fetch(`${API_BASE_URL}/api/auth/me`, {
                credentials: 'include'
            });

            if (res.ok) {
                const data = await res.json();
                setUser(data);
            } else {
                setUser(null);
            }
        } catch (err) {
            console.error("Failed to fetch user: ", err);
            setUser(null);
        } finally {
            setLoading(false);
        }
    }

    const register = async (email, password, firstName, lastName) => {
        try {
            const res = await fetch(`${API_BASE_URL}/api/auth/create-account`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify({ email, password, firstName, lastName })
            });

            if (!res.ok) {
                const errorData = await res.json();
                if (Array.isArray(errorData)) {
                    const messages = errorData.map((err) => err.description);
                    return { success: false, message: `The following errors occurred: ${messages.join(" | ")}` }
                }
                return { success: false, message: "An unknown error occurred." };
            }

            await login(email, password);
            return { success: true };
        } catch (err) {
            console.error("Registration error: ", err);
            return { success: false, message: "Registration failed. Please try again." };
        }
    }

    const login = async (email, password) => {
        try {
            const res = await fetch(`${API_BASE_URL}/api/auth/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify({ email, password })
            });

            if (!res.ok) {
                return { success: false, message: "Invalid email or password" };
            }

            await fetchUser();
            return { success: true };
        } catch (err) {
            console.error("Login error: ", err);
            return { success: false, message: "Login failed. Please try again." };
        }
    }

    const logout = async () => {
        await fetch(`${API_BASE_URL}/auth/logout`, {
            method: 'POST',
            credentials: 'include'
        });

        setUser(null);
    }

    useEffect(() => {
        fetchUser();
    }, []);

    return (
        <AuthContext.Provider value={{ user, loading, register, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }

    return context;
}