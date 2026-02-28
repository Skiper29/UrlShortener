using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.DAL.Entities;

namespace UrlShortener.DAL.Data.Configurations;

public class AboutContentConfig : IEntityTypeConfiguration<AboutContent>
{
    public void Configure(EntityTypeBuilder<AboutContent> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(5000);
    }
}
