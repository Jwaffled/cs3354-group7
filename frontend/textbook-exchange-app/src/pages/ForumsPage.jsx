import { useState, useEffect } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Link } from 'react-router-dom';
import { toast } from 'sonner';
import { useLocation } from 'react-router-dom';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ForumsPage() {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const location = useLocation();

    useEffect(() => {
        if (location.state?.showToast) {
            toast.success(location.state.toastMessage || 'Success!');
            window.history.replaceState({}, document.title);
        }
    }, [location.state]);

    useEffect(() => {
        const fetchPosts = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}/api/forums`, {
                    credentials: 'include',
                });

                if (res.ok) {
                    const data = await res.json();
                    setPosts(data);
                } else {
                    throw new Error('Failed to fetch forum posts.');
                }
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchPosts();
    }, []);

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
        <div className="sm:w-1/2 max-w-4xl mx-auto px-4 py-8 space-y-4">
            <h1 className="text-2xl font-semibold mb-4">Forum Posts</h1>

            {posts.length === 0 && (
                <p className="text-gray-500 text-center">No posts yet.</p>
            )}

            {posts.map((post) => (
                <Link to={`/forums/${post.id}`} key={post.id}>
                    <Card className="hover:shadow-md transition-shadow duration-200 my-4">
                        <CardContent className="px-4 space-y-2">
                            <div className="flex justify-between items-center">
                                <h2 className="text-lg font-semibold">
                                    {post.title}
                                </h2>
                                <span className="text-sm text-gray-500">
                                    {new Date(post.createdAt).toLocaleString()}
                                </span>
                            </div>
                            <p className="text-sm text-gray-700">
                                {post.preview}
                            </p>
                            <div className="text-xs text-gray-500 flex justify-between">
                                <span>By {post.authorName}</span>
                                <span>
                                    {post.replyCount}{' '}
                                    {post.replyCount === 1
                                        ? 'reply'
                                        : 'replies'}
                                </span>
                            </div>
                        </CardContent>
                    </Card>
                </Link>
            ))}
        </div>
    );
}
