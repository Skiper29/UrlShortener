import axiosInstance from "./axiosInstance";
import type { AuthResponse, LoginRequest, RegisterRequest } from "../types/auth.types";
import { API_ROUTES } from "../constants/api-routes.constants";

export const authApi = {
    async login(data: LoginRequest): Promise<AuthResponse> {
        const response = await axiosInstance.post<AuthResponse>(API_ROUTES.AUTH.LOGIN, data);
        return response.data;
    },
    async register(data: RegisterRequest): Promise<AuthResponse> {
        const response = await axiosInstance.post<AuthResponse>(API_ROUTES.AUTH.REGISTER, data);
        return response.data;
    },
}