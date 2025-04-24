import { useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { Button } from '@/components/ui/button';
import { toast } from 'sonner';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ListingsPage() {
    const [listings, setListings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [filteredListings, setFilteredListings] = useState([]);

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        if (location.state?.showToast) {
            toast.success(location.state.toastMessage || 'Success!');
            window.history.replaceState({}, document.title);
        }
    }, [location.state]);

    useEffect(() => {
        const fetchListings = async () => {
            try {
                const response = await fetch(`${API_BASE_URL}/api/listings`, {
                    credentials: 'include',
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch listings.');
                }

                const data = await response.json();
                setListings(data);
                setFilteredListings(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchListings();
    }, []);

    useEffect(() => {
        if (searchQuery === '') {
            setFilteredListings(listings);
        } else {
            const filtered = listings.filter((listing) => {
                return listing.title
                    .toLowerCase()
                    .includes(searchQuery.toLowerCase());
            });
            setFilteredListings(filtered);
        }
    }, [searchQuery, listings]);

    if (loading) {
        return (
            <div className="flex items-center justify-center">
                <div className="spinner-border animate-spin inline-block w-8 h-8 border-4 border-t-4 border-blue-500 rounded-full"></div>
            </div>
        );
    }

    if (error) {
        return <div className="text-red-500">{error}</div>;
    }

    return (
        <div className="flex flex-col items-center py-6">
            <div className="w-full sm:w-1/2 flex justify-between items-center mb-6">
                <h2 className="text-xl font-bold">Listings</h2>
                <Button onClick={() => navigate('/listings/create')}>
                    + New Listing
                </Button>
            </div>
            <div className="w-full sm:w-1/2 mb-6">
                <input
                    type="text"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    placeholder="Search by title..."
                    className="w-full px-4 py-2 text-lg border rounded-lg shadow-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
            </div>

            <div className="flex justify-center w-full sm:w-1/2">
                <div className="space-y-6 w-full">
                    {filteredListings.length === 0 && (
                        <div className="w-full flex justify-center">
                            <h1 className="text-lg font-semibold">
                                No listings found.
                            </h1>
                        </div>
                    )}
                    {filteredListings.map((listing) => (
                        <Link
                            key={listing.id}
                            to={`/listings/${listing.id}`}
                            className="flex items-center p-4 bg-white rounded-lg shadow-md hover:shadow-xl transition-all"
                        >
                            <div className="flex-shrink-0">
                                <img
                                    src={
                                        listing.imageUrl ||
                                        'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg'
                                    }
                                    alt={listing.title}
                                    className="w-64 h-36 object-cover rounded-lg"
                                />
                            </div>

                            <div className="ml-6 flex flex-col justify-between flex-grow">
                                <h3 className="text-lg font-semibold mb-2">
                                    {listing.title}
                                </h3>
                                <p className="text-sm text-gray-500">
                                    {listing.condition}
                                </p>
                            </div>

                            <div className="ml-6 flex items-center">
                                <span className="text-2xl font-semibold text-gray-900">
                                    ${listing.price.toFixed(2)}
                                </span>
                            </div>
                        </Link>
                    ))}
                </div>
            </div>
        </div>
    );
}
