import { useNavigate, useParams } from 'react-router-dom';
import { Card } from '@/components/ui/card';
import { useState, useEffect } from 'react';
import { toast } from 'sonner';
// …other imports…


const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ProfilePage() {
    const { profileId } = useParams();
    const navigate = useNavigate();

    const [replies, setReplies] = useState([]);
    const [replyText, setReplyText] = useState('');
    const [rating, setRating] = useState(0);
    const [profileName, setProfileName] = useState('');
    const [profileAvgRating, setProfileAvgRating] = useState(0);

    useEffect(() => {
        const fetchReplies = async () => {
            try {
                const res = await fetch(
                    `${API_BASE_URL}/api/profiles/${profileId}/replies`,
                    {
                        credentials: 'include',
                    }
                );

                if (res.ok) {
                    const data = await res.json();
                    setReplies(data);
                } else {
                    navigate('/error', {
                        state: {
                            status: res.status,
                            message: 'Error loading reviews for user.',
                        },
                    });
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err },
                });
            }
        };

        const fetchProfileData = async () => {
            try {
                const res = await fetch(
                    `${API_BASE_URL}/api/profiles/${profileId}`,
                    {
                        credentials: 'include',
                    }
                );

                if (res.ok) {
                    const data = await res.json();
                    setProfileName(`${data.firstName} ${data.lastName}`);
                    setProfileAvgRating(data.averageRating);
                } else {
                    navigate('/error', {
                        state: {
                            status: res.status,
                            message: 'Error loading profile data.',
                        },
                    });
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err },
                });
            }
        };

        fetchReplies();
        fetchProfileData();
    }, [profileId, navigate]);

    const createReply = async (e) => {
        e.preventDefault();
        const res = await fetch(
            `${API_BASE_URL}/api/profiles/${profileId}/replies`,
            {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    message: replyText,
                    rating: rating,
                }),
            }
        );

        if (res.ok) {
            setReplyText('');
            setRating(0);
            toast.success('Review submitted!');
        } else {
            toast.warning(
                'There was an error trying to post your review. Please try again later.'
            );
        }
    };

    return (
        <div className="w-full px-4 py-8 max-w-5xl mx-auto space-y-8">
            <h1 className="font-bold text-5xl">{profileName}'s Profile</h1>
            <h3>Average Rating: {profileAvgRating.toFixed(1)}/5.0</h3>
            <Card className="p-6 text-center text-gray-400 italic">
                <h2 className="text-xl font-semibold mb-4">Leave a Review</h2>
                <form className="space-y-4" onSubmit={createReply}>
                    <textarea
                        className="w-full p-2 border text-black rounded resize-none h-24"
                        placeholder="Write your review..."
                        value={replyText}
                        onChange={(e) => setReplyText(e.target.value)}
                        required
                    />

                    <div className="flex items-center space-x-2">
                        <span className="text-gray-700">Rating:</span>
                        {[1, 2, 3, 4, 5].map((star) => (
                            <button
                                type="button"
                                key={star}
                                onClick={() => setRating(star)}
                                className={`text-2xl ${
                                    star <= rating
                                        ? 'text-yellow-400'
                                        : 'text-gray-300'
                                }`}
                            >
                                ★
                            </button>
                        ))}
                    </div>

                    <button
                        type="submit"
                        className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
                    >
                        Submit Reply
                    </button>
                </form>
            </Card>
            <Card className="p-6 space-y-4">
                <h2 className="text-xl font-semibold">Reviews</h2>
                {replies.length === 0 ? (
                    <p className="text-gray-500 italic">No reviews yet.</p>
                ) : (
                    replies.map((reply, index) => (
                        <div key={index} className="border-t pt-4">
                            <div className="flex items-center justify-between mb-2">
                                <p className="font-medium">
                                    {reply.authorName || 'Anonymous'}
                                </p>
                                <div className="flex space-x-1 text-yellow-400 text-lg">
                                    {Array.from({ length: 5 }).map((_, i) => (
                                        <span key={i}>
                                            {i < reply.rating ? '★' : '☆'}
                                        </span>
                                    ))}
                                </div>
                            </div>
                            <p className="text-gray-700 whitespace-pre-line">
                                {reply.message}
                            </p>
                            <p className="text-sm text-gray-400 mt-1">
                                Posted on{' '}
                                {new Date(reply.createdAt).toLocaleDateString()}
                            </p>
                        </div>
                    ))
                )}
            </Card>
        </div>
    );
}
