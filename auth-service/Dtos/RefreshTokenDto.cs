using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.AuthService.Dtos
{
    public class RefreshTokenDto
    {
        public required string RefreshToken { get; init; }
    }
}
