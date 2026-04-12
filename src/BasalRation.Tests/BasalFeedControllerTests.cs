using BasalRation.Commands;

namespace BasalRation.Tests;

using Controllers;
using DAL;
using Model;
using Domain.Entities;
using Domain.EntityTypeConfiguration;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class BasalFeedControllerTests
{
    private readonly BasalFeedDB _dbContext;
    private readonly BasalFeedController _controller;

    public BasalFeedControllerTests()
    {
        var options = new DbContextOptionsBuilder<BasalFeedDB>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new BasalFeedDB(options);


        SeedDatabase();

        _controller = new BasalFeedController(_dbContext, null!);
    }

    private void SeedDatabase()
    {
        if (_dbContext.Categories.Any() || _dbContext.NutrientTypes.Any() || _dbContext.FeedTypes.Any())
        {
            return;
        }


        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "ENKELVOUDIG DROOG", Value = CategoryTypeValue.ENKELVOUDIG_DROOG },
            new Category { Id = 2, Name = "ENKELVOUDIG VOCHTIG", Value = CategoryTypeValue.ENKELVOUDIG_VOCHTIG },
            new Category { Id = 3, Name = "MINERALEN", Value = CategoryTypeValue.MINERALEN },
            new Category { Id = 4, Name = "STANDAARD MENGVOEDERS", Value = CategoryTypeValue.STANDAARD_MENGVOEDERS },
            new Category { Id = 5, Name = "STANDAARD RUWVOEDERS", Value = CategoryTypeValue.STANDAARD_RUWVOEDERS }
        };

        _dbContext.Categories.AddRange(categories);


        var nutrientTypes = new List<NutrientType>
        {
            new NutrientType { Id = 1, Code = "Oep", Value = NutrientTypeValue.Oep },
            new NutrientType { Id = 2, Code = "Vem", Value = NutrientTypeValue.Vem },
            new NutrientType { Id = 3, Code = "Bzb", Value = NutrientTypeValue.Bzb },
            new NutrientType { Id = 4, Code = "Dvp", Value = NutrientTypeValue.Dvp },
            new NutrientType { Id = 5, Code = "PercentRv", Value = NutrientTypeValue.PercentRv }
        };

        _dbContext.NutrientTypes.AddRange(nutrientTypes);


        var feedTypes = new List<FeedType>
        {
            new FeedType
            {
                Id = 1,
                Code = "F001",
                Name = "Corn",
                DsProcent = 50,
                CategoryId = categories.First(c => c.Value == CategoryTypeValue.ENKELVOUDIG_DROOG).Id,
                Category = categories.First(c => c.Value == CategoryTypeValue.ENKELVOUDIG_DROOG),
                Nutrients = new List<Nutrient>
                {
                    new Nutrient
                    {
                        Id = 1,
                        Value = 10,
                        NutrientTypeId = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Oep).Id,
                        NutrientType = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Oep)
                    },
                    new Nutrient
                    {
                        Id = 2,
                        Value = 5,
                        NutrientTypeId = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Vem).Id,
                        NutrientType = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Vem)
                    }
                }
            },
            new FeedType
            {
                Id = 2,
                Code = "F002",
                Name = "Wheat",
                DsProcent = 60,
                CategoryId = categories.First(c => c.Value == CategoryTypeValue.ENKELVOUDIG_VOCHTIG).Id,
                Category = categories.First(c => c.Value == CategoryTypeValue.ENKELVOUDIG_VOCHTIG),
                Nutrients = new List<Nutrient>
                {
                    new Nutrient
                    {
                        Id = 3,
                        Value = 12,
                        NutrientTypeId = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Bzb).Id,
                        NutrientType = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Bzb)
                    }
                }
            },
            new FeedType
            {
                Id = 3,
                Code = "F003",
                Name = "Barley",
                DsProcent = 45,
                CategoryId = categories.First(c => c.Value == CategoryTypeValue.MINERALEN).Id,
                Category = categories.First(c => c.Value == CategoryTypeValue.MINERALEN),
                Nutrients = new List<Nutrient>
                {
                    new Nutrient
                    {
                        Id = 4,
                        Value = 8,
                        NutrientTypeId = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Dvp).Id,
                        NutrientType = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.Dvp)
                    },
                    new Nutrient
                    {
                        Id = 5,
                        Value = 3,
                        NutrientTypeId = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.PercentRv).Id,
                        NutrientType = nutrientTypes.First(nt => nt.Value == NutrientTypeValue.PercentRv)
                    }
                }
            }
        };

        _dbContext.FeedTypes.AddRange(feedTypes);


        _dbContext.SaveChanges();
    }


    [Fact]
    public async Task GetAllFeedTypes_ShouldReturnOkResult_WithFeedTypeDTOs()
    {
        // Act
        var result = await _controller.GetAllFeedTypes();

        // Assert
        result.Should().BeOfType<Ok<List<FeedTypeDTO>>>();

        var okResult = result as Ok<List<FeedTypeDTO>>;
        okResult?.Value.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetAvailableFeedTypes_ShouldReturnOkResult_WithAvailableFeedTypes()
    {
        // Arrange
        var command = new GetAvailableFeedTypes
        {
            CategoryValues = new[] { CategoryTypeValue.ENKELVOUDIG_DROOG },
            FeedTypeKeys = new List<string> { "f002" } 
        };

        // Act
        var result = await _controller.GetAvailableFeedTypes(command);

        // Assert
        result.Should().BeOfType<Ok<List<FeedTypeDTO>>>(); 

        var okResult = result as Ok<List<FeedTypeDTO>>;
        okResult?.Value.Should().NotBeNullOrEmpty();
        okResult?.Value.Should().HaveCount(1); 
        okResult?.Value.First().Name.Should().Be("Corn");
    }


    [Fact]
    public async Task GetAvailableFeedTypes_ShouldReturnNotFound_WhenNoMatchingCategories()
    {
        // Arrange
        var command = new GetAvailableFeedTypes
        {
            CategoryValues = [CategoryTypeValue.NONE],
            FeedTypeKeys = new List<string>()
        };
    
        // Act
        var result = await _controller.GetAvailableFeedTypes(command);
    
        // Assert
        result.Should().BeOfType<NotFound>();
    }
    
    [Fact]
    public async Task GetAllFeedTypes_ShouldReturnOkResult_WithEmptyList_WhenNoFeedTypesExist()
    {
        // Arrange
        _dbContext.FeedTypes.RemoveRange(_dbContext.FeedTypes); 
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _controller.GetAllFeedTypes();

        // Assert
        result.Should().BeOfType<Ok<List<FeedTypeDTO>>>(); 
        var okResult = result as Ok<List<FeedTypeDTO>>;
        okResult?.Value.Should().BeEmpty(); 
    }
    
    [Fact]
    public async Task GetAvailableFeedTypes_ShouldReturnOkResult_WithEmptyList_WhenNoFeedTypesMatchCriteria()
    {
        // Arrange
        var command = new GetAvailableFeedTypes
        {
            CategoryValues = [CategoryTypeValue.STANDAARD_RUWVOEDERS], 
            FeedTypeKeys = new List<string> { "f003" } 
        };

        // Act
        var result = await _controller.GetAvailableFeedTypes(command);

        // Assert
        result.Should().BeOfType<Ok<List<FeedTypeDTO>>>(); 
        var okResult = result as Ok<List<FeedTypeDTO>>;
        okResult?.Value.Should().BeEmpty(); 
    }
    [Fact]
    public async Task GetAllNutrients_ShouldReturnOkResult_WithAllNutrientTypes()
    {
        // Act
        var result = await _controller.GetAllNutrients();

        // Assert
        result.Should().BeOfType<Ok<List<NutrientType>>>(); 
        var okResult = result as Ok<List<NutrientType>>;
        okResult?.Value.Should().HaveCount(5); 
    }
    [Fact]
    public async Task GetAllNutrients_ShouldReturnOkResult_WithEmptyList_WhenNoNutrientTypesExist()
    {
        // Arrange
        _dbContext.NutrientTypes.RemoveRange(_dbContext.NutrientTypes); 
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _controller.GetAllNutrients();

        // Assert
        result.Should().BeOfType<Ok<List<NutrientType>>>(); 
        var okResult = result as Ok<List<NutrientType>>;
        okResult?.Value.Should().BeEmpty(); 
    }

}