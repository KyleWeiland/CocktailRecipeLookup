import { useState } from "react";
import Search from "./Components/Search";
import Drinks from "./Components/Drinks";
import { getDrinksByIngredients, getDrinksByName } from './api/drinksApi'
import "./App.scss";

function App() {
  const [cocktailName, setCocktailName] = useState(''); 
  const [data, setData] = useState(null);
  const [checkboxVal, setCheckboxVal] = useState(false);
  const [loading, setLoading] = useState(false);

  const fetchCocktailDetails = async () => {
    setData(null);
    setLoading(true);
    try {
      const _data = (checkboxVal) ? await getDrinksByIngredients(cocktailName) : await getDrinksByName(cocktailName);
      if (_data && _data.length > 0) {
          setData(_data);
      } else {
          console.error("No drinks found for the given name:", cocktailName);
      }
    } catch (error){
        console.error("There was an error fetching coktail details:", error);
    }
    setLoading(false);
  }

  return (
    <div className="centerContainer">
      <div className="navBar">
        <label>Home</label>
        <label>Pages</label>
        <label>About</label>
        <label>Contact</label>
      </div>
      <div className="mainContent">
        <Search 
          cocktailName={cocktailName}
          setCocktailName={setCocktailName}
          checkboxVal={checkboxVal}
          setCheckboxVal={setCheckboxVal} 
          fetchCocktailDetails={fetchCocktailDetails}/>
        <Drinks 
          data={data}
          loading={loading} />
        <div className="footer"></div>
      </div>
    </div>
  );
}

export default App;