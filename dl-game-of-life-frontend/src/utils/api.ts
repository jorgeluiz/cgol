import { ErrorResponse } from "@/types/api";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "";

const defaultHeaders = {
    "Content-Type": "application/json",
    "Accept": "application/json",
};

async function handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
        console.log(API_BASE_URL);
        try {
            const errorData: ErrorResponse = await response.json();
            const firstError = errorData.errors?.[0]?.message || `Communication error: ${response.status}`;
            throw new Error(firstError);
        } catch (e) {
            throw new Error(`Communication error: ${response.status}`);
        }
    }
    return response.json() as Promise<T>;
}

export const get = <TResponse>(uri: string): Promise<TResponse> => {
    return fetch(`${API_BASE_URL}${uri}`, {
        method: 'GET',
        headers: defaultHeaders,
    }).then(response => handleResponse<TResponse>(response));
};

export const post = <TResponse, TRequest>(uri: string, data: TRequest): Promise<TResponse> => {
    return fetch(`${API_BASE_URL}${uri}`, {
        method: 'POST',
        headers: defaultHeaders,
        body: JSON.stringify(data),
    }).then(response => handleResponse<TResponse>(response));
};

export const put = <TResponse, TRequest>(uri: string, data: TRequest): Promise<TResponse> => {
    return fetch(`${API_BASE_URL}${uri}`, {
        method: 'PUT',
        headers: defaultHeaders,
        body: JSON.stringify(data),
    }).then(response => handleResponse<TResponse>(response));
};

export const deleteRequest = <TResponse>(uri: string): Promise<TResponse> => {
    return fetch(`${API_BASE_URL}${uri}`, {
        method: 'DELETE',
        headers: defaultHeaders,
    }).then(response => handleResponse<TResponse>(response));
};