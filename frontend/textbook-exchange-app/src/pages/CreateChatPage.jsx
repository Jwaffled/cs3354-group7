import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const CreateChatPage = () => {
    const [users, setUsers] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}/api/users`, {
                    credentials: "include",
                });
                if (res.ok) {
                    const data = await res.json();
                    setUsers(data);
                } else {
                    throw new Error("Failed to fetch users.");
                }
            } catch (err) {
                toast.error(err.message);
            }
        };

        fetchUsers();
    }, []);

    const handleStartChat = async () => {
        if (!selectedUserId) return;

        try {
            const res = await fetch(`${API_BASE_URL}/api/dms`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({ recipientId: selectedUserId }),
            });

            if (res.ok) {
                const newChat = await res.json();
                navigate(`/dms/${newChat.id}`);
            } else {
                throw new Error("Failed to create chat.");
            }
        } catch (err) {
            toast.error(err.message);
        }
    };

    return (
        <div className="max-w-lg mx-auto px-4 py-6">
            <h2 className="text-2xl font-semibold mb-4">Start a New Chat</h2>
            <select
                className="w-full border border-gray-300 rounded px-3 py-2 mb-4"
                value={selectedUserId}
                onChange={(e) => setSelectedUserId(e.target.value)}
            >
                <option value="">Select a user...</option>
                {users.map((user) => (
                    <option key={user.id} value={user.id}>
                        {user.firstName} {user.lastName}
                    </option>
                ))}
            </select>
            <Button onClick={handleStartChat}>Start Chat</Button>
        </div>
    );
};

export default CreateChatPage;
