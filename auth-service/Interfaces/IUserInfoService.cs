using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.AuthService.Interfaces
{
	public interface IUserInfoService
	{
		Task<UserInfoDto> GetUserInfo(AppUser user);
		Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
	}
}