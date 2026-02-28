import {useAuthStore} from "../../store/authStore.ts";
import {useAuth} from "../../features/auth/hooks/useAuth.ts";
import styles from "./Navbar.module.css";
import {Link} from "react-router-dom";
import {Button} from "../ui/buttons/Button.tsx";

export const Navbar = () => {
    const {user, isAuthenticated,} = useAuthStore();
    const {logout} = useAuth();

    return (
        <nav className={styles.nav}>
            <Link to="/urls" className={styles.logo}>
                URL Shortener
            </Link>
            <div className={styles.links}>
                <Link to="/urls" className={styles.navLink}>URLs</Link>
                <Link to="/about" className={styles.navLink}>About</Link>
            </div>
            <div className={styles.actions}>
                {isAuthenticated ? (
                    <>
                        <span className={styles.username}>
                            {user?.userName}
                            {user?.role === 'Admin' && (
                                <span className={styles.adminBadge}>Admin</span>
                            )}
                        </span>
                        <Button variant={"ghost"} size={"sm"} onClick={logout}>
                            Logout
                        </Button>
                    </>
                ) : (
                    <Link to="/auth">
                        <Button size={"sm"}>
                            Sign In
                        </Button>
                    </Link>
                )}
            </div>
        </nav>
    );
}