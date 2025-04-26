import { useEffect, useState, useRef } from 'react'; 
import * as signalR from '@microsoft/signalr';
import ChatWindow from './ChatWindow';
import { useAuth } from '@/context/AuthContext';
import { GroupDMModal } from '@/components/GroupDMModal';
import { toast } from 'sonner';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function DirectMessages() {
    const { user } = useAuth();
    const [chats, setChats] = useState([]);
    const [messagesMap, setMessagesMap] = useState({});
    const [currentChat, setCurrentChat] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const connectionRef = useRef(null);

    useEffect(() => {
        const fetchChannels = async () => {
            const res = await fetch(`${API_BASE_URL}/api/channels`, {
                credentials: 'include',
            });
            const data = await res.json();
            setChats(
                data.map((c) => ({
                    id: c.id,
                    name: c.channelMemberNames.join(', '),
                    lastMessage: c.lastMessage
                        ? c.lastMessage.length <= 50
                            ? c.lastMessage
                            : c.lastMessage.substring(0, 50) + '...'
                        : '',
                    timestamp: c.lastMessageDate
                        ? new Date(c.lastMessageDate).toLocaleTimeString()
                        : '',
                    initials: c.channelMemberNames
                        .map((n) => n[0])
                        .join('')
                        .toUpperCase(),
                }))
            );
        };
        fetchChannels();
    }, []);

    useEffect(() => {
        const connect = async () => {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl(`${API_BASE_URL}/api/chatHub`, {
                    withCredentials: true,
                })
                .withAutomaticReconnect()
                .build();

            connection.on('ReceiveMessage', (msg) => {
                setMessagesMap((prev) => {
                    const arr = prev[msg.channelId] || [];
                    return {
                        ...prev,
                        [msg.channelId]: [...arr, formatMessage(msg)],
                    };
                });

                setChats((prev) =>
                    prev.map((c) =>
                        c.id === msg.channelId
                            ? {
                                  ...c,
                                  lastMessage:
                                      msg.content.length <= 50
                                          ? msg.content
                                          : msg.content.substring(0, 50) +
                                            '...',
                                  timestamp: new Date(
                                      msg.createdAt
                                  ).toLocaleTimeString(),
                              }
                            : c
                    )
                );
            });

            await connection.start();
            connectionRef.current = connection;
        };

        connect();
    }, []);

    const onStartChat = async (ids) => {
        const res = await fetch(`${API_BASE_URL}/api/channels/dm`, {
            credentials: 'include',
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                userIds: ids,
            }),
        });

        if (res.ok) {
            const data = await res.json();
            if (!chats.find((x) => x.id === data.id)) {
                setChats([
                    ...chats,
                    {
                        id: data.id,
                        name: data.channelMemberNames.join(', '),
                        lastMessage: '',
                        timestamp: '',
                        initials: data.channelMemberNames
                            .map((n) => n[0])
                            .join('')
                            .toUpperCase(),
                    },
                ]);
            }
        } else {
            toast.warning('Failed to create channel.');
        }
    };

    const loadMessages = async (chat) => {
        const res = await fetch(
            `${API_BASE_URL}/api/channels/${chat.id}/messages`,
            {
                credentials: 'include',
            }
        );
        const data = await res.json();
        const formatted = data.map((m) => formatMessage(m));

        setMessagesMap((prev) => ({ ...prev, [chat.id]: formatted }));
        setCurrentChat(chat);
        connectionRef.current?.invoke('JoinChannel', chat.id);
    };

    const handleSendMessage = async (msg) => {
        const res = await fetch(
            `${API_BASE_URL}/api/channels/${currentChat.id}/messages`,
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include',
                body: JSON.stringify({ message: msg.content }),
            }
        );

        if (!res.ok) {
            toast.warning('Failed to send message. Please try again later.');
        }
    };

    const formatMessage = (msg) => ({
        id: msg.id,
        content: msg.content,
        sender: msg.authorId === user.id ? 'You' : msg.authorName,
        timestamp: new Date(msg.createdAt).toLocaleTimeString(),
    });

    const messages = currentChat ? messagesMap[currentChat.id] || [] : [];

    return (
        <div className="flex h-screen overflow-hidden p-4">
            <div className="w-1/3 border-r pr-4 flex flex-col h-full">
                <div className="flex items-center justify-between mb-4">
                    <h1 className="text-2xl font-bold">Direct Messages</h1>
                    <button
                        onClick={() => setIsModalOpen(true)}
                        className="px-3 py-1 text-black rounded shadow bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 hover:brightness-110 transition"
                    >
                        + New Chat
                    </button>
                </div>
                <ul className="flex-1 overflow-y-auto space-y-2">
                    {chats.map((chat) => (
                        <li
                            key={chat.id}
                            onClick={() =>
                                currentChat?.id !== chat.id &&
                                loadMessages(chat)
                            }
                            className={`flex items-center justify-between p-2 rounded-lg cursor-pointer ${
                                currentChat?.id === chat.id
                                    ? 'bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200'
                                    : 'bg-gray-100 hover:bg-gray-200'
                            }`}
                        >
                            <div>
                                <div className="font-semibold">{chat.name}</div>
                                <div className="text-sm text-gray-600">
                                    {chat.lastMessage || 'No messages yet'}
                                </div>
                            </div>
                            <div className="flex items-center space-x-2">
                                <div className="flex flex-col items-end">
                                    <div className="text-xs text-gray-400">
                                        {chat.timestamp}
                                    </div>
                                    <div className="w-8 h-8 bg-blue-200 rounded-full flex items-center justify-center text-blue-600">
                                        {chat.initials}
                                    </div>
                                </div>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>

            <div className="w-2/3 pl-4 flex flex-col h-full">
                {currentChat ? (
                    <ChatWindow
                        key={currentChat.id}
                        chat={currentChat}
                        messages={messages}
                        onSend={handleSendMessage}
                        bubbleClassName="bg-gradient-to-r from-sky-200 via-teal-200 to-emerald-200 text-black"
                    />
                ) : (
                    <div className="flex-grow flex items-center justify-center text-gray-500">
                        No chat selected.
                    </div>
                )}
            </div>
            <GroupDMModal
                open={isModalOpen}
                onOpenChange={setIsModalOpen}
                onStartChat={onStartChat}
            />
        </div>
    );
}
