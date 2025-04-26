import { useState } from 'react';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';

export default function AuthForm({ type = 'login', onSubmit }) {
    const [form, setForm] = useState({
        email: '',
        password: '',
        firstName: '',
        lastName: '',
        confirmPassword: '',
    });

    const handleChange = (e) => {
        setForm((prev) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(form);
    };

    const isRegister = type === 'register';

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div>
                <Label htmlFor="email" className="mb-2">
                    Email
                </Label>
                <Input
                    type="email"
                    name="email"
                    value={form.email}
                    onChange={handleChange}
                    required
                />
            </div>
            {isRegister && (
                <div>
                    <Label htmlFor="firstName" className="mb-2">
                        First Name
                    </Label>
                    <Input
                        type="text"
                        name="firstName"
                        value={form.firstName}
                        onChange={handleChange}
                        required
                    />
                </div>
            )}
            {isRegister && (
                <div>
                    <Label htmlFor="lastName" className="mb-2">
                        Last Name
                    </Label>
                    <Input
                        type="text"
                        name="lastName"
                        value={form.lastName}
                        onChange={handleChange}
                        required
                    />
                </div>
            )}
            <div>
                <Label htmlFor="password" className="mb-2">
                    Password
                </Label>
                <Input
                    type="password"
                    name="password"
                    value={form.password}
                    onChange={handleChange}
                    required
                />
            </div>
            {isRegister && (
                <div>
                    <Label htmlFor="confirmPassword" className="mb-2">
                        Confirm Password
                    </Label>
                    <Input
                        type="password"
                        name="confirmPassword"
                        value={form.confirmPassword}
                        onChange={handleChange}
                        required
                    />
                </div>
            )}
            <Button
                type="submit"
                className="w-full bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 text-black border border-black hover:brightness-105 transition"
            >
                {isRegister ? 'Register' : 'Login'}
            </Button>
        </form>
    );
}
