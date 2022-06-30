import {Api} from "./Api";

export const apiClient = () => {
    return new Api({
        baseUrl: process.env.REACT_APP_VA_BACKEND
    });
}