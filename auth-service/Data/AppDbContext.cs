using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.AuthService.Data
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			List<IdentityRole> roles = new List<IdentityRole>
		{
			new IdentityRole
			{
				Name = "Admin",
				NormalizedName = "ADMIN"
			},
			new IdentityRole
			{
				Name = "User",
				NormalizedName = "USER"
			}
		};
			modelBuilder.Entity<IdentityRole>().HasData(roles);
		}
	}
}
