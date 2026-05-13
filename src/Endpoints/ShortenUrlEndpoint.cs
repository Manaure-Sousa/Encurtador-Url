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
            });

            app.MapGet("{code}", async(string code, ApplicationDbContext dbContext) =>
            {
                var shortenedUrl = await dbContext.ShortenedUrls.SingleOrDefaultAsync(s => s.Code == code);

                if (shortenedUrl == null)
                {
                    return Results.NotFound();
                }

                return Results.Redirect(shortenedUrl.LongUrl);
            });
        }
    }
}