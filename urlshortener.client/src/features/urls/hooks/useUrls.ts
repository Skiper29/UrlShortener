import {useCallback, useEffect, useState} from "react";
import {urlsApi} from "../../../api/urlsApi.ts";
import {extractApiError} from "../../../utils/extractApiError.ts";
import type {UrlResponse} from "../../../types/url.types.ts";

export const useUrls = () => {
    const [urls, setUrls] = useState<UrlResponse[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchUrls = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const data = await urlsApi.getAll();
            setUrls(data);
        } catch (err) {
            setError(extractApiError(err));
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => { fetchUrls(); }, [fetchUrls]);

    const addUrl = async (originalUrl: string) : Promise<string | null> => {
        try {
            const created = await urlsApi.create({ originalUrl });
            setUrls((prev) => [created, ...prev]);
            return null;
        } catch (err) {
            return extractApiError(err);
        }
    }

    const deleteUrl = async (id: number): Promise<boolean> => {
        try {
            await urlsApi.delete(id);
            setUrls((prev) => prev.filter(url => url.id !== id));
            return true;
        } catch (err) {
            setError(extractApiError(err));
            return false;
        }
    }

    return { urls, isLoading, error, addUrl, deleteUrl, refetch: fetchUrls };
};