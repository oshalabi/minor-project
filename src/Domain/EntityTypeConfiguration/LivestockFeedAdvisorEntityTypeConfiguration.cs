using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class LivestockFeedAdvisorEntityTypeConfiguration : IEntityTypeConfiguration<LivestockFeedAdvisor>
{
    public void Configure(EntityTypeBuilder<LivestockFeedAdvisor> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
    }
}