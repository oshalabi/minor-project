using AutoMapper;
using Domain.Entities;
using Domain.EntityTypeConfiguration;
using EnergyFood.Commands;
using EnergyFood.Controllers;
using EnergyFood.DAL;
using EnergyFood.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using GetAvailableFeedTypes = EnergyFood.Model.GetAvailableFeedTypes;

namespace EnergyFood.Tests;

public class EnergyFoodControllerTests
{
    private readonly EnergyFoodDB _dbContext;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EnergyFoodController _controller;

    public EnergyFoodControllerTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<EnergyFoodDB>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new EnergyFoodDB(options);

        // Seed data
        SeedDatabase();

        // Mock IMapper
        _mapperMock = new Mock<IMapper>();

        // Initialize controller
        _controller = new EnergyFoodController(_dbContext, _mapperMock.Object);
    }

    private void SeedDatabase()
    {
        _dbContext.Categories.AddRange(
            new Category { Id = 1, Name = "Dry Feed", Value = CategoryTypeValue.ENKELVOUDIG_DROOG },
            new Category { Id = 2, Name = "Wet Feed", Value = CategoryTypeValue.ENKELVOUDIG_VOCHTIG }
        );

        _dbContext.FeedTypes.AddRange(
            new FeedType { Id = 1, Name = "Feed A", DsProcent = 50.0, CategoryId = 1 },
            new FeedType { Id = 2, Name = "Feed B", DsProcent = 40.0, CategoryId = 2 }
        );

        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllEnergyFoods_ReturnsAllFeedTypes()
    {
        // Act
        var result = await _controller.GetAllEnergyFoods();

        // Assert
        var okResult = Assert.IsType<Ok<List<FeedType>>>(result);
        var feedTypes = okResult.Value;
        Assert.NotNull(feedTypes);
        Assert.True(feedTypes.Count > 0, "Feed types should not be empty.");
    }

    [Fact]
    public async Task RemoveEnergyFood_RemovesFeedType()
    {
        // Arrange
        var command = new RemoveEnergyFood
        {
            Id = 1,
            EnergyFoodId = 0
        };

        // Act
        var result = await _controller.RemoveEnergyFood(command);

        // Assert
        Assert.IsType<Ok>(result);
        Assert.Null(await _dbContext.FeedTypes.FindAsync(1));
    }

    [Fact]
    public async Task GetAvailableFeedTypes_ReturnsFilteredFeedTypes()
    {
        // Arrange
        var command = new GetAvailableFeedTypes
        {
            CategoryValues = new[] { CategoryTypeValue.ENKELVOUDIG_DROOG },
            FeedTypeKeys = new[] { "feed_a" }
        };

        // Act
        var result = await _controller.GetAvailableFeedTypes(command);

        // Assert
        var okResult = Assert.IsType<Ok<List<FeedTypeDTO>>>(result);

        // Assert the returned value
        var feedTypes = okResult.Value;
        Assert.NotNull(feedTypes);
        Assert.True(feedTypes.Count > 0, "Feed types should not be empty.");
    }

    [Fact]
    public async Task GetAllNutrients_ReturnsAllNutrientTypes()
    {
        // Arrange
        _dbContext.NutrientTypes.AddRange(
            new NutrientType { Id = 1, Code = "Protein", Value = NutrientTypeValue.Vem },
            new NutrientType { Id = 2, Code = "Fat", Value = NutrientTypeValue.Bw }
        );
        _dbContext.SaveChanges();

        // Act
        var result = await _controller.GetAllNutrients();

        // Assert
        var okResult = Assert.IsType<Ok<List<NutrientType>>>(result);

        var feedTypes = okResult.Value;
        Assert.NotNull(feedTypes);
        Assert.True(feedTypes.Count > 0, "Feed types should not be empty.");
    }
}