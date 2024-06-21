using AutoMapper;
using Resturants.Application.Restaurants.Commands.CreateRestaurant;
using Resturants.Application.Restaurants.Commands.UpdateRestaurant;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;

namespace Resturants.Application.Tests.Restaurants.Dtos;

[TestFixture]
public class RestaurantMappingProfileTests
{
    private readonly IMapper _mapper;

    public RestaurantMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Test]
    public void CreateMap_FromRestaurantToRestaurantDto_ShallMapCorrectly()
    {
        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                City = "Test City",
                Street = "Test Street",
                PostalCode = "12-345"
            },
            OwnerId = "44444444-4444-4444-4444-44444444444"
        };

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        Assert.That(restaurantDto, Is.Not.Null);
        Assert.That(restaurantDto.Id, Is.EqualTo(restaurant.Id));
        Assert.That(restaurantDto.Name, Is.EqualTo(restaurant.Name));
        Assert.That(restaurantDto.Description, Is.EqualTo(restaurant.Description));
        Assert.That(restaurantDto.Category, Is.EqualTo(restaurant.Category));
        Assert.That(restaurantDto.HasDelivery, Is.EqualTo(restaurant.HasDelivery));
        Assert.That(restaurantDto.City, Is.EqualTo(restaurant.Address.City));
        Assert.That(restaurantDto.Street, Is.EqualTo(restaurant.Address.Street));
        Assert.That(restaurantDto.PostalCode, Is.EqualTo(restaurant.Address.PostalCode));
        Assert.That(restaurantDto.OwnerId, Is.EqualTo(restaurant.OwnerId));

    }

    [Test]
    public void CreateMap_FromUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        var command = new UpdateRestaurantCommand
        {
            Name = "Updated Restaurant",
            Description = "Updated Description",
            HasDelivery = false
        };
        command.SetId(1);

        var restaurant = _mapper.Map<Restaurant>(command);

        Assert.That(restaurant, Is.Not.Null);
        Assert.That(restaurant.Id, Is.EqualTo(command.Id));
        Assert.That(restaurant.Name, Is.EqualTo(command.Name));
        Assert.That(restaurant.Description, Is.EqualTo(command.Description));
        Assert.That(restaurant.HasDelivery, Is.EqualTo(command.HasDelivery));
    }

    [Test]
    public void CreateMap_FromCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "12345"
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        Assert.That(restaurant, Is.Not.Null);
        Assert.That(restaurant.Name, Is.EqualTo(command.Name));
        Assert.That(restaurant.Description, Is.EqualTo(command.Description));
        Assert.That(restaurant.Category, Is.EqualTo(command.Category));
        Assert.That(restaurant.HasDelivery, Is.EqualTo(command.HasDelivery));
        Assert.That(restaurant.ContactEmail, Is.EqualTo(command.ContactEmail));
        Assert.That(restaurant.ContactNumber, Is.EqualTo(command.ContactNumber));
        Assert.That(restaurant.Address, Is.Not.Null);
        Assert.That(restaurant.Address.City, Is.EqualTo(command.City));
        Assert.That(restaurant.Address.Street, Is.EqualTo(command.Street));
        Assert.That(restaurant.Address.PostalCode, Is.EqualTo(command.PostalCode));
    }
}