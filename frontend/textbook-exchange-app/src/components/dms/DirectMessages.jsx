import { useEffect, useState } from "react";
import MessageInput from "./MessageInput";
import MessageThread from "./MessageThread";

const DirectMessages = ({ recipientId }) => {
    const [messages, setMessages] = useState([]);

    useEffect(() => {

}, [recipientId]);

const handleSendMessage = (text) => {
    const newMessage = {
        senderId: 1, 
        recipientId,
        content: text,
        timestamp: new Date().toISOString(),
};

setMessages([...messages, newMessage]);

};

return (
    <div className="space-y-4 p-4 border rounded shadow bg-white max-w-xl mx-auto">
        {/*message thread*/}
        <MessageThread messages = {messages} />

        {/* Input box */} 
        <MessageInput onSend={handleSendMessage} />
    </div>
  );
};

export default DirectMessages;