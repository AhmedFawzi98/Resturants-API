using Resturants.Domain.Entities;

namespace Resturants.Application.Dtos;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Decimal Price { get; set; }
    public int? KiloCalorie { get; set; }
    public string RestaurantName { get; set; }
}
