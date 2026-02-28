import z from "zod";
import {useState} from "react";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import styles from "./AddUrlForm.module.css";
import {ErrorMessage} from "../../../../components/ui/errors/ErrorMessage.tsx";
import {FormField} from "../../../../components/ui/form-fields/FormField.tsx";
import {Button} from "../../../../components/ui/buttons/Button.tsx";

const schema = z.object({
    originalUrl: z.url("Please enter a valid URL"),
});

type FormData = z.infer<typeof schema>;

interface Props {
    onAdd: (url: string) => Promise<string | null>;
}

export const AddUrlForm = ({ onAdd }: Props) => {
    const [serverError, setServerError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(schema),
    });

    const onSubmit = async (data: FormData) => {
        setIsLoading(true);
        setServerError(null);
        const error = await onAdd(data.originalUrl);
        if (error) {
            setServerError(error);
        } else {
            reset();
        }
        setIsLoading(false);
    };

    return (
        <div className={styles.container}>
            <h3 className={styles.title}>Add New URL</h3>
            <ErrorMessage message={serverError} />
            <form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
                <FormField
                    id="originalUrl"
                    label="Original URL"
                    placeholder="https://example.com/very/long/url"
                    error={errors.originalUrl?.message}
                    {...register('originalUrl')}
                />
                <Button type="submit" isLoading={isLoading}>
                    Shorten
                </Button>
            </form>
        </div>
    );
};