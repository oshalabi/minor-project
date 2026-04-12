using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class NormEntityTypeConfiguration : IEntityTypeConfiguration<Norm>
{
    public void Configure(EntityTypeBuilder<Norm> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Remark).HasMaxLength(200);
    }
}