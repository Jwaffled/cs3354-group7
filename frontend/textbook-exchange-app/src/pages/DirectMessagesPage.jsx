import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Card, CardContent } from "@/components/ui/card";
import { useAuth } from "@/context/AuthContext";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const DirectMessagesPage = () => {
    const [conversations, setConversations] = useState([]);
    const [loading, setLoading] = useState(true);
    const { user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        const fetchConversations = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}/api/dms`, {
                    credentials: 'include',
                });
                if (res.ok) {
                    const data = await res.json();
                    // Assume each convo has: id, recipientName, lastMessage, updatedAt
                    const sorted = data.sort((a, b) => new Date(b.updatedAt) - new Date(a.updatedAt));
                    setConversations(sorted);
                } else {
                    throw new Error("Failed to fetch conversations.");
                }
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchConversations();
    }, []);

    if (loading) {
        return <p className="text-center mt-4">Loading DMs...</p>;
    }

    return (
        <div className="max-w-3xl mx-auto px-4 py-6">
            <h2 className="text-2xl font-semibold mb-4">Direct Messages</h2>
            {conversations.length === 0 ? (
                <p className="text-gray-500">No conversations yet.</p>
            ) : (
                conversations.map((convo) => (
                    <Card
                        key={convo.id}
                        className="cursor-pointer hover:bg-gray-50"
                        onClick={() => navigate(`/dms/${convo.id}`)}
                    >
                        <CardContent className="p-4">
                            <div className="flex justify-between items-center">
                                <h3 className="font-semibold">{convo.recipientName}</h3>
                                <span className="text-sm text-gray-500">
                                    {new Date(convo.updatedAt).toLocaleTimeString()}
                                </span>
                            </div>
                            <p className="text-gray-600 text-sm truncate">{convo.lastMessage}</p>
                        </CardContent>
                    </Card>
                ))
            )}
        </div>
    );
};

export default DirectMessagesPage;
