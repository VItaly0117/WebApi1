import { createContext, useContext, useState } from 'react';
import axios from 'axios';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(JSON.parse(localStorage.getItem('user')));

    const login = async (login, password) => {
        try {
            const response = await axios.post('/api/account/login', { login, password });
            if (response.data.success) {
                localStorage.setItem('user', JSON.stringify(response.data.data));
                setUser(response.data.data);
            }
            return response.data;
        } catch (error) {
            return error.response.data;
        }
    };

    const register = async (email, username, password) => {
        try {
            const response = await axios.post('/api/account/register', { email, username, password });
            if (response.data.success) {
                localStorage.setItem('user', JSON.stringify(response.data.data));
                setUser(response.data.data);
            }
            return response.data;
        } catch (error) {
            return error.response.data;
        }
    };

    const logout = () => {
        localStorage.removeItem('user');
        setUser(null);
    };

    const value = {
        user,
        login,
        register,
        logout,
    };

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
