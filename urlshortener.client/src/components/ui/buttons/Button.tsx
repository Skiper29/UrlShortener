import * as React from "react";
import styles from "./Button.module.css";

type Variant = 'primary' | 'danger' | 'ghost';
type Size = 'sm' | 'md' | 'lg';

interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: Variant;
    size?: Size;
    isLoading?: boolean;
}

export const Button = ({
    variant = 'primary',
    size = 'md',
    isLoading = false,
    children,
    disabled,
    className = '',
    ...rest
}: Props) => (
    <button
        className={`${styles.btn} ${styles[variant]} ${styles[size]} ${className}`}
        disabled={isLoading || disabled}
        {...rest}
    >
        {isLoading && <span className={styles.spinner} />}

        <span style={{ visibility: isLoading ? 'hidden' : 'visible' }}>
            {children}
        </span>
    </button>
);