import styles from './FormField.module.css';
import { Input } from '../inputs/Input.tsx';
import * as React from "react";

interface Props extends React.InputHTMLAttributes<HTMLInputElement> {
    label: string;
    error?: string;
}

export const FormField = ({ label, error, id, ...rest }: Props) => (
    <div className={`${label.length > 0 ? styles.field : styles.fieldWithoutGap}`}>
        <label className={styles.label} htmlFor={id}>
            {label}
        </label>
        <Input id={id} error={error} {...rest} />
    </div>
);
