import React, { useEffect, useState } from 'react'
import { getDrinksByName } from './api/drinksApi'


function CocktailSearch() {
    const [cocktailName, setCocktailName] = useState(''); 
    const [cocktailDetails, setCocktailDetails] = useState(null);
    const [result, setResult] = useState(1);
    const [resultsCount, setResultsCount] = useState(null);
    const [data, setData] = useState(null);

    const fetchCocktailDetails = async () => {
        try {
            const _data = await getDrinksByName(cocktailName);
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
    }

    function nextResult() {
        setResult((prevResult) => (prevResult === resultsCount ? 1 : prevResult + 1));
    }

    useEffect(() => {
        if (data && data.length > 0) {
          setCocktailDetails(data[result - 1]);
        }
      }, [result, data]);

    return (
        <div>
            <h2>Cocktail Search</h2>
            <input
                type="text"
                placeholder="Enter cocktail name"
                value={cocktailName}
                onChange={e => setCocktailName(e.target.value)}
            />
            <button onClick={fetchCocktailDetails}>Search</button>

            <div>
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