using EncurtadorUrl.src.Models;
using EncurtadorUrl.src.Settings;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorUrl.src.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder
                .Property(shortenedUrl => shortenedUrl.Code)
                .HasMaxLength(ShortLinkSettings.Length);

            builder
                .HasIndex(shortenedUrl => shortenedUrl.Code)
                .IsUnique();
        });
    }

    }
}