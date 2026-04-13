using Domain.Entities;
using Domain.EntityTypeConfiguration;
using Norms.Commands;
using Norms.Controllers;
using Norms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Norms.DAL;
using OfficeOpenXml;
using Xunit;

public class NormsControllerTests
{
    private readonly NormDB _dbContext;
    private readonly NormController _controller;

    public NormsControllerTests()
    {
        var options = new DbContextOptionsBuilder<NormDB>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new NormDB(options);

        _controller = new NormController(_dbContext);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        // Seed data for LactationPeriod
        _dbContext.LactationPeriods.AddRange(
            new LactationPeriod
            {
                Id = 1,
                Name = "ca. 0-40 dgn",
                StartDay = 0,
                EndDay = 40,
                Value = LactationPeriodValue.CA0TO40
            },
            new LactationPeriod
            {
                Id = 2,
                Name = "ca. 41-80 dgn",
                StartDay = 41,
                EndDay = 80,
                Value = LactationPeriodValue.CA41TO80
            },
            new LactationPeriod
            {
                Id = 3,
                Name = "ca. 81-120 dgn",
                StartDay = 81,
                EndDay = 120,
                Value = LactationPeriodValue.CA81TO120
            },
            new LactationPeriod
            {
                Id = 4,
                Name = "ca. 121-200 dgn",
                StartDay = 121,
                EndDay = 200,
                Value = LactationPeriodValue.CA121TO200
            },
            new LactationPeriod
            {
                Id = 5,
                Name = "ca. 201-280 dgn",
                StartDay = 201,
                EndDay = 280,
                Value = LactationPeriodValue.CA201TO280
            },
            new LactationPeriod
            {
                Id = 6,
                Name = "from 281 dgn",
                StartDay = 281,
                EndDay = int.MaxValue, // No upper limit
                Value = LactationPeriodValue.FROM281
            }
        );

        // Seed data for NutrientType
        _dbContext.NutrientTypes.AddRange(
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
        _dbContext.Norms.AddRange(
            new Norm
            {
                Id = 1,
                Name = "Bzb",
                NutrientTypeId = 2,
                MinValue = 10,
                MaxValue = 20,
                RationType = RationTypeValue.BASAL
            },
            new Norm
            {
                Id = 2,
                Name = "Vem",
                NutrientTypeId = 1,
                MinValue = 20,
                MaxValue = 30,
                RationType = RationTypeValue.CONCENTRATE
            }
        );

        _dbContext.SaveChanges();
    }
    [Fact]
    public async Task CompareNorms_ReturnsOkResult_WithComparedTotals()
    {
        // Arrange
        var compareNormsCommand = new List<CompareNorms>
        {
            new CompareNorms
            {
                Group = CowGroupingEnum.LACTATION,
                Name = "CA0TO40",
                Totals = new List<NormCompare>
                { 
                    new NormCompare{ Field = "Bzb", Value = 15 },
                    new NormCompare{ Field = "Vem", Value = 25 }
                },
                TotalCows = 100,
                Total = 200,
                Fat = 3,
                Protein = 3,
                Milk = 30,
                Advices = new List<AdviceDTO>(),
                Days = 40,
                RV = 1
            }
        };

        // Act
        var result = await _controller.CompareNorms(compareNormsCommand);

        // Assert
        var okResult = Assert.IsType<Ok<List<ComparedTotalRowDTO>>>(result);
        var comparedTotals = okResult.Value;
        Assert.Single(comparedTotals);
        var comparedTotal = comparedTotals.First();
        Assert.Equal(100, comparedTotal.TotalCows);
        Assert.Equal("CA0TO40", comparedTotal.Name);
        Assert.Equal(200, comparedTotal.Total);
        Assert.Equal(3, comparedTotal.Fat);
        Assert.Equal(3, comparedTotal.Protein);
        Assert.Equal(30, comparedTotal.Milk);
        Assert.Equal(40, comparedTotal.Days);
        Assert.Equal(1, comparedTotal.RV);
        Assert.Equal(2, comparedTotal.Totals.Count);
    }
    [Fact]
    public async Task GetAllNorms_ReturnsOk_WithNorms()
    {
        // Act
        var result = await _controller.GetAllNorms();

        // Assert
        var okResult = Assert.IsType<Ok<List<Norm>>>(result);
        Assert.NotNull(okResult);
        Assert.Equal(2, okResult.Value.Count); // Controleer het aantal norms
    }
    
    [Fact]
    public async Task UploadExcel_ReturnsBadRequest_WhenFileIsNull()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = await _controller.UploadExcel(file);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please upload a valid Excel file.", badRequestResult.Value);
    }

    [Fact]
    public async Task UploadExcel_ReturnsBadRequest_WhenFileHasNoWorksheets()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.xlsx");
        fileMock.Setup(f => f.Length).Returns(1);
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        // Act
        var result = await _controller.UploadExcel(fileMock.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("The Excel file does not contain any worksheets.", badRequestResult.Value);
    }

    [Fact]
    public async Task UploadExcel_ReturnsOk_WhenValidExcelProvided()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.xlsx");
        fileMock.Setup(f => f.Length).Returns(1);

        var fakeExcelContent = CreateFakeExcelFile();
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fakeExcelContent));

        // Act
        var result = await _controller.UploadExcel(fileMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Data inserted successfully.", okResult.Value);

        var normsInDb = await _dbContext.Norms.ToListAsync();
        Assert.Equal(7, normsInDb.Count); // 2 from seed + 2 new from Excel
    }

    private byte[] CreateFakeExcelFile()
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Add headers
        worksheet.Cells[1, 1].Value = "Nutrient";
        worksheet.Cells[1, 2].Value = "Min Value";
        worksheet.Cells[1, 3].Value = "Max Value";

        // Add data
        worksheet.Cells[2, 1].Value = "Protein";
        worksheet.Cells[2, 2].Value = 12;
        worksheet.Cells[2, 3].Value = 18;

        worksheet.Cells[3, 1].Value = "Fat";
        worksheet.Cells[3, 2].Value = 6;
        worksheet.Cells[3, 3].Value = 14;

        return package.GetAsByteArray();
    }
}