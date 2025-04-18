import { Routes, Route } from 'react-router-dom';
import HomePage from '@/pages/HomePage';
import LoginPage from '@/pages/LoginPage';
import RegisterPage from '@/pages/RegisterPage';
import MainLayout from '@/MainLayout';
import ListingsPage from '@/pages/ListingsPage';
import ProtectedRoute from '@/components/ProtectedRoute';
import ListingDetailsPage from '@/pages/ListingDetailsPage';
import ErrorPage from '@/pages/ErrorPage';
import ListingCreatePage from '@/pages/ListingCreatePage';
import MyProfilePage from '@/pages/MyProfilePage';
import ProfilePage from '@/pages/ProfilePage';

export default function AppRoutes() {
    return (
        <Routes>
            <Route element={<MainLayout />}>
                <Route index path="/" element={<HomePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/error" element={<ErrorPage />} />

                <Route element={<ProtectedRoute />}>
                    <Route path="/listings" element={<ListingsPage />} />
                    <Route
                        path="/listings/create"
                        element={<ListingCreatePage />}
                    />
                    <Route
                        path="/listings/:listingId"
                        element={<ListingDetailsPage />}
                    />

                    <Route path="/profile" element={<MyProfilePage />} />
                    <Route
                        path="/profile/:profileId"
                        element={<ProfilePage />}
                    />
                </Route>
            </Route>
        </Routes>
    );
}
