import axios from 'axios';

const drinksApi = axios.create({
    baseURL: 'https://localhost:7033/api/', 
    //baseUrl: 'https://api.kyle-weiland.com/api/',
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

export const getDrinksByIngredients = async _data => {
    try {
        const response = await drinksApi.post(`Drinks/ByIngredients`, [_data]);
        return response.data;
    } catch (error) {
        console.error("Error fetching cocktail details:", error);
        throw error;
    }
}