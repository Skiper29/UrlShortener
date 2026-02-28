import {useAuthStore} from "../../../../store/authStore.ts";
import {useUrls} from "../../hooks/useUrls.ts";
import styles from "./UrlsPage.module.css";
import {ErrorMessage} from "../../../../components/ui/errors/ErrorMessage.tsx";
import {UrlTable} from "../../components/url-table/UrlTable.tsx";
import {AddUrlForm} from "../../components/add-form/AddUrlForm.tsx";

export const UrlsPage = () => {
    const { user, isAuthenticated } = useAuthStore();
    const { urls, isLoading, error, addUrl, deleteUrl } = useUrls();

    return (
        <div className={styles.page}>
            <div className={styles.header}>
                <h1 className={styles.title}>All URLs</h1>
            </div>

            {isAuthenticated && <AddUrlForm onAdd={addUrl} />}

            {error && <ErrorMessage message={error} />}

            {isLoading ? (
                <div className={styles.loading}>Loading...</div>
            ) : (
                <UrlTable urls={urls} currentUser={user} onDelete={deleteUrl}/>
            )}
        </div>
    );
};