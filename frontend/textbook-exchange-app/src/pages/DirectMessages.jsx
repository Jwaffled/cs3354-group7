
import { useState } from 'react';
import ChatWindow from './ChatWindow';


const allUsers = [
  { id: 101, name: 'Alice Smith' },
  { id: 102, name: 'Bob Johnson' },
  { id: 103, name: 'Charlie Lee' },
  { id: 104, name: 'Dana White' },
  { id: 105, name: 'Evan Davis' },
];

const initialDemoMessages = [
  { id: 1, content: 'hello', sender: 'them', timestamp: '11:37:34 PM' },
  { id: 2, content: 'hi', sender: 'me', timestamp: '11:37:35 PM' },
  { id: 3, content: 'u have book?', sender: 'them', timestamp: '11:37:36 PM' },
];

export default function DirectMessages() {

  const [chats, setChats] = useState([
    { id: 1, name: 'John Doe', lastMessage: 'hello', timestamp: '11:37 PM', initials: 'JD' },
    { id: 2, name: 'User 1', lastMessage: '', timestamp: '', initials: 'U1' },
    { id: 3, name: 'User 2', lastMessage: '', timestamp: '', initials: 'U2' },
  ]);


  const [messagesMap, setMessagesMap] = useState({
    1: [...initialDemoMessages],
    2: [],
    3: [],
  });

  const [currentChat, setCurrentChat] = useState(chats[0]);


  const [showSearch, setShowSearch] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');

  const filteredUsers = allUsers.filter((u) =>
    u.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleNewChatClick = () => {
    setShowSearch(true);
    setSearchTerm('');
  };

  const handleSelectUser = (user) => {
    const chatId = Date.now();
    const timestamp = new Date().toLocaleTimeString('en-US', { hour12: true });
    const initials = user.name
      .split(' ')
      .map((w) => w[0])
      .join('')
      .slice(0, 2)
      .toUpperCase();

    const newChat = {
      id: chatId,
      name: user.name,
      lastMessage: '',
      timestamp,
      initials,
    };

    setChats((prev) => [...prev, newChat]);
    setMessagesMap((prev) => ({ ...prev, [chatId]: [] }));
    setCurrentChat(newChat);
    setShowSearch(false);
  };

  const handleDeleteChat = (chatId) => {
    setChats((prev) => prev.filter((c) => c.id !== chatId));
    setMessagesMap((prev) => {
      const { [chatId]: _, ...rest } = prev;
      return rest;
    });
    if (currentChat.id === chatId) {
      const nextChats = chats.filter((c) => c.id !== chatId);
      setCurrentChat(nextChats[0] || null);
    }
  };

  const handleSendMessage = (msg) => {
    setMessagesMap((prev) => ({
      ...prev,
      [currentChat.id]: [...(prev[currentChat.id] || []), msg],
    }));
    setChats((prev) =>
      prev.map((c) =>
        c.id === currentChat.id
          ? { ...c, lastMessage: msg.content, timestamp: msg.timestamp }
          : c
      )
    );
  };

  const messages = messagesMap[currentChat.id] || [];

  return (
    <div className="flex-grow flex p-4">
      <div className="w-1/3 border-r pr-4 flex flex-col">
        <div className="flex items-center justify-between mb-4">
          <h1 className="text-2xl font-bold">Direct Messages</h1>
          <button
            onClick={handleNewChatClick}
            className="px-3 py-1 bg-orange-500 text-white rounded hover:bg-orange-600"
          >
            + New Chat
          </button>
        </div>
        <ul className="flex-1 overflow-y-auto space-y-2">
          {chats.map((chat) => (
            <li
              key={chat.id}
              onClick={() => { setCurrentChat(chat); setShowSearch(false); }}
              className={`flex items-center justify-between p-2 rounded-lg cursor-pointer
                ${currentChat.id === chat.id ? 'bg-gray-200' : 'bg-gray-100 hover:bg-gray-150'}`}
            >
              <div>
                <div className="font-semibold">{chat.name}</div>
                <div className="text-sm text-gray-500">
                  {chat.lastMessage || 'No messages yet'}
                </div>
              </div>
              <div className="flex items-center space-x-2">
                <div className="flex flex-col items-end">
                  <div className="text-xs text-gray-400">{chat.timestamp}</div>
                  <div className="w-8 h-8 bg-blue-200 rounded-full flex items-center justify-center text-blue-600">
                    {chat.initials}
                  </div>
                </div>
                <button
                  onClick={(e) => { e.stopPropagation(); handleDeleteChat(chat.id); }}
                  className="text-red-500 hover:text-red-700 ml-2"
                  title="Delete chat"
                >
                  &times;
                </button>
              </div>
            </li>
          ))}
        </ul>
      </div>

      <div className="w-2/3 pl-4 flex flex-col flex-grow">
        {currentChat ? (
          <ChatWindow
            key={currentChat.id}
            chat={currentChat}
            messages={messages}
            onSend={handleSendMessage}
          />
        ) : (
          <div className="flex-grow flex items-center justify-center text-gray-500">
            No chat selected.
          </div>
        )}
      </div>

      {showSearch && (
        <div className="absolute inset-0 bg-black bg-opacity-25 flex items-center justify-center">
          <div className="bg-white rounded-lg p-6 w-1/3">
            <h2 className="text-lg font-semibold mb-2">Start a new chat</h2>
            <input
              type="text"
              placeholder="Search users..."
              className="w-full mb-4 px-3 py-2 border rounded focus:outline-none"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
            <ul className="max-h-60 overflow-y-auto space-y-2">
              {filteredUsers.map((u) => (
                <li
                  key={u.id}
                  onClick={() => handleSelectUser(u)}
                  className="p-2 hover:bg-gray-100 rounded cursor-pointer"
                >
                  {u.name}
                </li>
              ))}
              {filteredUsers.length === 0 && (
                <li className="text-gray-500">No users found</li>
              )}
            </ul>
            <button
              onClick={() => setShowSearch(false)}
              className="mt-4 px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
            >
              Cancel
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
