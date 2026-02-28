import type {UrlResponse} from "../../../../types/url.types.ts";
import type {AuthUser} from "../../../../types/auth.types.ts";
import styles from "./UrlTable.module.css";
import {UrlTableRow} from "./UrlTableRow.tsx";

interface Props {
    urls: UrlResponse[];
    currentUser: AuthUser | null;
    onDelete: (id: number) => Promise<boolean>;
}

export const UrlTable = ({ urls, currentUser, onDelete }: Props) => {
    if(urls.length === 0) {
        return <p className={styles.empty}>No URLs yet. Be the first to add one!</p>;
    }

    return (
        <div className={styles.tableWrapper}>
            <table className={styles.table}>
                <thead>
                <tr>
                    <th className={styles.th}>Original URL</th>
                    <th className={styles.th}>Short Code</th>
                    <th className={styles.th}>Created By</th>
                    <th className={styles.th}>Date</th>
                    <th className={styles.th}>Actions</th>
                </tr>
                </thead>
                <tbody>
                {urls.map((url)=>(
                    <UrlTableRow
                        key={url.id}
                        url={url}
                        currentUser={currentUser}
                        onDelete={onDelete}
                    />
                ))}
                </tbody>
            </table>
        </div>
    );
};