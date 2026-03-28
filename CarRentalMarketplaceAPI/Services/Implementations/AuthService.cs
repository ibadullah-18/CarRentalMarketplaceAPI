using AutoMapper;
using CarRentalMarketplaceAPI.DTOs.Auth;
using CarRentalMarketplaceAPI.DTOs.User;
using CarRentalMarketplaceAPI.Entities;
using CarRentalMarketplaceAPI.Exceptions;
using CarRentalMarketplaceAPI.Helpers;
using CarRentalMarketplaceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarRentalMarketplaceAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _mapper = mapper;
            _configuration = configuration;
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

            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshTokenValue = _refreshTokenGenerator.GenerateRefreshToken();
            var refreshTokenDays = int.Parse(_configuration["Jwt:RefreshTokenDays"]!);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshTokenValue,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshTokenValue,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);

            if (existingRefreshToken == null)
                throw new UnauthorizedException("Refresh token tapılmadı");

            if (existingRefreshToken.IsRevoked)
                throw new UnauthorizedException("Refresh token ləğv olunub");

            if (existingRefreshToken.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedException("Refresh token vaxtı bitib");

            var user = await _userRepository.GetByIdAsync(existingRefreshToken.UserId);

            if (user == null)
                throw new NotFoundException("User tapılmadı");

            existingRefreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(existingRefreshToken);

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user);
            var newRefreshTokenValue = _refreshTokenGenerator.GenerateRefreshToken();
            var refreshTokenDays = int.Parse(_configuration["Jwt:RefreshTokenDays"]!);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshTokenValue,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshTokenValue,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task RevokeRefreshTokenAsync(RevokeRefreshTokenDto dto)
        {
            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);

            if (existingRefreshToken == null)
                throw new NotFoundException("Refresh token tapılmadı");

            if (existingRefreshToken.IsRevoked)
                throw new BadRequestException("Refresh token artıq ləğv olunub");

            existingRefreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(existingRefreshToken);
        }
    }
}