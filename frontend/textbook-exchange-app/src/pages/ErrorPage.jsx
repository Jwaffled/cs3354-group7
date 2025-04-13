import { useLocation } from "react-router-dom";

export default function ErrorPage() {
    const location = useLocation();
    const { state } = location;
    const status = state?.status || 500;
    const message = state?.message?.message || state?.message || "Something went wrong!";

    return (
        <div className="h-full flex flex-1 flex-col items-center justify-center px-4 text-center">
            <h1 className="text-5xl font-bold text-red-600 mb-4">Uh oh!</h1>
            <p className="text-xl mb-2">{message}</p>
            <p className="text-gray-500">Status code: {status}</p>
        </div>
    );
}