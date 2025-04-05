
using System.Collections.Generic;

namespace Hackathon.AuthService.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user, List<string> roles);
        string CreateRefreshToken();
    }
}