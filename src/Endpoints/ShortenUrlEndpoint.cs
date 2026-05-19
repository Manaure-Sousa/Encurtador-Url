using EncurtadorUrl.src.Data;
using EncurtadorUrl.src.Models;
using EncurtadorUrl.src.Services;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorUrl.src.Endpoints
{
    public static class ShortenUrlEndpoint
    {
        public static void MapShortenUrlEndpoint(this WebApplication app)
        {
            app.MapPost("shorten", async (
                ShortenUrlRequest request,
                HttpContext httpContext,
                UrlShorteningService urlShorteningService
            ) =>
            {
                return await urlShorteningService.ShortenUrl(request, httpContext);
            }).RequireAuthorization().RequireRateLimiting("FixedWindowPolicy");

            app.MapGet("{code}", async (string code, ApplicationDbContext dbContext,
            UrlShorteningService urlShorteningService) =>
            {
                return await urlShorteningService.GetShortenedUrlByCode(code);
            }).RequireRateLimiting("FixedWindowPolicy");
        }
    }
}