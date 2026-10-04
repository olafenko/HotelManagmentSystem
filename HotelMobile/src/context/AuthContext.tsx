import {createContext, ReactNode, useContext, useEffect, useState} from "react";
import apiService from "../api/apiService.ts";
import {LoginRequest} from "../types/models.ts";
import AsyncStorage from "@react-native-async-storage/async-storage";

interface AuthContextType {

    token: string | null;
    loading: boolean;
    error: string | null;
    login: (data: LoginRequest) => Promise<void>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({children} : {children: ReactNode}) {

    const [token, setToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    
    const login = async (data: LoginRequest) => {

        try {
            setLoading(true);
            setError(null);
            const token = await apiService.login(data);
            await AsyncStorage.setItem("jwt_token",token)
            setToken(token);
        } catch (e) {
            const message = e instanceof Error ? e.message : "Nieprawidłowe dane logowania";
            setError(message);
            throw e;
        } finally {
            setLoading(false);
        }
    };

    const logout = async () => {

        try {
            setLoading(true);
            setError(null);
            setToken(null);
            await AsyncStorage.removeItem("jwt_token");
        } catch (e){
            const message = e instanceof Error ? e.message : "Unknown error";
            setError(message);
            throw e;
        } finally {
            setLoading(false);
        }

    }

    const isUserLoggedIn = async () => {

        try {
            setLoading(true);
            let token = await AsyncStorage.getItem("jwt_token");
            setToken(token);
        } catch (e) {
            console.error("Błąd odczytu jwt z pamięci");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        isUserLoggedIn();
    }, []);
    
    return (
        <AuthContext.Provider value={{
            token,
            loading,
            error,
            login,
            logout
        }}
        >
            { children }
        </AuthContext.Provider>

    );

}

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context){
        throw new Error('useAuth must be used within AuthProvider')
    }
    return context;
}