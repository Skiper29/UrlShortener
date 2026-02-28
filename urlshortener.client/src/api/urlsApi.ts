import axiosInstance from "./axiosInstance.ts";
import type { CreateUrlRequest, UrlDetailResponse, UrlResponse} from "../types/url.types.ts";
import { API_ROUTES } from "../constants/api-routes.constants.ts";

export const urlsApi = {
    async getAll() : Promise<UrlResponse[]> {
        const response = await axiosInstance.get(API_ROUTES.URL.BASE);
        return response.data;
    },
    async getById(id: number) : Promise<UrlDetailResponse> {
        const response = await axiosInstance.get(`${API_ROUTES.URL.BASE}/${id}`);
        return response.data;
    },
    async create(data: CreateUrlRequest) : Promise<UrlResponse> {
        const response = await axiosInstance.post(API_ROUTES.URL.BASE, data);
        return response.data;
    },
    async delete(id: number) : Promise<void> {
        await axiosInstance.delete(`${API_ROUTES.URL.BASE}/${id}`);
    }
};