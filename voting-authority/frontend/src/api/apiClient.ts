import {Api} from "../Api";

export const apiClient = () => {
    return new Api({
        baseUrl: "http://localhost:5000"
    });
}