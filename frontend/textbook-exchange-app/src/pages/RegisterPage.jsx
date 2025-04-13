import { useAuth } from "@/context/AuthContext";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import AuthForm from "@/components/AuthForm";
import { Link } from "react-router-dom";

export default function RegisterPage() {
    const { register } = useAuth();
    const navigate = useNavigate();

    const [formError, setFormError] = useState("");

    const handleRegister = async ({ email, firstName, lastName, password, confirmPassword }) => {
        if (password !== confirmPassword) {
            setFormError("Passwords do not match.");
        } else {
            const { success, message } = await register(email, password, firstName, lastName);
            if (success) {
                navigate('/listings');
            } else {
                setFormError(message);
            }
        }
    }

    return (
        <div className="flex-1 flex flex-col items-center justify-center px-4">
            <div className="w-full max-w-md p-8 bg-white shadow-md rounded-lg">
                <h1 className="text-2xl font-bold mb-4 text-center">Register</h1>
                <AuthForm type="register" onSubmit={handleRegister} />
                {formError && (
                    <p className="text-red-500 text-sm text-center mb-2">{formError}</p>
                )}
                <p className="mt-4 text-center text-sm text-gray-600">
                    Already have an account?{" "}
                    <Link to="/login" className="text-primary underline">
                        Log in here
                    </Link>
                </p>
            </div>
        </div>
    );
}