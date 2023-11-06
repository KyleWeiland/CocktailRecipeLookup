import axios from 'axios';

const drinksApi = axios.create({
    baseURL: 'https://api-kyle.weiland.com/api/',
    headers: {
        'Content-Type': 'application/json',
    }
});

export const getDrinksByName = async name => {
    try {
        const response = await drinksApi.get(`Drinks/ByName/${name}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching cocktail details:", error);
        throw error;
    }
};