using RentHouse.Models;

namespace RentHouse.TokenSevice.ITokenSevice
{
    public interface ITokenService
    {
        Task<string> BuildToken(string key, string issuer, ApplicationUser user);
        bool ValidateToken(string key, string issuer,  string token);
    }
}
