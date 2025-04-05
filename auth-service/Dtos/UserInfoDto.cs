using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.AuthService.Dtos
{
    public class UserInfoDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string Username { get; init; }
        [Required]
        public required string Id { get; init; }
        [Required]
        public required List<string> Role { get; init; } = new List<string>();
        [Required]
        public required bool EmailConfirmed { get; init; }
    }
}
