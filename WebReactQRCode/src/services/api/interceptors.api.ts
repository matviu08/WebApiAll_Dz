import axios from 'axios';
import {API_URL} from "../../config/api.config.ts";
import {AUTH_STORAGE_KEY} from "../../context/AuthContext.tsx";

const instance = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

instance.interceptors.request.use((config) => {
    const token = localStorage.getItem(AUTH_STORAGE_KEY);
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

instance.interceptors.response.use(
    (response) => response,
    (error) => {
        console.error('API Error:', error?.response?.data || error.message);
        return Promise.reject(error);
    },
);

export default instance;