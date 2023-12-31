﻿using CocktailRecipeLookup.Api.Services;
using Microsoft.AspNetCore.Mvc;

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
            try
            {
                var drinks = await _drinksService.GetDrinksByNameAsync(name);
                if (drinks.Count.Equals(0)) return NotFound();
                else return Ok(drinks);
            }
            catch (Exception e)
            {
                return BadRequest($"Exception: {e.Message ?? ""},\tInner Exception: {e.InnerException?.Message ?? ""}");
            }
            
        }

        [HttpPost("ByIngredients")]
        public async Task<IActionResult> GetDrinksByIngredients(List<string> ingredients)
        {
            var drinks = await _drinksService.GetDrinksByIngredientsAsync(ingredients);
            if (drinks.Count.Equals(0)) return NotFound();
            else return Ok(drinks);
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Success!");
        }
    }
}

