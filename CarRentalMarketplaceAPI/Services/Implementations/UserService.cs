using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Helpers;
using CarRentalMarketplaceAPI.Repositories.Interfaces;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public UserService(IUserRepository userRepository, IMapper mapper, IWebHostEnvironment environment)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _environment = environment;
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User tapilmadi");

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            DriverLicenseNumber = user.DriverLicenseNumber,
            ProfileImageUrl = !string.IsNullOrWhiteSpace(user.ProfileImageUrl)
                ? $"/{user.ProfileImageUrl.TrimStart('/')}"
                : null
        };
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User tapilmadi");

        user.FullName = dto.FullName;
        user.Phone = dto.Phone;
        user.DriverLicenseNumber = dto.DriverLicenseNumber;

        if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
        {
            if (!string.IsNullOrWhiteSpace(user.ProfileImageUrl))
            {
                FileUploadHelper.DeleteFile(_environment.WebRootPath, user.ProfileImageUrl);
            }

            var newImagePath = await FileUploadHelper.SaveFileAsync(
                dto.ProfileImage,
                _environment.WebRootPath,
                "images/users");

            user.ProfileImageUrl = newImagePath;
        }

        await _userRepository.UpdateAsync(user);
    }
}
