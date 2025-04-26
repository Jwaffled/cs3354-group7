import { Skeleton } from '@/components/ui/skeleton';
import { Card } from '@/components/ui/card';
import { Link } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'sonner';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ForumDetailsPage() {
    const { forumId } = useParams();
    const navigate = useNavigate();

    const [forumPost, setForumPost] = useState(null);
    const [replies, setReplies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [replyText, setReplyText] = useState('');

    useEffect(() => {
        const fetchForumData = async () => {
            try {
                const [postRes, repliesRes] = await Promise.all([
                    fetch(`${API_BASE_URL}/api/forums/${forumId}`, {
                        credentials: 'include',
                    }),
                    fetch(`${API_BASE_URL}/api/forums/${forumId}/replies`, {
                        credentials: 'include',
                    }),
                ]);

                if (postRes.ok && repliesRes.ok) {
                    const postData = await postRes.json();
                    const repliesData = await repliesRes.json();
                    setForumPost(postData);
                    setReplies(
                        repliesData.sort(
                            (a, b) =>
                                new Date(b.createdAt) - new Date(a.createdAt)
                        )
                    );
                } else {
                    throw new Error('Failed to load forum post.');
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err.message },
                });
            } finally {
                setLoading(false);
            }
        };

        fetchForumData();
    }, [forumId, navigate]);

    const createReply = async (e) => {
        e.preventDefault();

        const res = await fetch(
            `${API_BASE_URL}/api/forums/${forumId}/replies`,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include',
                body: JSON.stringify({ message: replyText }),
            }
        );

        if (res.ok) {
            toast.success('Reply submitted!');
            const newReply = await res.json();
            setReplyText('');
            setReplies([newReply, ...replies]);
        } else {
            toast.warning(
                'There was an error trying to post your reply. Please try again later.'
            );
        }
    };

    if (loading) {
        return (
            <div className="p-8">
                <Skeleton className="w-1/2 h-10 mb-4" />
                <Skeleton className="w-full h-40 mb-4" />
                <Skeleton className="w-full, h-20" />
            </div>
        );
    }

    return (
        <div className="sm:w-1/2 px-4 py-8 max-w-5xl mx-auto space-y-8 bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 min-h-screen">
            <div className="space-y-2">
                <h1 className="text-3xl font-bold">{forumPost.title}</h1>
                <p className="text-gray-700">
                    Posted by{' '}
                    <span className="font-medium">
                        <Link
                            to={`/profile/${forumPost.authorId}`}
                            className="font-medium text-blue-600 hover:underline hover:text-blue-800 inline-flex items-center gap-1"
                        >
                            {forumPost.authorName || 'Unknown'}
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
                    </span>{' '}
                    on {new Date(forumPost.createdAt).toLocaleString()}
                </p>
            </div>
    
            <Card className="p-6 bg-white">
                <h2 className="text-xl font-semibold mb-2">Description</h2>
                <p className="text-gray-700 whitespace-pre-line">
                    {forumPost.description || 'No content provided.'}
                </p>
            </Card>
    
            <Card className="p-6 text-center bg-white text-black">
                <h2 className="text-xl font-semibold mb-4">Leave a Reply</h2>
                <form className="space-y-4" onSubmit={createReply}>
                    <textarea
                        className="w-full p-2 border border-emerald-300 text-black rounded resize-none h-24"
                        placeholder="Write your reply..."
                        value={replyText}
                        onChange={(e) => setReplyText(e.target.value)}
                        required
                    />
                    <button
                        type="submit"
                        className="px-4 py-2 rounded text-black bg-gradient-to-r from-sky-300 via-teal-300 to-emerald-300 hover:opacity-90"
                    >
                        Submit Reply
                    </button>
                </form>
            </Card>
    
            <Card className="p-6 space-y-4 bg-white text-black">
                <h2 className="text-xl font-semibold">Replies</h2>
                {replies.length === 0 ? (
                    <p className="text-gray-500 italic">No replies yet.</p>
                ) : (
                    replies.map((reply, index) => (
                        <div key={index} className="border-t pt-4">
                            <p className="font-medium">
                                <Link
                                    to={`/profile/${reply.authorId}`}
                                    className="font-medium text-blue-600 hover:underline hover:text-blue-800 inline-flex items-center gap-1"
                                >
                                    {reply.authorName || 'Unknown'}
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
                            <p className="text-gray-700 whitespace-pre-line">
                                {reply.message}
                            </p>
                            <p className="text-sm text-gray-500 mt-1">
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