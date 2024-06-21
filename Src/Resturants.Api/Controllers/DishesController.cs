using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturants.Application.Dishes.Commands.CreateDish;
using Resturants.Application.Dishes.Commands.DeleteAllDishes;
using Resturants.Application.Dishes.Queries.GetAllDishesOfRestaurant;
using Resturants.Application.Dishes.Queries.GetDishById;
using Resturants.Application.Dtos;
using Resturants.Domain.Constants;

namespace Resturants.Api.Controllers;

[Route("api/restaurants/{restaurantId:int}")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DishesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dishes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DishDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetAllDishes(int restaurantId, [FromQuery] GetAllDishesOfRestaurantQuery getAllDishesOfRestaurantQuery)
    {
        getAllDishesOfRestaurantQuery.RestaurantId = restaurantId;
        var dishesDtos = await _mediator.Send(getAllDishesOfRestaurantQuery);
        return Ok(dishesDtos);
    }


    [HttpGet("dishes/{dishId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DishDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetDishById(int restaurantId, int dishId)
    {
        var dishDto = await _mediator.Send(new FindDishQuery { Includes = ["Restaurant"],Criteria = d => d.Id == dishId, RestaurantId = restaurantId, DishId = dishId});
        return Ok(dishDto);
    }


    [HttpPost("dishes")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(DishDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(string))]
    [Authorize(Roles = $"{RolesConstants.Owner},{RolesConstants.Admin}")]
    public async Task<IActionResult> AddDish(int restaurantId, CreateDishCommand createDishCommand)
    {
        createDishCommand.SetRestaurantId(restaurantId);
        var addedDish = await _mediator.Send(createDishCommand);
        return CreatedAtAction(nameof(GetDishById), new {restaurantId, addedDish.Id}, addedDish);
    }

    [HttpDelete("dishes")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(DishDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [Authorize(Roles = $"{RolesConstants.Owner},{RolesConstants.Admin}")]
    public async Task<IActionResult> DeleteAllDishes(int restaurantId)
    {
        await _mediator.Send(new DeleteAllDishesOfRestaurantCommand { RestaurantId = restaurantId, Includes = ["Dishes"] });
        return NoContent();
    }
}
