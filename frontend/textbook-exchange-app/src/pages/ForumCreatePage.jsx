import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ForumCreatePage() {
    const [form, setForm] = useState({
        title: '',
        description: '',
    });

    const [submitting, setSubmitting] = useState(false);
    const navigate = useNavigate();

    const handleChange = (e) => {
        setForm((prev) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSubmitting(true);

        try {
            await fetch(`${API_BASE_URL}/api/forums`, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(form),
            });

            navigate('/forums', {
                state: {
                    showToast: true,
                    toastMessage: 'Forum post created successfully!',
                },
            });
        } catch (err) {
            navigate('/error', {
                state: { message: err },
            });
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="min-h-screen bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 flex justify-center items-start py-12 px-4">
            <div className="w-full max-w-2xl bg-white text-black rounded-lg shadow-md p-8">
                <h1 className="text-3xl font-bold mb-6">Create a New Forum Post</h1>

                <form onSubmit={handleSubmit} className="space-y-6">
                    <div>
                        <Label htmlFor="title" className="mb-1 block">
                            Title
                        </Label>
                        <Input
                            name="title"
                            value={form.title}
                            onChange={handleChange}
                            required
                            className="border border-emerald-300 focus:ring-emerald-500"
                        />
                    </div>

                    <div>
                        <Label htmlFor="description" className="mb-1 block">
                            Description
                        </Label>
                        <Textarea
                            name="description"
                            value={form.description}
                            onChange={handleChange}
                            rows={6}
                            required
                            className="border border-emerald-300 focus:ring-emerald-500"
                        />
                    </div>

                    <Button
                        type="submit"
                        className="w-full bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 hover:opacity-90 text-black"
                        disabled={submitting}
                    >
                        {submitting ? 'Posting...' : 'Create Post'}
                    </Button>
                </form>
            </div>
        </div>
    );
}
