import { useState } from "react"
import { Input } from "../ui/input";

const MessageInput = ({ onSend }) => {
    const [text, setText] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        if(!text.trim()) return;
        onSend(text);
        setText("");
    };
    
    return (
        <form onSubmit={handleSubmit} className="flex space-x-2">
            <input 
                type="text"
                value={text}
                onChange={(e) => setText(e.target.value)}
                className="flex-1 border rounded px-2 py-1"
                placeholder="Message..."
            />
            <button type="submit" className="bg-blue-500 text-white px-4 py-1 rounded">
                Send 
            </button>
        </form>
    );
};

export default MessageInput;