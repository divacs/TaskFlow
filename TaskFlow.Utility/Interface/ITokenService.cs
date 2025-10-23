using TaskFlow.Models.Models;

namespace TaskFlow.Utility.Service
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}
