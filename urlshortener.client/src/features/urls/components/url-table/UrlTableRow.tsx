import type {UrlResponse} from "../../../../types/url.types.ts";
import type {AuthUser} from "../../../../types/auth.types.ts";
import {useNavigate} from "react-router-dom";
import styles from "./UrlTable.module.css";
import {Button} from "../../../../components/ui/buttons/Button.tsx";
import {DeleteUrlButton} from "../buttons/DeleteUrlButton.tsx";

interface Props {
    url: UrlResponse;
    currentUser: AuthUser | null;
    onDelete: (id: number) => Promise<boolean>;
}

const canDelete = (url: UrlResponse, user: AuthUser | null): boolean => {
    if (!user) return false;
    if (user.role === 'Admin') return true;
    return url.createdBy === user.userName;
};

export const UrlTableRow = ({ url, currentUser, onDelete }: Props) => {
    const  navigate = useNavigate();

    return (
        <tr className={styles.row}>
            <td className={styles.cell}>
                <a
                    href = {url.originalUrl}
                    target="_blank"
                    rel="noreferrer"
                    className={styles.link}
                    title={url.originalUrl}
                >
                    {url.originalUrl.length > 50
                            ? `${url.originalUrl.slice(0, 50)}...`
                            : url.originalUrl}
                </a>
            </td>
            <td className={styles.cell}>
                <a
                    href={`/r/${url.shortCode}`}
                    target="_blank"
                    rel="noreferrer"
                    className={styles.shortLink}
                >
                    {url.shortCode}
                </a>
            </td>
            <td className={styles.cell}>{url.createdBy}</td>
            <td className={styles.cell}>
                {new Date(url.createdAt).toLocaleDateString()}
            </td>
            <td className={`${styles.cell} ${styles.actions}`}>
                <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => navigate(`/urls/${url.id}`)}
                >
                    Info
                </Button>
                {canDelete(url, currentUser) && (
                    <DeleteUrlButton urlId={url.id} onDelete={onDelete} />
                )}
            </td>
        </tr>
    )
}