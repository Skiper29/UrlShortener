import axiosInstance from './axiosInstance';
import type { AboutContentResponse, AboutContentUpdateRequest } from '../types/about.types';
import { API_ROUTES } from "../constants/api-routes.constants";

export const aboutApi = {
    async get(): Promise<AboutContentResponse> {
        const response = await axiosInstance.get(API_ROUTES.ABOUT.BASE);
        return response.data;
    },
    async update(data: AboutContentUpdateRequest): Promise<AboutContentResponse> {
        const response = await axiosInstance.patch(API_ROUTES.ABOUT.BASE, data);
        return response.data;
    }
};