using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class ParityEntityTypeConfiguration : IEntityTypeConfiguration<Parity>
{
    public void Configure(EntityTypeBuilder<Parity> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(200);
    }
}