using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Helpers;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IMapper mapper, IWebHostEnvironment environment, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _environment = environment;
        _passwordHasher = new PasswordHasher<User>();
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
            throw new NotFoundException("User tapılmadı");

        user.FullName = dto.FullName;
        user.Phone = dto.Phone;
        user.DriverLicenseNumber = dto.DriverLicenseNumber;

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null && existingUser.Id != user.Id)
                throw new BadRequestException("Bu email artıq istifadə olunur");

            user.Email = dto.Email;
        }

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
                throw new BadRequestException("Köhnə şifrə daxil edilməlidir");

            var passwordCheck = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.CurrentPassword
            );

            if (passwordCheck == PasswordVerificationResult.Failed)
                throw new BadRequestException("Köhnə şifrə yanlışdır");

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
        }

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
