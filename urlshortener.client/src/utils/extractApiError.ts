import axios from "axios";
import type { ProblemDetails } from "../types/api.types.ts";

export function extractApiError(error: unknown): string {
    if (!axios.isAxiosError(error)) {
        return "Something went wrong.";
    }

    const data = error.response?.data as ProblemDetails | undefined;

    if (!data) {
        return error.message || "Something went wrong.";
    }

    // Single error message
    if (data.detail) {
        return data.detail;
    }

    // Validation errors
    if (data.errors) {
        const messages = Object.values(data.errors).flat();
        if (messages.length > 0) {
            return messages.join(" ");
        }
    }

    // Fallback to title
    if (data.title) {
        return data.title;
    }

    return "Something went wrong.";
}
