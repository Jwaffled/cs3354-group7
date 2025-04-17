import { useAuth } from '../context/AuthContext';
import { Navigate, Outlet } from 'react-router-dom';

export default function ProtectedRoute() {
    const { user, loading } = useAuth();

    if (loading) {
        return <div className="text-center mt-20">Loading...</div>;
    }

    return user ? <Outlet /> : <Navigate to="/login" replace />;
}
