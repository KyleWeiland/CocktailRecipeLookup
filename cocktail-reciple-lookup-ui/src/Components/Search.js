function Search({cocktailName, setCocktailName, checkboxVal, setCheckboxVal, fetchCocktailDetails}) {

    function handleInputKeyDown(key) {
        if (key==='Enter') {
            fetchCocktailDetails()
        }
    }

    return (
        <div className="searchContainer">
            <h1>Cocktail Search</h1>
            <div>
                <input
                    type="text"
                    placeholder={"Enter cocktail " + (checkboxVal ? "ingredients" : "name")}
                    value={cocktailName}
                    onChange={e => setCocktailName(e.target.value)}
                    onKeyDown={e => handleInputKeyDown(e.key)}
                />
                <button onClick={() => fetchCocktailDetails()}>Search</button>
            </div>
            <div>
                <label>Search by ingredients: </label>
                <input type="checkbox" checked = {checkboxVal} onChange={() => setCheckboxVal(!checkboxVal)}></input>
            </div>
        </div>
    );
};

export default Search;