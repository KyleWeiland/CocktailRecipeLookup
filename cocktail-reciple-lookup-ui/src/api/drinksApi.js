import axios from 'axios';

const drinksApi = axios.create({
    baseURL: 'http://drink-api.us-east-1.elasticbeanstalk.com/api/',
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
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