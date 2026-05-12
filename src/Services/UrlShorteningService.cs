using EncurtadorUrl.src.Data;
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
    }
}