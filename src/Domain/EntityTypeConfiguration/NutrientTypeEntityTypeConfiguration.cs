using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfiguration;

public class NutrientTypeEntityTypeConfiguration : IEntityTypeConfiguration<NutrientType>
{
    public void Configure(EntityTypeBuilder<NutrientType> builder)
    {
        builder.Property(x => x.Code).HasMaxLength(20);
    }
}