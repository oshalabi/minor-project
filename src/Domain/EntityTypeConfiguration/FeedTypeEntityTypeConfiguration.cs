using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class FeedTypeEntityTypeConfiguration : IEntityTypeConfiguration<FeedType>
{
    public void Configure(EntityTypeBuilder<FeedType> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Code).HasMaxLength(20);
    }
}