import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';
import { Label } from "@/components/ui/label";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ForumCreatePage() {
    const [form, setForm] = useState({
        title: "",
        description: "",
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
    }


    return (
        <div className="sm:w-1/2 max-w-2xl mx-auto py-10 px-4">
            <h1 className="text-3xl font-bold mb-6">Create a New Forum Post</h1>

            <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                    <Label htmlFor="title" className="mb-2">
                        Title
                    </Label>
                    <Input
                        name="title"
                        value={form.title}
                        onChange={handleChange}
                        placeholder="Enter your post title"
                        required
                    />
                </div>

                <div>
                    <Label htmlFor="description" className="block text-sm font-medium mb-2">
                        Description
                    </Label>
                    <Textarea
                        name="description"
                        rows={6}
                        value={form.description}
                        onChange={handleChange}
                        placeholder="Write your post..."
                        required
                    />
                </div>

                <Button type="submit" className="w-full" disabled={submitting}>
                    {submitting ? "Posting..." : "Create Post"}
                </Button>
            </form>
        </div>
    );
}