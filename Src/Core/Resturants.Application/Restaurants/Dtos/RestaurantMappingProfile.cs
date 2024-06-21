using AutoMapper;
using Resturants.Application.Restaurants.Commands.CreateRestaurant;
using Resturants.Application.Restaurants.Commands.UpdateRestaurant;
using Resturants.Domain.Entities;

namespace Resturants.Application.Restaurants.Dtos;

public class RestaurantMappingProfile : Profile
{
    public RestaurantMappingProfile()
    {

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(d => d.City, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(d => d.Street, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.PostalCode,opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(d => d.Dishes,opt => opt.MapFrom(src => src.Dishes))
            .ReverseMap();

   
        CreateMap<CreateRestaurantCommand, Restaurant>()
                .ForMember(d => d.Address, opt => opt.MapFrom(src =>
                    new Address()
                    {
                        City = string.IsNullOrEmpty(src.City) ? null : src.City,
                        Street = string.IsNullOrEmpty(src.Street) ? null : src.Street,
                        PostalCode = string.IsNullOrEmpty(src.PostalCode) ? null : src.PostalCode
                    }))
                .ForMember(d => d.ContactNumber, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ContactNumber) ? null : src.ContactNumber))
                .ForMember(d => d.ContactEmail, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ContactEmail) ? null : src.ContactEmail));



        CreateMap<UpdateRestaurantCommand, Restaurant>();
    
    
    }
}
