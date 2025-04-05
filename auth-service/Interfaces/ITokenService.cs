using System.Collections.Generic;
using Hackathon.AuthService.Entities;

namespace Hackathon.AuthService.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user, List<string> roles);
        string CreateRefreshToken();
    }
}