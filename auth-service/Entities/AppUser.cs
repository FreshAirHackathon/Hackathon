using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Hackathon.AuthService.Entities
{
    public class AppUser : IdentityUser
    {
        [MaxLength(50)] public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpiryDate { get; set; }
    }
}