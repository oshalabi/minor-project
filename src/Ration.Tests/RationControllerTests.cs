using AutoMapper;
using Domain.Entities;
using Domain.EntityTypeConfiguration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Ration.Commands;
using Ration.Controllers;
using Ration.DAL;
using Ration.Model;
using Ration.Service;
using Xunit;

namespace Ration.Tests
{
    public class RationControllerTests
    {
        private readonly RationController _rationController;
        private readonly RationService _rationService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RationDB _mockDbContext;

        public RationControllerTests()
        {
            // Mock de DbContext
            var options = new DbContextOptionsBuilder<RationDB>()
                .UseInMemoryDatabase("RationServiceTestsDB")
                .Options;

            _mockDbContext = new RationDB(options); // Maak een echte in-memory database aan voor de tests

            _mockMapper = new Mock<IMapper>();

            // Initialiseer de service met de echte DbContext (in-memory database) en de IMapper mock
            _rationService = new RationService(_mockDbContext, _mockMapper.Object);

            // Initialiseer de controller
            _rationController = new RationController(_rationService);

            // Seed testdata in de in-memory database
            SeedTestData(_mockDbContext);
        }

        private void SeedTestData(RationDB dbContext)
        {
            if (!dbContext.LivestockProperties.Any())
            {
                dbContext.LivestockProperties.Add(new LivestockProperties()
                {
                    Id = 1,
                    MilkFat = 4, MilkProtein = 3
                });
                dbContext.LivestockProperties.Add(new LivestockProperties()
                {
                    Id = 2,
                    MilkFat = 4,
                    MilkProtein = 3
                });
                dbContext.LivestockProperties.Add(new LivestockProperties()
                {
                    Id = 3,
                    MilkFat = 4,
                    MilkProtein = 3
                });
            }

            if (!dbContext.RationFeedTypes.Any())
            {
                dbContext.RationFeedTypes.Add(new RationFeedType
                {
                    Id = 1,
                    KgAmount = 0m,
                    GAmount = 0m,
                    FeedTypeId = 1, // Mais
                    RationId = 1, // Ration 1
                });
                dbContext.RationFeedTypes.Add(new RationFeedType
                {
                    Id = 2,
                    KgAmount = 0m,
                    GAmount = 0m,
                    FeedTypeId = 2, // Gras
                    RationId = 1, // Ration 1
                });
            }

            // Voeg testdata toe aan de DbContext, maar controleer of de entiteit al bestaat
            if (!dbContext.LactationPeriods.Any())
            {
                dbContext.LactationPeriods.AddRange(
                    new LactationPeriod
                    {
                        Id = 1, Name = "ca. 0-40 dgn", StartDay = 0, EndDay = 40, Value = LactationPeriodValue.CA0TO40
                    },
                    new LactationPeriod
                    {
                        Id = 2, Name = "ca. 41-80 dgn", StartDay = 41, EndDay = 80,
                        Value = LactationPeriodValue.CA41TO80
                    },
                    new LactationPeriod
                    {
                        Id = 3, Name = "ca. 81-120 dgn", StartDay = 81, EndDay = 120,
                        Value = LactationPeriodValue.CA81TO120
                    }
                );
            }

            if (!dbContext.Parities.Any())
            {
                dbContext.Parities.AddRange(
                    new Parity
                    {
                        Id = 1, Name = "Fetus", Description = "A fetus", ParityTypeValue = ParityTypeValue.FETUS
                    },
                    new Parity { Id = 2, Name = "Calf", Description = "A calf", ParityTypeValue = ParityTypeValue.CALF }
                );
            }

            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(
                    new Category { Id = 1, Name = "Enkelvoudig droog", Value = CategoryTypeValue.ENKELVOUDIG_DROOG },
                    new Category { Id = 2, Name = "Enkelvoudig vochtig", Value = CategoryTypeValue.ENKELVOUDIG_VOCHTIG }
                );
            }

            if (!dbContext.FeedTypes.Any())
            {
                dbContext.FeedTypes.AddRange(
                    new FeedType { Id = 1, Name = "Mais", CategoryId = 1, DsProcent = 47.5 },
                    new FeedType { Id = 2, Name = "Gras", CategoryId = 1, DsProcent = 60.5 }
                );
            }

            if (!dbContext.Rations.Any())
            {
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
                    }
                );
            }

            dbContext.SaveChanges(); // Sla de veranderingen op in de in-memory database
        }

        [Fact]
        public async Task GetAllRations_ReturnsRations()
        {
            // Act
            var result = await _rationController.GetAllRations();

            // Assert
            var okResult = Assert.IsType<Ok<List<Domain.Entities.Ration>>>(result);

            Assert.NotNull(okResult.Value);
            Assert.Equal(2, okResult.Value.Count); // We hebben 2 rations toegevoegd in SeedTestData
        }

        [Fact]
        public async Task CreateRation_CreatesRation()
        {
            // Arrange
            var command = new CreateRation { Name = "New Ration" };
            var createdRation = new Domain.Entities.Ration
            {
                Id = 3,
                Name = "New Ration",
                LivestockPropertyId = 0
            };

            // Mock de mapper om een Ration object te retourneren bij het mappen van het CreateRation commando
            _mockMapper.Setup(m => m.Map<Domain.Entities.Ration>(It.IsAny<CreateRation>()))
                .Returns(createdRation);

            // Act
            var result = await _rationController.CreateRation(command);

            // Assert
            var createdResult = Assert.IsType<Created>(result);
        }

        [Fact]
        public async Task DeleteRation_ReturnsNoContent()
        {
            // Arrange
            var rationId = 1;

            // Act
            var result = await _rationController.DeleteRation(rationId);

            // Assert
            Assert.IsType<NoContent>(result);
        }

        [Fact]
        public async Task GetFeedTypeKeys_ReturnsFeedTypeKeys()
        {
            // Arrange
            var command = new GetFeedTypeKeys { IsEnergy = false };

            // Act
            var result = await _rationController.GetFeedTypeKeys(1, command);

            // Assert
            var okResult = Assert.IsType<Ok<List<string>>>(result);
            var feedTypeKeysResult = okResult.Value;

            Assert.NotNull(feedTypeKeysResult);
            Assert.NotEmpty(feedTypeKeysResult);
        }

    }
}