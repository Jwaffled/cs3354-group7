import { useNavigate } from 'react-router-dom';
import { useAuth } from '@/context/AuthContext';
import { Button } from '@/components/ui/button';

export default function HomePage() {
    const navigate = useNavigate();
    const { user } = useAuth();

    const handleClick = () => {
        navigate(user ? '/listings' : '/login');
    };

    return (
        <div className="bg-white h-full flex-1 flex items-center justify-center px-4">
            <div className="relative text-center max-w-2xl w-full py-12">
                <img
                    src="/book.png"
                    alt="Book icon"
                    className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 opacity-50 w-180 h-180 z-0"
                />

                <div className="relative z-10">
                    <h1 className="text-4xl md:text-5xl font-bold mb-3">
                        UTD Textbook Exchange
                    </h1>
                    <p className="text-xl text-gray-600 mb-6">
                        Books by Comets, For Comets
                    </p>
                    <Button
                        onClick={handleClick}
                        className="bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 text-black text-base px-6 py-3 rounded-xl border border-black/20"
                    >
                        Get Started
                    </Button>
                </div>
            </div>
        </div>
    );
}
