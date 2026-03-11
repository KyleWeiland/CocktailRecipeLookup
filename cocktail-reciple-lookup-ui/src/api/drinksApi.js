import axios from 'axios';

const drinksApi = axios.create({
    //baseURL: 'https://localhost:7033/api/', 
    // baseURL: 'https://api.kyle-weiland.com/api/',
    baseURL: 'https://p7d8ipi548.execute-api.us-east-1.amazonaws.com/prod/api/',
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
