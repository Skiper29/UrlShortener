import z from "zod";
import {useAuth} from "../hooks/useAuth.ts";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import styles from "./AuthForm.module.css";
import {ErrorMessage} from "../../../components/ui/errors/ErrorMessage.tsx";
import {FormField} from "../../../components/ui/form-fields/FormField.tsx";
import {Button} from "../../../components/ui/buttons/Button.tsx";

const schema = z.object({
    login: z.string().min(1, 'Login is required'),
    password: z.string().min(1, 'Password is required'),
})

type FormData = z.infer<typeof schema>

export const LoginForm = () => {
    const {login, error, isLoading} = useAuth();
    const { register, handleSubmit, formState: {errors} } = useForm<FormData>({
        resolver: zodResolver(schema),
    });

    const onSubmit = (data: FormData) => login(data.login, data.password);

    return (
        <form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
            <h2 className={styles.title} >Sign In</h2>
            <ErrorMessage message={error}/>
            <FormField
                id="login"
                label="Login"
                placeholder="Enter login"
                error={errors.login?.message}
                {...register('login')}
            />

            <FormField
                id="password"
                label="Password"
                type="password"
                placeholder="Enter password"
                error={errors.password?.message}
                {...register('password')}
            />

            <Button type="submit" isLoading={isLoading}>
                Sign In
            </Button>
        </form>
    );
}