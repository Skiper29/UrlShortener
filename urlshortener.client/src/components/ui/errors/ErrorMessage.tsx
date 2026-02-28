import styles from './ErrorMessage.module.css';

interface Props {
    message: string;
}

export const ErrorMessage = ({message}: Props) => {
    if (!message) return null;
    return <div className={styles.error}>{message}</div>;
}