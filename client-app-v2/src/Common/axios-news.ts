import axios, { AxiosInstance, InternalAxiosRequestConfig, AxiosResponse } from 'axios';
import { Configs } from "./constants";

const axiosInstance: AxiosInstance = axios.create({
	baseURL: Configs.URL,
	timeout: 10000,
	headers: {
		"Content-Type": "application/json",
	},
});

// Add a request interceptor if needed
axiosInstance.interceptors.request.use(
	(config: InternalAxiosRequestConfig) => {
		// You can add authorization tokens or other headers here
		const token = localStorage.getItem('authToken');
		if(token){
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	}
);


axiosInstance.interceptors.response.use(
	(response: AxiosResponse) => {
		return response;
	},
	(error) => {
		// Handle errors globally
		console.error("API call error:", error);
		return Promise.reject(error);
	}
);

const login = async (username: string, password: string): Promise<any> => {
	const response = await axiosInstance.post("/Account/authenticate", { username, password });
	const token = response.data.token;
	localStorage.setItem('authToken', token);
	axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${token}`;
	return response.data;
};

const get = async <T>(url: string, config?: InternalAxiosRequestConfig): Promise<T> => {
	const response = await axiosInstance.get<T>(url, config);
	return response.data;
};

const post = async <T>(url: string, data: any, config?: InternalAxiosRequestConfig): Promise<T> => {
	const response = await axiosInstance.post<T>(url, data, config);
	return response.data;
};

// Function to handle PUT requests
const put = async <T>(url: string, data: any, config?: InternalAxiosRequestConfig): Promise<T> => {
	const response = await axiosInstance.put<T>(url, data, config);
	return response.data;
};

// Function to handle DELETE requests
const del = async <T>(url: string, config?: InternalAxiosRequestConfig): Promise<AxiosResponse> => {
	const response = await axiosInstance.delete<T>(url, config);
	return response;
};

export { login, get, post, put, del };
