import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { Skeleton } from '@/components/ui/skeleton';
import { Card, CardContent } from '@/components/ui/card';
import { toast } from 'sonner';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ListingDetailsPage() {
    const { listingId } = useParams();
    const navigate = useNavigate();

    const [listing, setListing] = useState(null);
    const [loading, setLoading] = useState(true);

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

        fetchListing();
    }, [listingId, navigate]);

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
                        <Link
                            to={`/profile/${listing.createdById}`}
                            className="font-medium text-blue-600 hover:underline hover:text-blue-800 inline-flex items-center gap-1"
                        >
                            {listing.authorName || 'Unknown'}
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                className="w-4 h-4"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke="currentColor"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M17 8l4 4m0 0l-4 4m4-4H3"
                                />
                            </svg>
                        </Link>
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
        </div>
    );
}
