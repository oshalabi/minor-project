using Domain.Entities;
using Domain.EntityTypeConfiguration;
using ImportRation.controllers;
using ImportRation.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OfficeOpenXml;

namespace ImportRation.Tests;

public class ImportControllerTests
{
    private readonly BasalRationDBContext _dbContext;
    private readonly ImportController _controller;

    public ImportControllerTests()
    {
        var options = new DbContextOptionsBuilder<BasalRationDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new BasalRationDBContext(options);

        var fakeMessageService = new FakeMessageService();
        _controller = new ImportController(_dbContext, fakeMessageService);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _dbContext.Categories.AddRange(
            new Category { Id = 1, Name = "droog", Value = CategoryTypeValue.ENKELVOUDIG_DROOG },
            new Category { Id = 2, Name = "vochtig", Value = CategoryTypeValue.ENKELVOUDIG_VOCHTIG }
        );

        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task UploadFeedtype_ReturnsBadRequest_WhenFileIsNull()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = await _controller.UploadFeedtype(file);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please upload a valid Excel file.", badRequestResult.Value);
    }

    [Fact]
    public async Task UploadFeedtype_ReturnsBadRequest_WhenFileExtensionIsInvalid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("invalidfile.txt");

        // Act
        var result = await _controller.UploadFeedtype(fileMock.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please upload a valid Excel file.", badRequestResult.Value);
    }

    [Fact]
    public async Task UploadFeedtype_ReturnsBadRequest_WhenCategoryDoesNotExist()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("nonexistentcategory.xlsx");
        fileMock.Setup(f => f.Length).Returns(1);

        var fakeExcelContent = CreateFakeExcelFile();
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fakeExcelContent));

        // Act
        var result = await _controller.UploadFeedtype(fileMock.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Category does not exist.", badRequestResult.Value);
    }

    [Fact]
    public async Task UploadFeedtype_ReturnsOk_WhenValidFileProvided()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("droog.xlsx");
        fileMock.Setup(f => f.Length).Returns(1);

        var fakeExcelContent = CreateFakeExcelFile();
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fakeExcelContent));

        // Act
        var result = await _controller.UploadFeedtype(fileMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Data inserted successfully.", okResult.Value);
    }

    private byte[] CreateFakeExcelFile()
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Add headers
        worksheet.Cells[1, 1].Value = "Code";
        worksheet.Cells[1, 2].Value = "Voersoort";
        worksheet.Cells[1, 3].Value = "DS Procent";

        // Add data
        worksheet.Cells[2, 1].Value = "CODE123";
        worksheet.Cells[2, 2].Value = "Test Voer";
        worksheet.Cells[2, 3].Value = "50";

        return package.GetAsByteArray();
    }
}