// src/MainLayout.jsx
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '@/context/AuthContext';
import { Toaster } from 'sonner';
import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuPortal,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuSub,
  DropdownMenuSubTrigger,
  DropdownMenuSubContent,
} from '@/components/ui/dropdown-menu';
import { Button } from '@/components/ui/button';
import { ChevronDown } from 'lucide-react';

const MainLayout = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex flex-col">
      <header className="bg-white shadow p-4 flex items-center justify-between">
        <Link to="/" className="text-xl font-bold text-black">
          UTD Textbook Exchange
        </Link>

        {user && (
          <div className="flex space-x-4 items-center">
            {/* Listings Dropdown */}
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" className="text-black hover:bg-gray-100">
                  Listings <ChevronDown className="w-4 h-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem onClick={() => navigate('/listings')}>
                  Browse Listings
                </DropdownMenuItem>
                <DropdownMenuItem onClick={() => navigate('/listings/create')}>
                  New Listing
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>

            {/* Forums Dropdown */}
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" className="text-black hover:bg-gray-100">
                  Forums <ChevronDown className="w-4 h-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem onClick={() => navigate('/forums')}>
                  View Forums
                </DropdownMenuItem>
                <DropdownMenuItem onClick={() => navigate('/forums/create')}>
                  New Post
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>

            {/* Direct Messages Link */}
            <button
              onClick={() => navigate('/messages')}
              className="text-black hover:bg-gray-100 px-3 py-1 rounded"
            >
              Messages
            </button>

            {/* Profile Dropdown */}
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" className="text-black hover:bg-gray-100">
                  Welcome, {user.firstName} <ChevronDown className="w-4 h-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem onClick={() => navigate('/profile')}>
                  Profile
                </DropdownMenuItem>
                <DropdownMenuItem onClick={() => logout()}>
                  Logout
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        )}
      </header>

      <main className="flex-1 flex flex-col py-4">
        <Outlet />
      </main>

      <Toaster />
    </div>
  );
};

export default MainLayout;
