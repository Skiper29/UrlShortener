import * as React from "react";
import {useAuthStore} from "../../store/authStore.ts";
import {Navigate} from "react-router-dom";

interface Props {
    children: React.ReactNode;
}

export const ProtectedRoute = ({ children }: Props) => {
    const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

    return isAuthenticated ? <>{children}</> : <Navigate to={"/auth"} replace />;
};