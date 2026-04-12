using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class LactationPeriodEntityTypeConfiguration : IEntityTypeConfiguration<LactationPeriod>
{
    public void Configure(EntityTypeBuilder<LactationPeriod> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}