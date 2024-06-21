using AutoMapper;
using Resturants.Application.Dishes.Commands.CreateDish;
using Resturants.Application.Dtos;
using Resturants.Domain.Entities;

namespace Resturants.Application.Dishes.Dtos;

public class DishMappingProfile : Profile
{
    public DishMappingProfile()
    {
        CreateMap<CreateDishCommand, Dish>();

        CreateMap<Dish, DishDto>()
            .ForMember(d => d.RestaurantName, opt => opt.MapFrom(src => src.Restaurant == null ? null : src.Restaurant.Name))
            .ReverseMap();
    }
}
