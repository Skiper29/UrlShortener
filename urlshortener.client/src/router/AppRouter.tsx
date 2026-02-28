import {BrowserRouter, Route, Routes} from 'react-router-dom';
import {AuthPage} from "../features/auth/pages/AuthPage.tsx";

export const AppRouter = () => (
    <BrowserRouter>
        <Routes>
            <Route path="/auth" element={<AuthPage />} />
        </Routes>
    </BrowserRouter>
);