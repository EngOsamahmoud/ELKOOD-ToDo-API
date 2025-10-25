using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Enums;
using ELKOOD.ToDo.Core.Interfaces;
using ELKOOD.ToDo.Application.Interfaces;
using ELKOOD.ToDo.Application.DTOs.Auth;
namespace ELKOOD.ToDo.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, string requestedByRole)
        {
            // Check if user already exists
            if (await _userRepository.UserExistsAsync(registerDto.Email, registerDto.Username))
            {
                throw new InvalidOperationException("User with this email or username already exists");
            }

            // Determine user role - only Owner can create other Owners
            UserRole role;
            if (requestedByRole == "Owner" && registerDto.Role == "Owner")
            {
                role = UserRole.Owner;
            }
            else
            {
                role = UserRole.Guest;
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordService.HashPassword(registerDto.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            return await _userRepository.UserExistsAsync(email, username);
        }
    }
}