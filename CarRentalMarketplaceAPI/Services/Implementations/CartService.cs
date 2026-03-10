using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Cart;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        ICarRepository carRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartAsync(Guid userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
            throw new NotFoundException("Səbət tapılmadı");

        var items = await _cartItemRepository.GetItemsByCartIdAsync(cart.Id);

        var itemDtos = new List<CartItemDto>();

        foreach (var item in items)
        {
            var car = await _carRepository.GetByIdAsync(item.CarId);

            if (car != null)
            {
                itemDtos.Add(new CartItemDto
                {
                    Id = item.Id,
                    CarId = item.CarId,
                    Brand = car.Brand,
                    Model = car.Model,
                    Color = car.Color,
                    PricePerDay = car.PricePerDay,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    TotalPrice = item.TotalPrice
                });
            }
        }

        return new CartDto
        {
            Id = cart.Id,
            Items = itemDtos
        };
    }

    public async Task AddItemAsync(Guid userId, AddCartItemDto dto)
    {
        var car = await _carRepository.GetByIdAsync(dto.CarId);

        if (car == null)
            throw new NotFoundException("Maşın tapılmadı");

        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            await _cartRepository.AddAsync(cart);
        }

        var totalDays = (dto.EndDate - dto.StartDate).Days;

        if (totalDays <= 0)
            throw new BadRequestException("Tarix aralığı düzgün deyil");

        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cart.Id,
            CarId = dto.CarId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            TotalPrice = totalDays * car.PricePerDay
        };

        await _cartItemRepository.AddAsync(cartItem);
    }

    public async Task RemoveItemAsync(Guid itemId)
    {
        await _cartItemRepository.RemoveAsync(itemId);
    }
}
