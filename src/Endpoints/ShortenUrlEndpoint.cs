using EncurtadorUrl.src.Data;
using EncurtadorUrl.src.Models;
using EncurtadorUrl.src.Services;

namespace EncurtadorUrl.src.Endpoints
{
    public static class ShortenUrlEndpoint
    {
        public static void MapShortenUrlEndpoint(this WebApplication app)
        {
            app.MapPost("shorten", async (
                ShortenUrlRequest request,
                UrlShorteningService urlShorteningService,
                ApplicationDbContext dbContext,
                HttpContext httpContext
            ) =>
            {
                if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
                {
                    return Results.BadRequest("URL inválida.");
                }

                var code = await urlShorteningService.GenerateUniqueCode();

                var httpRequest = httpContext.Request;

                var shortenedUrl = new ShortenedUrl
                {
                    Id = Guid.NewGuid(),
                    LongUrl = request.Url,
                    Code = code,
                    ShortUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{code}",
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.ShortenedUrls.Add(shortenedUrl);
                await dbContext.SaveChangesAsync();

                return Results.Ok(shortenedUrl.ShortUrl);
            });
        }
    }
}