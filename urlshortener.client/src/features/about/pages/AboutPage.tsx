import {useAuthStore} from "../../../store/authStore.ts";
import {useAbout} from "../hooks/useAbout.ts";
import {useState} from "react";
import styles from "./AboutPage.module.css";
import {Button} from "../../../components/ui/buttons/Button.tsx";
import {ErrorMessage} from "../../../components/ui/errors/ErrorMessage.tsx";
import {AboutEditForm} from "../components/edit-form/AboutEditForm.tsx";
import {AboutContent} from "../components/content/AboutContent.tsx";

export const AboutPage = () => {
    const { user } = useAuthStore();
    const { about, isLoading, error, update } = useAbout();
    const [isEditing, setIsEditing] = useState(false);

    const isAdmin = user?.role === 'Admin';

    const handleSave = async (content: string): Promise<string | null> => {
        const error = await update(content);
        if (!error) setIsEditing(false);
        return error;
    };

    return (
        <div className={styles.page}>
            <div className={styles.card}>
                <div className={styles.header}>
                    <div>
                        <h1 className={styles.title}>About</h1>
                        <p className={styles.subtitle}>How our URL shortening algorithm works</p>
                    </div>
                    {isAdmin && !isEditing && about && (
                        <Button size="sm" onClick={() => setIsEditing(true)}>
                            Edit
                        </Button>
                    )}
                </div>

                <ErrorMessage message={error} />

                {isLoading && <p className={styles.loading}>Loading...</p>}

                {!isLoading && about && (
                    isEditing ? (
                        <AboutEditForm
                            initialContent={about.content}
                            onSave={handleSave}
                            onCancel={() => setIsEditing(false)}
                        />
                    ) : (
                        <AboutContent content={about.content} updatedAt={about.updatedAt} />
                    )
                )}
            </div>
        </div>
    );
};