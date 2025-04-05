
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}
else
{
    throw new Exception($".env file not found at: {envPath}");
}

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

//  PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//  Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT 
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddControllers();

// Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
