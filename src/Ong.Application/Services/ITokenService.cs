using Ong.Domain;

namespace Ong.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
