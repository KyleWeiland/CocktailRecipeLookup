import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://localhost:7033/',
    headers: {
        'Content-Type': 'application/json'
    }
});

export const getDrinksByName = name => {
    return apiClient.get(`.drinks/${name}`);
};