using CocktailRecipeLookup.Api.Models;
using CocktailRecipeLookup.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace CocktailRecipeLookup.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksController : ControllerBase
    {
        private readonly IDrinksService _drinksService;

        public DrinksController(IDrinksService drinksService)
        {
            _drinksService = drinksService;
        }

        [HttpGet("ByName/{name}")]
        public async Task<IActionResult> GetDrinksByName(string name)
        {
            var drinks = await _drinksService.GetDrinksByNameAsync(name);
            if (drinks.Count.Equals(0)) return NotFound();
            else return Ok(drinks);
        }
    }
}

