namespace ELKOOD.ToDo.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Core.Entities.User user);
        System.Security.Claims.ClaimsPrincipal? ValidateToken(string token);
    }
}