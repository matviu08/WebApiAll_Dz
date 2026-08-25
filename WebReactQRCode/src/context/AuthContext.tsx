import { createContext, useContext, useState, type FC, type PropsWithChildren } from "react";

interface IAuthState {
    token: string | null;
    email: string | null;
}

interface IAuthContextValue extends IAuthState {
    isAuthenticated: boolean;
    login: (token: string, email: string) => void;
    logout: () => void;
}

const AUTH_STORAGE_KEY = "auth";

const getInitialState = (): IAuthState => {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    if (!raw) return { token: null, email: null };
    try {
        return JSON.parse(raw) as IAuthState;
    } catch {
        return { token: null, email: null };
    }
};

const AuthContext = createContext<IAuthContextValue | null>(null);

export const AuthProvider: FC<PropsWithChildren> = ({ children }) => {
    const [auth, setAuth] = useState<IAuthState>(getInitialState);

    const login = (token: string, email: string) => {
        const newState = { token, email };
        localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(newState));
        setAuth(newState);
    };

    const logout = () => {
        localStorage.removeItem(AUTH_STORAGE_KEY);
        setAuth({ token: null, email: null });
    };

    return (
        <AuthContext.Provider value={{ ...auth, isAuthenticated: !!auth.token, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("useAuth must be used within AuthProvider");
    return ctx;
};