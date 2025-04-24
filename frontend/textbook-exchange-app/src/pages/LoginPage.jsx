import { useAuth } from '@/context/AuthContext';
import { useNavigate, Link } from 'react-router-dom';
import { useState } from 'react';
import AuthForm from '@/components/AuthForm';

export default function LoginPage() {
    const { login } = useAuth();
    const navigate = useNavigate();

    const [formError, setFormError] = useState('');

    const handleLogin = async ({ email, password }) => {
        const { success, message } = await login(email, password);
        if (success) {
            navigate('/listings');
        } else {
            setFormError(message);
        }
    };

    return (
        <div className="flex-1 flex flex-col items-center justify-center px-4">
            <div className="w-full max-w-md p-8 bg-white shadow-md rounded-lg">
                <h1 className="text-2xl font-bold mb-4 text-center">Login</h1>
                <AuthForm type="login" onSubmit={handleLogin} />
                {formError && (
                    <p className="text-red-500 text-sm text-center mb-2">
                        {formError}
                    </p>
                )}
                <p className="mt-4 text-center text-sm text-gray-600">
                    Don’t have an account?{' '}
                    <Link to="/register" className="text-primary underline">
                        Register here
                    </Link>
                </p>
            </div>
        </div>
    );
}
