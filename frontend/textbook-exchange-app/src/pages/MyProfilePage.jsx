import { useAuth } from '@/context/AuthContext';

export default function MyProfilePage() {
    const { user } = useAuth();

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 px-4">
            <div className="bg-white text-black shadow-lg rounded-2xl p-8 w-full max-w-md">
                <h1 className="text-3xl font-bold mb-6 text-center">Profile</h1>
                <div className="space-y-4 text-lg">
                    <div>
                        <span className="font-semibold">Email:</span> {user.email}
                    </div>
                    <div>
                        <span className="font-semibold">First Name:</span> {user.firstName}
                    </div>
                    <div>
                        <span className="font-semibold">Last Name:</span> {user.lastName}
                    </div>
                </div>
            </div>
        </div>
    );
}
