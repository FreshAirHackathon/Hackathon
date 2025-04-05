using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prevencio.Domain.DTOs.Account;
using Prevencio.Domain.Entities;
using Prevencio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.AuthService.Services
{
	public class userInfoService(UserManager<AppUser> userManager) : IUserInfoService
	{
		public async Task<UserInfoDto> GetUserInfo(AppUser appUser)
		{
			var userId = appUser.Id;

			var user = await userManager.Users
			.FirstOrDefaultAsync(u => u.Id == userId);

			var roles = (await userManager.GetRolesAsync(user!)).ToList();

			if (user == null)
				throw new Exception("User not found");

			return new UserInfoDto
			{
				Id = user.Id,
				Email = user.Email!,
				Username = user.UserName!,
				Role = roles,
				EmailConfirmed = user.EmailConfirmed
			};
		}
		public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
		{
			var users = await userManager.Users
				.ToListAsync();

			var userDtos = new List<UserInfoDto>();

			foreach (var user in users)
			{
				var roles = (await userManager.GetRolesAsync(user)).ToList();
				userDtos.Add(new UserInfoDto
				{
					Id = user.Id,
					Email = user.Email!,
					Username = user.UserName!,
					Role = roles,
					EmailConfirmed = user.EmailConfirmed
				});
			}

			return userDtos;
		}
	}
}