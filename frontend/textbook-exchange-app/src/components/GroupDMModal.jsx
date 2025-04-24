import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from '@/components/ui/dialog';
import {
    Command,
    CommandEmpty,
    CommandGroup,
    CommandInput,
    CommandItem,
} from '@/components/ui/command';
import { Checkbox } from '@/components/ui/checkbox';
import { Button } from '@/components/ui/button';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@/context/AuthContext';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export function GroupDMModal({ open, onOpenChange, onStartChat }) {
    const { user } = useAuth();
    const [users, setUsers] = useState([]);
    const [selectedUserIds, setSelectedUserIds] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}/api/profiles`, {
                    credentials: 'include',
                });

                if (res.ok) {
                    let data = await res.json();
                    data = data.filter((u) => u.id !== user.id);
                    setUsers(data);
                } else {
                    navigate('/error', {
                        state: {
                            status: res.status,
                            message: 'Error loading users.',
                        },
                    });
                }
            } catch (err) {
                navigate('/error', {
                    state: { message: err },
                });
            }
        };

        fetchUsers();
    }, [navigate]);

    const toggleUser = (id) => {
        setSelectedUserIds((prev) =>
            prev.includes(id) ? prev.filter((uid) => uid !== id) : [...prev, id]
        );
    };

    const handleStart = () => {
        onStartChat(selectedUserIds);
        onOpenChange(false);
        setSelectedUserIds([]);
    };

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent className="max-w-md max-h-1/2 flex flex-col">
                <DialogHeader>
                    <DialogTitle>Select users to DM</DialogTitle>
                </DialogHeader>

                <Command className="rounded-lg broder h-full">
                    <CommandInput placeholder="Search users..." />
                    <CommandEmpty>No users found.</CommandEmpty>
                    <div className="flex-1 overflow-y-auto">
                        <CommandGroup>
                            {users.map((user) => (
                                <CommandItem
                                    key={user.id}
                                    value={`${user.id}-${user.firstName}-${user.lastName}`}
                                    onMouseDown={(e) => {
                                        e.preventDefault();
                                        toggleUser(user.id);
                                    }}
                                >
                                    <Checkbox
                                        checked={selectedUserIds.includes(
                                            user.id
                                        )}
                                        className="mr-2"
                                    />
                                    {user.firstName} {user.lastName}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </div>
                </Command>

                <DialogFooter>
                    <Button
                        onClick={handleStart}
                        disabled={selectedUserIds.length === 0}
                    >
                        Start Chat
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
