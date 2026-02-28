import {BrowserRouter, Route, Routes} from 'react-router-dom';
import {AuthPage} from "../features/auth/pages/AuthPage.tsx";
import {Navbar} from "../components/layout/Navbar.tsx";

export const AppRouter = () => (
    <BrowserRouter>
        <Navbar/>
        <Routes>
            <Route path="/auth" element={<AuthPage />} />
        </Routes>
    </BrowserRouter>
);