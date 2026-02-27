using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.DAL.Entities;

namespace UrlShortener.DAL.Data.Configurations;

public class ShortenedUrlConfig : IEntityTypeConfiguration<ShortenedUrl>
{
    private const int MaxOriginalUrlLength = 2048;
    private const int MaxShortCodeLength = 10;

    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.OriginalUrl)
            .IsRequired()
            .HasMaxLength(MaxOriginalUrlLength);

        builder.Property(s => s.ShortCode)
            .IsRequired()
            .HasMaxLength(MaxShortCodeLength);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.HasOne(s => s.CreatedBy)
            .WithMany(u => u.ShortenedUrls)
            .HasForeignKey(s => s.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.ShortCode).IsUnique();
        builder.HasIndex(e => e.OriginalUrl).IsUnique();
    }
}
