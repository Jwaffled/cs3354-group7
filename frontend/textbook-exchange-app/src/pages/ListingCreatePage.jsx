import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function ListingCreatePage() {
    const navigate = useNavigate();
    const [form, setForm] = useState({
        title: '',
        description: '',
        price: '',
        condition: '',
    });

    const [conditions, setConditions] = useState([]);

    useEffect(() => {
        const fetchConditions = async () => {
            try {
                const response = await fetch(
                    `${API_BASE_URL}/api/listings/conditions`,
                    {
                        credentials: 'include',
                    }
                );

                if (!response.ok) {
                    navigate('/error', {
                        state: {
                            status: response.status,
                            message: 'Error loading condition dropdown.',
                        },
                    });
                } else {
                    const data = await response.json();
                    setConditions(data);
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err },
                });
            }
        };

        fetchConditions();
    }, [navigate]);

    const handleChange = (e) => {
        setForm((prev) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            await fetch(`${API_BASE_URL}/api/listings`, {
                method: 'POST',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    ...form,
                    condition: parseInt(form.condition),
                }),
            });

            navigate('/listings', {
                state: {
                    showToast: true,
                    toastMessage: 'Listing created successfully!',
                },
            });
        } catch (err) {
            navigate('/error', {
                state: { message: err },
            });
        }
    };

    return (
        <div className="max-w-2xl mx-auto py-10 px-4">
            <h1 className="text-3xl font-bold mb-6">Create a New Listing</h1>
            <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                    <Label htmlFor="title" className="mb-2">
                        Title
                    </Label>
                    <Input
                        name="title"
                        value={form.title}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <Label htmlFor="description" className="mb-2">
                        Description
                    </Label>
                    <Textarea
                        name="description"
                        value={form.description}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <Label htmlFor="price" className="mb-2">
                        Price
                    </Label>
                    <Input
                        name="price"
                        type="number"
                        min="0"
                        step="0.01"
                        value={form.price}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <Label htmlFor="condition" className="mb-2">
                        Condition
                    </Label>
                    <select
                        name="condition"
                        value={form.condition}
                        onChange={handleChange}
                        required
                        className="w-full border rounded-md px-3 py-2 mt-1"
                    >
                        <option value="">Select a condition</option>
                        {conditions.map((c) => (
                            <option key={c.value} value={c.value}>
                                {c.label}
                            </option>
                        ))}
                    </select>
                </div>

                <div>
                    <Label htmlFor="imageUrl" className="mb-2">
                        Image URL
                    </Label>
                    <Input
                        name="imageUrl"
                        type="text"
                        value={form.imageUrl}
                        onChange={handleChange}
                        required
                    />
                </div>

                <Button type="submit" className="w-full">
                    Create Listing
                </Button>
            </form>
        </div>
    );
}
