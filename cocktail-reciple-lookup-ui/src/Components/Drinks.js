import { useEffect, useState } from 'react'

function Drinks({ data, loading }) {
  const [cocktailDetails, setCocktailDetails] = useState(null);
  const [result, setResult] = useState(1);
  const [resultsCount, setResultsCount] = useState(null);

  function incrementResult(value) {
    if (value > 0) {
      setResult((prevResult) => (prevResult === resultsCount ? 1 : prevResult + 1));
    } else {
      setResult((prevResult) => (prevResult === 1 ? resultsCount : prevResult - 1));
    }
  };

  useEffect(() => {
    if (data && data.length > 0) {
      setCocktailDetails(data[0]);
      setResultsCount(data.length);
      setResult(1);
    }
  }, [data]);

  useEffect(() => {
    if (data && data.length > 0) {
      setCocktailDetails(data[result - 1]);
    }
  }, [result, data]);

  return (
    <div className="drinkContainer">
      {loading && (
        <div className='loadingContainer'>
          <h3>Loading...</h3>
        </div>
      )}
      {data && cocktailDetails && (
        <>
          <div className="previousArrow">
            <button onClick={() => incrementResult(-1)}>&lt;</button>
          </div>
          <div className="drinkContainerMain">
            <div className="drinkName">
              <h2>{cocktailDetails.name}</h2>
            </div>
            <div className="ingredients">
              <h5>Ingredients:</h5>
              <ul>
                {cocktailDetails.ingredients.map((ingredient, index) => (
                  <li key={index}>{ingredient}</li>))
                }
              </ul>
            </div>
            <div className="directions">
              <h5>Directions:</h5>
              <p>{cocktailDetails.instructions}</p>
            </div>
            <div className="results">
              <h5>Result {result} of {resultsCount}</h5>
            </div>
          </div>
          <div className="nextArrow">
            <button onClick={() => incrementResult(1)}>&gt;</button>
          </div>
        </>
      )}
    </div>
  );
};

export default Drinks;