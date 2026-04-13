using AutoMapper;
using Domain.Entities;
using Domain.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Moq;
using Ration.Commands;
using Ration.DAL;
using Ration.Exceptions;
using Ration.Model;
using Ration.Service;

namespace Ration.Tests;

public class RationServiceTests
{
    private readonly RationDB _mockDbContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RationService _rationService;

    public RationServiceTests()
    {
        // Configureer een in-memory database
        var options = new DbContextOptionsBuilder<RationDB>()
            .UseInMemoryDatabase($"RationServiceTestsDB_{Guid.NewGuid()}")
            .Options;

        // Initialiseer de DbContext
        _mockDbContext = new RationDB(options);

        // Configureer een mock van IMapper
        _mockMapper = new Mock<IMapper>();

        // Initialiseer de service met de DbContext en de IMapper mock
        _rationService = new RationService(_mockDbContext, _mockMapper.Object);

        // Seed testdata in de in-memory database
        SeedTestData(_mockDbContext);
    }

    private void SeedTestData(RationDB dbContext)
    {
        dbContext.LactationPeriods.AddRange(
            new LactationPeriod
            {
                Id = 1,
                Name = "ca. 0-40 dgn",
                StartDay = 0,
                EndDay = 40,
                Value = LactationPeriodValue.NONE
            },
            new LactationPeriod
            {
                Id = 2,
                Name = "ca. 41-80 dgn",
                StartDay = 41,
                EndDay = 80,
                Value = LactationPeriodValue.NONE
            });

        dbContext.Parities.AddRange(
            new Parity { Id = 1, Name = "Fetus", Description = "A fetus", ParityTypeValue = ParityTypeValue.FETUS },
            new Parity { Id = 2, Name = "Calf", Description = "A calf", ParityTypeValue = ParityTypeValue.CALF },
            new Parity
            {
                Id = 3, Name = "Heifer", Description = "Young female cow", ParityTypeValue = ParityTypeValue.HEIFER
            },
            new Parity
            {
                Id = 4, Name = "Adult Cow", Description = "Fully grown cow", ParityTypeValue = ParityTypeValue.ADULT_COW
            },
            new Parity
            {
                Id = 5, Name = "Old Cow", Description = "Older cow past its prime",
                ParityTypeValue = ParityTypeValue.OLD_COW
            });

        dbContext.Categories.AddRange(
            new Category { Id = 1, Name = "Enkelvoudig droog", Value = CategoryTypeValue.ENKELVOUDIG_DROOG },
            new Category { Id = 2, Name = "Enkelvoudig vochtig", Value = CategoryTypeValue.ENKELVOUDIG_VOCHTIG });

        dbContext.FeedTypes.AddRange(
            new FeedType { Id = 1, Name = "Mais", CategoryId = 1, DsProcent = 47.5 },
            new FeedType { Id = 2, Name = "Gras", CategoryId = 1, DsProcent = 60.5 });

        dbContext.Rations.AddRange(
            new Domain.Entities.Ration
            {
                Id = 1,
                Name = "Ration A",
                LivestockPropertyId = 0
            },
            new Domain.Entities.Ration
            {
                Id = 2,
                Name = "Ration B",
                LivestockPropertyId = 0
            });

        dbContext.RationFeedTypes.AddRange(
            new RationFeedType { Id = 1, FeedTypeId = 1, RationId = 1, KgAmount = 50, GAmount = 150 },
            new RationFeedType { Id = 2, FeedTypeId = 2, RationId = 1, KgAmount = 60, GAmount = 180 });

        dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllRations_ReturnsRations()
    {
        // Act
        var result = await _rationService.GetAllRations();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllRations_ReturnsEmptyList_WhenNoRationsExist()
    {
        // Arrange
        _mockDbContext.Rations.RemoveRange(_mockDbContext.Rations);
        await _mockDbContext.SaveChangesAsync();

        // Act
        var result = await _rationService.GetAllRations();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

   

    [Fact]
    public async Task CreateRation_ThrowsException_WhenRationExists()
    {
        // Arrange
        var createRationCommand = new CreateRation { Name = "Ration A" }; // Already exists

        // Act & Assert
        await Assert.ThrowsAsync<RationFeedTypeException>(() => _rationService.CreateRation(createRationCommand));
    }

    [Fact]
    public async Task RemoveRationById_RemovesRation()
    {
        // Act
        var result = await _rationService.RemoveRationById(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddFeedTypeMultiple_AddsFeedTypes()
    {
        // Arrange
        var command = new AddFeedTypeMultiple
        {
            FeedTypes = new List<FeedTypeDTO>
            {
                new FeedTypeDTO { Id = 3, Name = "New Feed", CategoryName = "Enkelvoudig vochtig", DsProcent = 70, Nutrients = new List<NutrientDTO>() }
            },
            IsEnergy = false
        };

        // Setup de IMapper mock om de mapping goed te laten verlopen
        _mockMapper.Setup(m => m.Map<FeedType>(It.IsAny<FeedTypeDTO>()))
            .Returns(new FeedType
            {
                Id = 3,
                Name = "New Feed",
                DsProcent = 70,
                CategoryId = 0
            });

        // Act
        var result = await _rationService.AddFeedTypeMultiple(1, command);

        // Assert
        Assert.True(result);

        // Controleer of de juiste FeedType is toegevoegd aan de in-memory database
        var feedType = await _mockDbContext.FeedTypes
            .FirstOrDefaultAsync(ft => ft.Name == "New Feed");

        Assert.NotNull(feedType); // Zorg ervoor dat de feedtype daadwerkelijk is toegevoegd
    }

    [Fact]
    public async Task GetFeedTypeKeys_ReturnsKeys()
    {
        // Arrange
        var command = new GetFeedTypeKeys { IsEnergy = false };

        // Act
        var result = await _rationService.GetFeedTypeKeys(1, command);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task UpdateRationFeedType_UpdatesValuesCorrectly()
    {
        // Arrange
        var command = new UpdateRationCommand
        {
            FeedTypeId = 1,
            FeedTypeKg = 80,
            FeedTypeG = 80 * (47.5m / 100), // Zorg ervoor dat FeedTypeG binnen de tolerantie valt
            IsEnergy = false
        };
        
        await _mockDbContext.SaveChangesAsync(); // Zorg ervoor dat de wijzigingen worden opgeslagen

        // Act
        var result = await _rationService.UpdateRationFeedType(1, command);

        // Assert
        Assert.True(result);

        var updatedFeedType = await _mockDbContext.RationFeedTypes
            .FirstOrDefaultAsync(rft => rft.RationId == 1 && rft.FeedTypeId == 1);

        Assert.NotNull(updatedFeedType);
        Assert.Equal(80, updatedFeedType.KgAmount); // Controleren of de waarde van KgAmount is bijgewerkt
        Assert.Equal(command.FeedTypeG, updatedFeedType.GAmount); // Controleren of de waarde van GAmount is bijgewerkt
    }
}