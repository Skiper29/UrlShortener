import {BrowserRouter, Navigate, Route, Routes} from 'react-router-dom';
import {AuthPage} from "../features/auth/pages/AuthPage.tsx";
import {Navbar} from "../components/layout/Navbar.tsx";
import {UrlsPage} from "../features/urls/pages/main-page/UrlsPage.tsx";
import {ProtectedRoute} from "../components/layout/ProtectedRoute.tsx";
import {UrlInfoPage} from "../features/urls/pages/info-page/UrlInfoPage.tsx";
import {AboutPage} from "../features/about/pages/AboutPage.tsx";

export const AppRouter = () => (
    <BrowserRouter>
        <Navbar/>
        <Routes>
            <Route path="/auth" element={<AuthPage />} />
            <Route path="/urls" element={<UrlsPage />} />
            <Route path="/about" element={<AboutPage />} />
            <Route
                path="/urls/:id"
                element={
                    <ProtectedRoute>
                        <UrlInfoPage />
                    </ProtectedRoute>
                }
            />
            <Route path="*" element={<Navigate to="/urls" replace />} />
        </Routes>
    </BrowserRouter>
);