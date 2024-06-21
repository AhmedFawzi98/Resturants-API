using Resturants.Application.Dtos;
using Resturants.Domain.Entities;
using System.Text.Json.Serialization;

namespace Resturants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool HasDelivery { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    [JsonIgnore]
    public string? LogoUrl { get; set; }
    public string? SasLogoUrl { get; set; }

    public ICollection<DishDto> Dishes { get; set; } = new List<DishDto>();
    public string OwnerId { get; set; }
}
