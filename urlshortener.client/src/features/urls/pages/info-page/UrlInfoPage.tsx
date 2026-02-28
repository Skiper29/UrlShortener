import styles from "./UrlInfoPage.module.css";
import {useNavigate, useParams} from "react-router-dom";
import type {UrlDetailResponse} from "../../../../types/url.types.ts";
import {useEffect, useState} from "react";
import {urlsApi} from "../../../../api/urlsApi.ts";
import {Button} from "../../../../components/ui/buttons/Button.tsx";
import {ErrorMessage} from "../../../../components/ui/errors/ErrorMessage.tsx";

interface InfoRowProps {
    label: string;
    value: string;
    isLink?: boolean;
}

const InfoRow = ({ label, value, isLink}: InfoRowProps) => (
    <div className={styles.row}>
        <span className={styles.label}>{label}</span>
        {isLink ? (
            <a href={value} target="_blank" rel="noreferrer" className={styles.link}>
                {value}
            </a>
        ) : (
            <span className={styles.value}>{value}</span>
        )}
    </div>
);

export const UrlInfoPage = () => {
    const { id } = useParams<{id: string}>();
    const navigate = useNavigate();
    const [url, setUrl] = useState<UrlDetailResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!id) return;
        urlsApi.getById(Number(id))
            .then(setUrl)
            .catch((err) => setError(err))
            .finally(() => setIsLoading(false));
    } , [id]);

    if (isLoading) return <div className={styles.center}>Loading...</div>;

    return (
        <div className={styles.page}>
            <Button variant="ghost" size="sm" onClick={() => navigate('/urls')}>
                ← Back
            </Button>

            <div className={styles.card}>
                <h1 className={styles.title}>URL Details</h1>
                <ErrorMessage message={error} />

                {url && (
                    <div className={styles.info}>
                        <InfoRow label="Original URL:" value={url.originalUrl} isLink />
                        <InfoRow
                            label="Short URL"
                            value={`/r/${url.shortCode}`}
                            isLink
                        />
                        <InfoRow label="Short Code" value={url.shortCode} />
                        <InfoRow label="Created By:" value={url.createdByUserName} />
                        <InfoRow label="Email" value={url.createdByEmail} />
                        <InfoRow label="Created At:" value={new Date(url.createdAt).toLocaleString()} />
                    </div>
                )}
            </div>
        </div>
    );
};