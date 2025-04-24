const MessageThread = ({ messages }) => {
    return (
        <div className="h-64 overflow-y-auto border rounded p-3 bg-gray-50">
            {messages.length === 0 ? (
                <p className="text-gray-500 italic">No messages yet.</p>
            ) : (
                messages.map((msg, index) => (
                    <div key={index} className="mb-2">
                        <strong>User {msg.senderId}:</strong> {msg.content}
                    </div>
                ))
            )}
        </div>
    );
};

export default MessageThread;