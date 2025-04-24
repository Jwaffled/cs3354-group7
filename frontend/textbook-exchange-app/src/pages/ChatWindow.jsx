
import { useState, useEffect, useRef } from 'react';

export default function ChatWindow({ chat, messages, onSend }) {
  const [newMessage, setNewMessage] = useState('');
  const endRef = useRef(null);


  useEffect(() => {
    endRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const handleSend = () => {
    const text = newMessage.trim();
    if (!text) return;
    const msg = {
      id: Date.now(),
      content: text,
      sender: 'me',
      timestamp: new Date().toLocaleTimeString('en-US', { hour12: true }),
    };
    onSend(msg);
    setNewMessage('');
  };

  return (
    <div className="flex flex-col flex-grow border rounded-lg p-4">

      <div className="border-b pb-2 mb-4">
        <h2 className="text-xl font-bold">Chat with {chat.name}</h2>
      </div>


      <div className="flex-1 overflow-y-auto space-y-4 px-2">
        {messages.map((msg) => (
          <div
            key={msg.id}
            className={`flex ${msg.sender === 'me' ? 'justify-end' : 'justify-start'}`}
          >
            <div
              className={`max-w-xs px-4 py-2 rounded-full
                ${msg.sender === 'me' ? 'bg-gray-200 text-black' : 'bg-white text-black border'}`}
            >
              <div className="text-lg font-bold">{msg.content}</div>
              <div className="text-xs text-gray-500 mt-1">
                {msg.sender === 'me' ? 'You' : chat.name} | {msg.timestamp}
              </div>
            </div>
          </div>
        ))}
        <div ref={endRef} />
      </div>


      <div className="mt-4 flex items-center space-x-2">

        <input
          type="text"
          placeholder="Type a Message..."
          className="flex-grow px-4 py-2 border rounded-full focus:outline-none"
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && handleSend()}
        />
        <button
          onClick={handleSend}
          className="px-4 py-2 bg-green-500 text-white rounded-full hover:bg-green-600"
        >
          Send
        </button>
      </div>
    </div>
  );
}
