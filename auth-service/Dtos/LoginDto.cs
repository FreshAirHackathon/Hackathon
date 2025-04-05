using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.AuthService.Dtos
{
    public class LoginDto
    {
        [Required]
        public required string Username { get; init; }
        [Required]
        public required string Password { get; init; }
    }
}

