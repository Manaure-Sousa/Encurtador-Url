using EncurtadorUrl.src.Data;
using EncurtadorUrl.src.Models;
using EncurtadorUrl.src.Settings;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorUrl.src.Services
{
    public class UrlShorteningService(ApplicationDbContext dbContext)
    {
        private readonly Random _random = new Random();

        public async Task<string> GenerateUniqueCode()
        {
            var codeChars = new char[ShortLinkSettings.Length];
            int maxValue = ShortLinkSettings.Alphabet.Length;

            while (true)
            {

                for (int i = 0; i < ShortLinkSettings.Length; i++)
                {
                    codeChars[i] = ShortLinkSettings.Alphabet[_random.Next(maxValue)];
                }

                var code = new string(codeChars);

                if (!await dbContext.ShortenedUrls.AnyAsync(sl => sl.Code == code))
                {
                    return code;
                }
            }

        }

        public async Task<IResult> ShortenUrl(
                ShortenUrlRequest request,
                HttpContext httpContext)
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return Results.BadRequest("URL inválida.");
            }

            var code = await GenerateUniqueCode();

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
        }
    }
}