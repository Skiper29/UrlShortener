import z from "zod";
import {useAuth} from "../hooks/useAuth.ts";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import styles from "./AuthForm.module.css";
import {ErrorMessage} from "../../../components/ui/errors/ErrorMessage.tsx";
import {FormField} from "../../../components/ui/form-fields/FormField.tsx";
import {Button} from "../../../components/ui/buttons/Button.tsx";

const schema = z.object({
    userName: z.string().min(1, 'Username is required'),
    email: z.email('Invalid email address'),
    password: z.string().min(1, 'Password is required'),
    confirmPassword: z.string().min(1, 'Confirm Password is required'),
}).refine((data) => data.password === data.confirmPassword, {
    message: 'Passwords do not match',

})

type FormData = z.infer<typeof schema>

export const RegisterForm = () => {
    const { register: registerUser, error, isLoading } = useAuth();
    const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(schema),
    });

    const onSubmit = (data: FormData) =>
        registerUser(data.userName, data.email, data.password);

    return (
        <form className={styles.form} onSubmit={handleSubmit(onSubmit)}>
            <h2 className={styles.title}>Create Account</h2>
            <ErrorMessage message={error} />
            <FormField
                id="userName"
                label="Username"
                placeholder="Choose a username"
                error={errors.userName?.message}
                {...register('userName')}
            />
            <FormField
                id="email"
                label="Email"
                type="email"
                placeholder="your@email.com"
                error={errors.email?.message}
                {...register('email')}
            />
            <FormField
                id="password"
                label="Password"
                type="password"
                placeholder="Min 6 characters"
                error={errors.password?.message}
                {...register('password')}
            />
            <FormField
                id="confirmPassword"
                label="Confirm Password"
                type="password"
                placeholder="Re-enter your password"
                error={errors.confirmPassword?.message}
                {...register('confirmPassword')}
            />
            <Button type="submit" isLoading={isLoading}>
                Register
            </Button>
        </form>
    );
}