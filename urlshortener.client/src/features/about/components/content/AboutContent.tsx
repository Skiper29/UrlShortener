import styles from "./AboutContent.module.css";

interface Props {
    content: string;
    updatedAt: string;
}

export const AboutContent = ({ content, updatedAt }: Props) => (
    <div className={styles.container}>
        <p className={styles.text}>{content}</p>
        <span className={styles.meta}>
            Last updated: {new Date(updatedAt).toLocaleString()}
        </span>
    </div>
);