using Domain.EntityTypeConfiguration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace BasalRation.DAL;

public class BasalFeedDB : DbContext
{
    public BasalFeedDB(DbContextOptions<BasalFeedDB> options)
        : base(options)
    {
    }

    public DbSet<FeedType> FeedTypes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Nutrient> Nutrients { get; set; }
    public DbSet<NutrientType> NutrientTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryEntityTypeConfiguration).Assembly);

        
        // Use PostgreSQL-specific naming conventions
        modelBuilder.HasDefaultSchema("public");
        
        modelBuilder.Entity<LivestockProperties>()
            .HasOne(lp => lp.Ration)
            .WithOne(r => r.LivestockProperties)
            .HasForeignKey<Ration>(r => r.LivestockPropertyId) // Explicit Foreign Key
            .OnDelete(DeleteBehavior.Cascade);

        // Existing model configuration and seeding data
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Category>()
            .HasData(
                new Category
                {
                    Id = 1,
                    Name = "Enkelvoudig droog",
                    Value = CategoryTypeValue.ENKELVOUDIG_DROOG
                },
                new Category
                {
                    Id = 2,
                    Name = "Enkelvoudig vochtig",
                    Value = CategoryTypeValue.ENKELVOUDIG_VOCHTIG
                },
                new Category
                {
                    Id = 3,
                    Name = "Mineralen",
                    Value = CategoryTypeValue.MINERALEN
                },
                new Category
                {
                    Id = 4,
                    Name = "Standaard mengvoeders",
                    Value = CategoryTypeValue.STANDAARD_MENGVOEDERS
                },
                new Category
                {
                    Id = 5,
                    Name = "Standaard ruwvoeders",
                    Value = CategoryTypeValue.STANDAARD_RUWVOEDERS
                }
            );

        modelBuilder
            .Entity<FeedType>()
            .HasData(
                new FeedType
                {
                    Id = 1,
                    Name = "Mais",
                    CategoryId = 1,
                    Nutrients = new List<Nutrient>(),
                    DsProcent = 47.5,
                },
                new FeedType
                {
                    Id = 2,
                    Name = "Gras",
                    CategoryId = 1,
                    DsProcent = 60.5,
                },
                new FeedType
                {
                    Id = 3,
                    Name = "Graan",
                    CategoryId = 1,
                    DsProcent = 90.5,
                },
                new FeedType
                {
                    Id = 4,
                    Name = "Pellets",
                    CategoryId = 1,
                    DsProcent = 45.5,
                },
                new FeedType
                {
                    Id = 5,
                    Name = "Mais+",
                    CategoryId = 1,
                    DsProcent = 60.5,
                },
                new FeedType
                {
                    Id = 6,
                    Name = "Pellets+",
                    CategoryId = 1,
                    DsProcent = 90.5,
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

        modelBuilder
            .Entity<Nutrient>()
            .HasData(
                // Nutrients for Mais
                new Nutrient { Id = 1, NutrientTypeId = (int)NutrientTypeValue.VemDvp, Value = 80 },
                new Nutrient { Id = 2, NutrientTypeId = (int)NutrientTypeValue.Bw, Value = 92 },

                // Nutrients for Gras
                new Nutrient { Id = 3, NutrientTypeId = (int)NutrientTypeValue.Bzet, Value = 70 },
                new Nutrient { Id = 4, NutrientTypeId = (int)NutrientTypeValue.PercentRv, Value = 85 },

                // Nutrients for Graan
                new Nutrient { Id = 5, NutrientTypeId = (int)NutrientTypeValue.PercentRv, Value = 75 },
                new Nutrient { Id = 6, NutrientTypeId = (int)NutrientTypeValue.Bzet, Value = 88 },

                // Nutrients for Pellets
                new Nutrient { Id = 7, NutrientTypeId = (int)NutrientTypeValue.Bzet, Value = 65 },
                new Nutrient { Id = 8, NutrientTypeId = (int)NutrientTypeValue.PercentRv, Value = 80 },

                // Nutrients for Mais+
                new Nutrient { Id = 9, NutrientTypeId = (int)NutrientTypeValue.Zw, Value = 85 },
                new Nutrient { Id = 10, NutrientTypeId = (int)NutrientTypeValue.Bw, Value = 95 },

                // Nutrients for Pellets+
                new Nutrient { Id = 11, NutrientTypeId = (int)NutrientTypeValue.Zw, Value = 90 },
                new Nutrient { Id = 12, NutrientTypeId = (int)NutrientTypeValue.Bw, Value = 100 },

                //DS Nutrients for Mais
                new Nutrient { Id = 13, NutrientTypeId = (int)NutrientTypeValue.Vem, Value = 120 },
                new Nutrient { Id = 14, NutrientTypeId = (int)NutrientTypeValue.Oep, Value = 90 },
                new Nutrient { Id = 15, NutrientTypeId = (int)NutrientTypeValue.Bzb, Value = 75 },
                new Nutrient { Id = 16, NutrientTypeId = (int)NutrientTypeValue.Dvp, Value = 110 },
                
                //DS Nutrients for Gras
                new Nutrient { Id = 17, NutrientTypeId = (int)NutrientTypeValue.Vem, Value = 95 },
                new Nutrient { Id = 18, NutrientTypeId = (int)NutrientTypeValue.Oep, Value = 85 },
                new Nutrient { Id = 19, NutrientTypeId = (int)NutrientTypeValue.Bzb, Value = 65 },
                new Nutrient { Id = 20, NutrientTypeId = (int)NutrientTypeValue.Dvp, Value = 105 },
                
                //DS Nutrients for Graan
                new Nutrient { Id = 21, NutrientTypeId = (int)NutrientTypeValue.Vem, Value = 110 },
                new Nutrient { Id = 22, NutrientTypeId = (int)NutrientTypeValue.Oep, Value = 70 },
                new Nutrient { Id = 23, NutrientTypeId = (int)NutrientTypeValue.Bzb, Value = 80 },
                new Nutrient { Id = 24, NutrientTypeId = (int)NutrientTypeValue.Dvp, Value = 120 },
                
                //DS Nutrients for Pellets
                new Nutrient { Id = 25, NutrientTypeId = (int)NutrientTypeValue.Vem, Value = 130 },
                new Nutrient { Id = 26, NutrientTypeId = (int)NutrientTypeValue.Oep, Value = 75 },
                new Nutrient { Id = 27, NutrientTypeId = (int)NutrientTypeValue.Bzb, Value = 85 },
                new Nutrient { Id = 28, NutrientTypeId = (int)NutrientTypeValue.Dvp, Value = 115 },
                
                //DS Nutrients for Mais+
                new Nutrient { Id = 29, NutrientTypeId = (int)NutrientTypeValue.Vem, Value = 90 },
                new Nutrient { Id = 30, NutrientTypeId = (int)NutrientTypeValue.Oep, Value = 95 },
                new Nutrient { Id = 31, NutrientTypeId = (int)NutrientTypeValue.Bzb, Value = 70 },
                new Nutrient { Id = 32, NutrientTypeId = (int)NutrientTypeValue.Dvp, Value = 125 },

                //DS Nutrients for Pellets+
                new Nutrient { Id = 33, NutrientTypeId = (int)NutrientTypeValue.Vem, Value = 90 },
                new Nutrient { Id = 34, NutrientTypeId = (int)NutrientTypeValue.Oep, Value = 95 },
                new Nutrient { Id = 35, NutrientTypeId = (int)NutrientTypeValue.Bzb, Value = 70 },
                new Nutrient { Id = 36, NutrientTypeId = (int)NutrientTypeValue.Dvp, Value = 125 }
            );

        modelBuilder.Entity("FeedTypeNutrient").HasData(
            // Nutrients for Mais
            new { FeedTypesId = 1, NutrientsId = 1 },
            new { FeedTypesId = 1, NutrientsId = 2 },
            new { FeedTypesId = 1, NutrientsId = 13 },
            new { FeedTypesId = 1, NutrientsId = 14 },
            new { FeedTypesId = 1, NutrientsId = 15 },
            new { FeedTypesId = 1, NutrientsId = 16 },

            // Nutrients for Gras
            new { FeedTypesId = 2, NutrientsId = 3 },
            new { FeedTypesId = 2, NutrientsId = 4 },
            new { FeedTypesId = 2, NutrientsId = 17 },
            new { FeedTypesId = 2, NutrientsId = 18 },
            new { FeedTypesId = 2, NutrientsId = 19 },
            new { FeedTypesId = 2, NutrientsId = 20 },

            // Nutrients for Graan
            new { FeedTypesId = 3, NutrientsId = 5 },
            new { FeedTypesId = 3, NutrientsId = 6 },
            new { FeedTypesId = 3, NutrientsId = 21 },
            new { FeedTypesId = 3, NutrientsId = 22 },
            new { FeedTypesId = 3, NutrientsId = 23 },
            new { FeedTypesId = 3, NutrientsId = 24 },

            // Nutrients for Pellets
            new { FeedTypesId = 4, NutrientsId = 7 },
            new { FeedTypesId = 4, NutrientsId = 8 },
            new { FeedTypesId = 4, NutrientsId = 25 },
            new { FeedTypesId = 4, NutrientsId = 26 },
            new { FeedTypesId = 4, NutrientsId = 27 },
            new { FeedTypesId = 4, NutrientsId = 28 },

            // Nutrients for Mais+
            new { FeedTypesId = 5, NutrientsId = 9 },
            new { FeedTypesId = 5, NutrientsId = 10 },
            new { FeedTypesId = 5, NutrientsId = 29 },
            new { FeedTypesId = 5, NutrientsId = 30 },
            new { FeedTypesId = 5, NutrientsId = 31 },
            new { FeedTypesId = 5, NutrientsId = 32 },

            // Nutrients for Pellets+
            new { FeedTypesId = 6, NutrientsId = 11 },
            new { FeedTypesId = 6, NutrientsId = 12 },
            new { FeedTypesId = 6, NutrientsId = 33 },
            new { FeedTypesId = 6, NutrientsId = 34 },
            new { FeedTypesId = 6, NutrientsId = 35 },
            new { FeedTypesId = 6, NutrientsId = 36 }
        );
    }

    public void MigrateDb()
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(10, r => TimeSpan.FromSeconds(10)) // Retry 10 times with a 10-second delay
            .Execute(() => Database.Migrate());
    }
}