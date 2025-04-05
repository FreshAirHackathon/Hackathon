using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System;
using System.IO;

namespace Hackathon.AuthService.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
            }
            else
            {
                throw new Exception($".env file not found at: {envPath}");
            }

            var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("POSTGRES_CONNECTION_STRING is missing in environment variables.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
