using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User tapılmadı");

        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User tapılmadı");

        _mapper.Map(dto, user);

        await _userRepository.UpdateAsync(user);
    }
}
