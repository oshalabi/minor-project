using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class CowEntityTypeConfiguration : IEntityTypeConfiguration<Cow>
{
    public void Configure(EntityTypeBuilder<Cow> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}