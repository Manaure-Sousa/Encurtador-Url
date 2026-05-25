using EncurtadorUrl.src.Data;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorUrl.src.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var groupAuth = app.MapGroup("auth/");
            
            groupAuth.MapPost("register", async (UserRequest request, ApplicationDbContext dbContext, AuthService authService) =>
            {
                if(await dbContext.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return Results.BadRequest("Email já registrado.");
                }

                return await authService.RegisterUserAsync(request, dbContext);

            });

            groupAuth.MapPost("login", async (UserRequest request, ApplicationDbContext dbContext, AuthService authService) =>
            {
                return await authService.LoginUserAsync(request, dbContext);
            });
        }
    }
}