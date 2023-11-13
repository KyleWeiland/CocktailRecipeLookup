import React, { useEffect, useState } from 'react'
import { getDrinksByIngredients, getDrinksByName } from './api/drinksApi'


function CocktailSearch() {
    const [cocktailName, setCocktailName] = useState(''); 
    const [cocktailDetails, setCocktailDetails] = useState(null);
    const [result, setResult] = useState(1);
    const [resultsCount, setResultsCount] = useState(null);
    const [data, setData] = useState(null);
    const [checkboxVal, setCheckboxVal] = useState(false);
    const [loading, setLoading] = useState(false);

    const fetchCocktailDetails = async () => {
        setCocktailDetails(null);
        setLoading(true);
        try {
            const _data = (checkboxVal) ? await getDrinksByIngredients(cocktailName) : await getDrinksByName(cocktailName);
            if (_data && _data.length > 0) {
                setCocktailDetails(_data[0]);
                setResultsCount(_data.length);
                setResult(1)
                setData(_data);
            } else {
                console.error("No drinks found for the given name:", cocktailName);
            }
        } catch (error){
            console.error("There was an error fetching coktail details:", error);
        }
        setLoading(false);
    }

    function nextResult() {
        setResult((prevResult) => (prevResult === resultsCount ? 1 : prevResult + 1));
    }

    function handleInputKeyDown(key) {
        if (key==='Enter') {
            fetchCocktailDetails()
        }
    }

    useEffect(function displayCocktail() {
        if (data && data.length > 0) {
          setCocktailDetails(data[result - 1]);
        }
      }, [result, data]);

    return (
        <div>
            <h2>Cocktail Search</h2>
            <input
                type="text"
                placeholder={"Enter cocktail " + (checkboxVal ? "ingredients" : "name")}
                value={cocktailName}
                onChange={e => setCocktailName(e.target.value)}
                onKeyDown={e => handleInputKeyDown(e.key)}
            />
            <button onClick={fetchCocktailDetails}>Search</button>
            <div>
                <label>Search by ingredients: </label>
                <input type="checkbox" checked = {checkboxVal} onChange={() => setCheckboxVal(!checkboxVal)}></input>
            </div>
            <div>
                {loading && (
                    <h3>Loading...</h3>
                )}
                {cocktailDetails && (
                    <div>
                        <h5>Result {result} of {resultsCount}</h5>
                        <button onClick={nextResult}>Next Drink</button>
                        <h3>{cocktailDetails.name}</h3>
                        <p>Ingredients:</p>
                        <ul>
                            {cocktailDetails.ingredients.map((ingredient, index) => (
                            <li key={index}>{ingredient}</li>
                            ))}
                        </ul>
                        <p>{cocktailDetails.instructions}</p>
                    </div>
                )}
            </div>
        </div>
    );
}

export default CocktailSearch;
