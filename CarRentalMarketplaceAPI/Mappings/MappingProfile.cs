using AutoMapper;
using CarRentalMarketplaceAPI.DTOs;
using CarRentalMarketplaceAPI.DTOs.Car;
using CarRentalMarketplaceAPI.DTOs.Cart;
using CarRentalMarketplaceAPI.DTOs.Favorite;
using CarRentalMarketplaceAPI.DTOs.Rental;
using CarRentalMarketplaceAPI.DTOs.Review;
using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CarRentalMarketplaceAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<User, UserProfileDto>();
        CreateMap<UpdateUserDto, User>();

        // Car
        CreateMap<Car, CarListDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId));
        CreateMap<Car, CarDetailDto>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerName, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId));
        CreateMap<Car, OwnerCarsDto>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId));
        CreateMap<CreateCarDto, Car>();
        CreateMap<UpdateCarDto, Car>();

        // Rental
        CreateMap<Rental, RentalDto>()
            .ForMember(dest => dest.CarName, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateRentalDto, Rental>();

        // Favorite
        CreateMap<Favorite, FavoriteDto>();
        CreateMap<AddFavoriteDto, Favorite>();

        // Cart
        CreateMap<CartItem, CartItemDto>();
        CreateMap<AddCartItemDto, CartItem>();

        // Review
        CreateMap<Review, ReviewDto>();
        CreateMap<CreateReviewDto, Review>();
    }
}