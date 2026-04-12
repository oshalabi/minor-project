using Domain.EntityTypeConfiguration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasalRation.DAL;

public class NormDB : DbContext
{
    public NormDB(DbContextOptions<NormDB> options)
        : base(options) { }

    public DbSet<Norm> Norms { get; set; }
    public DbSet<NutrientType> NutrientTypes { get; set; }
    public DbSet<LactationPeriod> LactationPeriods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Use PostgreSQL-specific naming conventions
        modelBuilder.HasDefaultSchema("public");
        
        modelBuilder.Entity<LivestockProperties>()
            .HasOne(lp => lp.Ration)
            .WithOne(r => r.LivestockProperties)
            .HasForeignKey<Ration>(r => r.LivestockPropertyId) // Explicit Foreign Key
            .OnDelete(DeleteBehavior.Cascade);
        
        // Existing model configuration and seeding data
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<LactationPeriod>()
            .HasData(
                new LactationPeriod
                {
                    Id = 1,
                    Name = "ca. 0-40 dgn",
                    StartDay = 0,
                    EndDay = 40,
                    Value = LactationPeriodValue.CA0TO40,
                },
                new LactationPeriod
                {
                    Id = 2,
                    Name = "ca. 41-80 dgn",
                    StartDay = 41,
                    EndDay = 80,
                    Value = LactationPeriodValue.CA41TO80,
                },
                new LactationPeriod
                {
                    Id = 3,
                    Name = "ca. 81-120 dgn",
                    StartDay = 81,
                    EndDay = 120,
                    Value = LactationPeriodValue.CA81TO120,
                },
                new LactationPeriod
                {
                    Id = 4,
                    Name = "ca. 121-200 dgn",
                    StartDay = 121,
                    EndDay = 200,
                    Value = LactationPeriodValue.CA121TO200,
                },
                new LactationPeriod
                {
                    Id = 5,
                    Name = "ca. 201-280 dgn",
                    StartDay = 201,
                    EndDay = 280,
                    Value = LactationPeriodValue.CA201TO280,
                },
                new LactationPeriod
                {
                    Id = 6,
                    Name = "from 281 dgn",
                    StartDay = 281,
                    EndDay = int.MaxValue, // Use a large number to signify no upper limit
                    Value = LactationPeriodValue.FROM281,
                }
            );

        
        modelBuilder.Entity<NutrientType>().HasData(
            Enum.GetValues(typeof(NutrientTypeValue))
                .Cast<NutrientTypeValue>()
                .Where(value => value != NutrientTypeValue.None) // Exclude "None"
                .Select((value, index) => new NutrientType
                {
                    Id = index + 1, // Assign unique Ids starting from 1
                    Code = value.ToString(), // Use the enum member's name as the Code
                    Value = value // Assign the enum value
                })
                .ToArray()
        );
    }
}