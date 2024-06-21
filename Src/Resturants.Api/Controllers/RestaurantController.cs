using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Application.Restaurants.Commands.CreateRestaurant;
using Resturants.Application.Restaurants.Commands.DeleteRestaurant;
using Resturants.Application.Restaurants.Queries.FindRestaurant;
using Resturants.Application.Restaurants.Queries.GetAllRestaurants;
using Resturants.Application.Restaurants.Commands.UpdateRestaurant;
using Microsoft.AspNetCore.Authorization;
using Resturants.Domain.Constants;
using Resturants.Infrastructure.Constants;
using Resturants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Azure.Core;


namespace Resturants.Api.Controllers;

[Route("api")]
[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IMediator _mediator;

    public RestaurantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("restaurants")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
    [Authorize(Policy = PoliciesConstants.IsAllowedNationality)]
    public async Task<IActionResult> GetAllResturants([FromQuery] GetAllRestaurantsQuery getAllRestaurantsQuery)
    {  
        var restaurantsDtos = await _mediator.Send(getAllRestaurantsQuery);
        return Ok(restaurantsDtos);
    }

    [HttpGet("restaurants/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]

    public async Task<IActionResult> GetResturantById(int id, [FromQuery] bool includes)
    {
        var resturantsDto = await _mediator.Send(new FindRestaurantQuery { Id = id, Includes = includes, Criteria = res => res.Id == id });
        return Ok(resturantsDto);
    }

    [HttpPost("restaurants")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RestaurantDto))]
    [Authorize(Roles = RolesConstants.Owner)]
    public async Task<IActionResult> AddRestuarant(CreateRestaurantCommand createRestaurantcommand)
    {
        var addedRestuarant = await _mediator.Send(createRestaurantcommand);
        return CreatedAtAction(nameof(GetResturantById), new { addedRestuarant.Id }, addedRestuarant);
    }

    [HttpDelete("restaurants/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> DeleteRestuarant(int id)
    {
        await _mediator.Send(new DeleteRestaurantCommand { Id = id });
        return NoContent();
    }

    [HttpPatch("restaurants/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateRestuarant(int id, UpdateRestaurantCommand updateRestaurantCommand)
    {
        updateRestaurantCommand.SetId(id);
        var updatedRestaurant = await _mediator.Send(updateRestaurantCommand);
        return Ok(updatedRestaurant);
    }

    [HttpPost("restaurants/{id:int}/logo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [Authorize(Roles = RolesConstants.Owner)]
    public async Task<IActionResult> UploadLogo(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not selected");

        var command = new UploadRestaurantLogoCommand()
        {
            RestaurantId = id,
            File = file
        };

        var sasUrlDto = await _mediator.Send(command);

        return Ok(sasUrlDto);
    }

}



