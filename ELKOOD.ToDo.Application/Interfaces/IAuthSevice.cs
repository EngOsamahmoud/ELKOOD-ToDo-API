using ELKOOD.ToDo.Application.DTOs.Auth;

namespace ELKOOD.ToDo.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, string requestedByRole);
        Task<bool> UserExistsAsync(string email, string username);
    }
}