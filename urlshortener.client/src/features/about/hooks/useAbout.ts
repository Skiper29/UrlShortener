import type {AboutContentResponse} from "../../../types/about.types.ts";
import {useEffect, useState} from "react";
import {aboutApi} from "../../../api/aboutApi.ts";
import {extractApiError} from "../../../utils/extractApiError.ts";

export const useAbout = () => {
    const [about, setAbout] = useState<AboutContentResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        aboutApi.get()
            .then(setAbout)
            .catch((err) => setError(err))
            .finally(() => setIsLoading(false));
    }, []);

    const update = async (content: string): Promise<string | null> => {
        try {
            const updated = await aboutApi.update({ content });
            setAbout(updated);
            return null;
        } catch (err) {
            return extractApiError(err);
        }
    };

    return { about, isLoading, error, update };
};