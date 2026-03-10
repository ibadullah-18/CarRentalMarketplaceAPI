using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Auth;
using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Helpers;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarRentalMarketplaceAPI.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new BadRequestException("Bu email ilə artıq istifadəçi mövcuddur");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            DriverLicenseNumber = dto.DriverLicenseNumber,
            ProfileImageUrl = string.Empty,
            CreatedDate = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new UnauthorizedException("Email və ya şifrə yanlışdır");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedException("Email və ya şifrə yanlışdır");

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        };
    }
}
