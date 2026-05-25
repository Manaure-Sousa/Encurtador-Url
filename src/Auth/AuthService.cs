using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EncurtadorUrl.src.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EncurtadorUrl.src.Auth
{
    public class AuthService (IConfiguration configuration)
    {
        public async Task<IResult> RegisterUserAsync(UserRequest user, ApplicationDbContext dbContext)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            dbContext.Users.Add(new Models.User { Email = user.Email, PasswordHash = user.Password });
            await dbContext.SaveChangesAsync();
            return Results.Ok("Usuário registrado com sucesso.");
        }

        public async Task<IResult> LoginUserAsync(UserRequest request, ApplicationDbContext dbContext)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest("Email ou senha inválidos.");
            }

            // buscar meio de usar o build e usar o appsettings
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured in service."));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer not configured in service."),
                Audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience not configured in service."),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var JwtToken = tokenHandler.WriteToken(token);
            string tokenString = tokenHandler.WriteToken(token);

            return Results.Ok(tokenString);
        }
    }
}