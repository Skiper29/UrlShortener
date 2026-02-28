import {useAuthStore} from "../../../store/authStore.ts";
import {Navigate} from "react-router-dom";
import {useState} from "react";
import styles from "./AuthPage.module.css";
import {LoginForm} from "../components/LoginForm.tsx";
import {RegisterForm} from "../components/RegisterForm.tsx";
import {Button} from "../../../components/ui/buttons/Button.tsx";

export const AuthPage = () => {
    const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
    const [mode, setMode] = useState<'login' | 'register'>('login');

    if (isAuthenticated) {
        return <Navigate to="/urls" replace />;
    }

    return (
        <div className={styles.page}>
            <div className={styles.card}>
                {mode === 'login' ? <LoginForm /> : <RegisterForm />}
                <div className={styles.toggle}>
                    {mode === 'login' ? (
                        <>
                            <span>Don't have an account?</span>
                            <Button variant="ghost" size="sm" onClick={() => setMode('register')}>
                                Register
                            </Button>
                        </>
                    ) : (
                        <>
                            <span>Already have an account?</span>
                            <Button variant="ghost" size="sm" onClick={() => setMode('login')}>
                                Sign In
                            </Button>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
}