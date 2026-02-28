import styles from "./Input.module.css";
import * as React from "react";

interface Props extends React.InputHTMLAttributes<HTMLInputElement> {
    error?: string;
}

export const Input = ({ error, className = '', ...rest }: Props) => (
    <div className={styles.wrapper}>
        <input
            className={`${styles.input} ${error ? styles.inputError : ''} ${className}`}
            {...rest}
        />
        {error && <span className={styles.errorText}>{error}</span>}
    </div>
)