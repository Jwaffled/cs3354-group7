import { useAuth } from "@/context/AuthContext";

export default function ProfilePage() {
    const { user } = useAuth();

    return (
        <div className="flex flex-col items-center mx-auto bg-white shadow-md rounded-lg p-6 mt-10">
            <h1 className="text-2xl font-bold mb-4">Profile</h1>
            <div className="space-y-2">
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
    );
}