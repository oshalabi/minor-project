using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class RationEntityTypeConfiguration : IEntityTypeConfiguration<Ration>
{
    public void Configure(EntityTypeBuilder<Ration> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100);
    }
}