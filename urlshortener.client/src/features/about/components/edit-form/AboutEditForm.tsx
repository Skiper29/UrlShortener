import z from "zod";
import {useState} from "react";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {ErrorMessage} from "../../../../components/ui/errors/ErrorMessage.tsx";
import styles from "./AboutEditForm.module.css";
import {Button} from "../../../../components/ui/buttons/Button.tsx";

const schema = z.object({
    content: z
        .string()
        .min(1, 'Content is required')
        .max(5000, 'Max 5000 characters'),
});

export type FormData  = z.infer<typeof schema>;

interface Props {
    initialContent: string;
    onSave: (content: string) => Promise<string | null>;
    onCancel: () => void;
}

export const AboutEditForm = ({ initialContent, onSave, onCancel }: Props) => {
    const [serverError, setServerError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const { register, handleSubmit, watch, formState: { errors } } = useForm<FormData>({
        resolver:zodResolver(schema),
        defaultValues: { content: initialContent },
    });

    const contentValue = watch('content');

    const onSubmit = async (data: FormData) => {
        setIsLoading(true);
        setServerError(null);
        const error = await onSave(data.content);
        if (error) setServerError(error);
        setIsLoading(false);
    };

    return (
        <form className={styles.form} onSubmit={handleSubmit(onSubmit)}>
            <ErrorMessage message={serverError} />

            <div className={styles.fieldWrapper}>
        <textarea
            className={`${styles.textarea} ${errors.content ? styles.textareaError : ''}`}
            rows={10}
            {...register('content')}
        />
                <div className={styles.footer}>
                    {errors.content && (
                        <span className={styles.errorText}>{errors.content.message}</span>
                    )}
                    <span className={styles.counter}>
            {contentValue?.length ?? 0} / 5000
          </span>
                </div>
            </div>

            <div className={styles.actions}>
                <Button type="button" variant="ghost" onClick={onCancel}>
                    Cancel
                </Button>
                <Button type="submit" isLoading={isLoading}>
                    Save Changes
                </Button>
            </div>
        </form>
    );
};