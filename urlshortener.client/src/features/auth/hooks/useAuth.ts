import {useState} from "react";
import {useAuthStore} from "../../../store/authStore.ts";
import {useNavigate} from "react-router-dom";
import type {AuthUser} from "../../../types/auth.types.ts";
import {authApi} from "../../../api/authApi.ts";
import { extractApiError } from "../../../utils/extractApiError.ts";

export const useAuth = () => {
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const setAuth = useAuthStore((state) => state.setAuth);
    const logout = useAuthStore((state) => state.logout);
    const navigate = useNavigate();

    const handleAuth = async (action: () => Promise<{token: string; userName: string; email: string; role: string}>) => {
        setError(null);
        setIsLoading(true);
        try {
            const data = await action();
            const user : AuthUser ={
                userName: data.userName,
                email: data.email,
                role: data.role as AuthUser["role"],
            };
            setAuth(user, data.token);
            navigate("/urls");
        } catch (err: unknown) {
            setError(extractApiError(err));
        } finally {
            setIsLoading(false);
        }
    };

    const login = (login: string, password:string) =>
        handleAuth(() => authApi.login({ login, password }));

    const register = (userName: string, email: string, password: string) =>
        handleAuth(() => authApi.register({ userName, email, password }));

    const handleLogout = () => {
        logout();
        navigate("/auth");
    };

    return { login, register, logout: handleLogout, error, isLoading };
}