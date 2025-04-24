// src/routes/AppRoutes.jsx
import { Routes, Route } from 'react-router-dom';
import HomePage from '@/pages/HomePage';
import LoginPage from '@/pages/LoginPage';
import RegisterPage from '@/pages/RegisterPage';
import MainLayout from '@/MainLayout';
import ListingsPage from '@/pages/ListingsPage';
import ListingDetailsPage from '@/pages/ListingDetailsPage';
import ListingCreatePage from '@/pages/ListingCreatePage';
import MyProfilePage from '@/pages/MyProfilePage';
import ProfilePage from '@/pages/ProfilePage';
import ForumsPage from '@/pages/ForumsPage';
import ForumCreatePage from '@/pages/ForumCreatePage';
import ForumDetailsPage from '@/pages/ForumDetailsPage';
import ErrorPage from '@/pages/ErrorPage';
import ProtectedRoute from '@/components/ProtectedRoute';

// import your new page:
import DirectMessages from '@/pages/DirectMessages';

function AppRoutes() {
    return (
        <Routes>
            <Route element={<MainLayout />}>
                {/* public */}
                <Route index path="/" element={<HomePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />

                {/* protected */}
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

                    {/* direct messages */}
                    <Route path="/messages" element={<DirectMessages />} />

                    <Route path="/profile" element={<MyProfilePage />} />
                    <Route
                        path="/profile/:profileId"
                        element={<ProfilePage />}
                    />

                    <Route path="/forums" element={<ForumsPage />} />
                    <Route
                        path="/forums/create"
                        element={<ForumCreatePage />}
                    />
                    <Route
                        path="/forums/:forumId"
                        element={<ForumDetailsPage />}
                    />
                </Route>

                {/* fallback */}
                <Route path="*" element={<ErrorPage />} />
            </Route>
        </Routes>
    );
}

export default AppRoutes;
