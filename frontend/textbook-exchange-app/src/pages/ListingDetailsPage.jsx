import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Skeleton } from '@/components/ui/skeleton';
import { Card, CardContent } from '@/components/ui/card';
import { toast } from 'sonner';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ListingDetailsPage() {
    const { listingId } = useParams();
    const navigate = useNavigate();

    const [listing, setListing] = useState(null);
    const [replies, setReplies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [replyText, setReplyText] = useState('');
    const [rating, setRating] = useState(0);

    useEffect(() => {
        const fetchListing = async () => {
            try {
                const res = await fetch(
                    `${API_BASE_URL}/api/listings/${listingId}/details`,
                    {
                        credentials: 'include',
                    }
                );

                if (res.ok) {
                    const data = await res.json();
                    setListing(data);
                } else {
                    navigate('/error', {
                        state: {
                            status: res.status,
                            message: 'Error loading listing.',
                        },
                    });
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err },
                });
            } finally {
                setLoading(false);
            }
        };

        const fetchReplies = async () => {
            try {
                const res = await fetch(
                    `${API_BASE_URL}/api/replies/get-listing-replies?listingId=${listingId}`,
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
                            message: 'Error loading reviews to listing.',
                        },
                    });
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err },
                });
            }
        };

        fetchListing();
        fetchReplies();
    }, [listingId, navigate]);

    const createReply = async (e) => {
        e.preventDefault();
        const res = await fetch(`${API_BASE_URL}/api/replies/create`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                message: replyText,
                rating: rating,
                listingId: listingId,
            }),
        });

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

    if (loading) {
        return (
            <div className="p-8">
                <Skeleton className="w-1/2 h-10 mb-4" />
                <Skeleton className="w-full h-60 mb-4" />
                <Skeleton className="w-1/4 h-6 mb-2" />
                <Skeleton className="w-full h-20" />
            </div>
        );
    }

    return (
        <div className="px-4 py-8 max-w-5xl mx-auto space-y-8">
            <div className="flex flex-col md:flex-row gap-8">
                <img
                    src={
                        listing.imageUrl ||
                        'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg'
                    }
                    alt="Book cover"
                    className="w-full md:w-1/3 object-cover rounded shadow-md"
                />

                <div className="flex-1 space-y-2">
                    <h1 className="text-3xl font-bold">
                        {listing.title} | ${listing.price.toFixed(2)}
                    </h1>
                    <p className="text-gray-500">
                        Condition:{' '}
                        <span className="font-medium">{listing.condition}</span>
                    </p>
                    <p className="text-gray-500">
                        Posted by:{' '}
                        <span className="font-medium">
                            {listing.authorName || 'Unknown'}
                        </span>
                    </p>
                    <p className="text-gray-500">
                        Posted on:{' '}
                        <span className="font-medium">
                            {new Date(listing.createdAt).toLocaleDateString()}
                        </span>
                    </p>
                </div>
            </div>

            <Card className="p-6">
                <h2 className="text-xl font-semibold mb-2">Description</h2>
                <p className="text-gray-700 whitespace-pre-line">
                    {listing.description || 'No description provided.'}
                </p>
            </Card>

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
