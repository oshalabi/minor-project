using Domain.Entities;
using Domain.EntityTypeConfiguration;
using ImportRation.Data;
using ImportRation.Exceptions;
using ImportRation.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace ImportRation.controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly BasalRationDBContext _context;
    private readonly IMessageService _messageService;

    public ImportController(BasalRationDBContext context, IMessageService messageService)
    {
        _context = context;
        _messageService = messageService;
    }


    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadFeedtype([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Please upload a valid Excel file.");

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (extension != ".xlsx")
        {
            return BadRequest("Alleen .xlsx-bestanden worden geaccepteerd.");
        }

        try
        {
            var feedTypes = await GetFeedTypes(file);

            _context.FeedTypes.AddRange(feedTypes);
            await _context.SaveChangesAsync();
            return Ok("Data inserted successfully.");
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the file: {ex.Message}");
        }
    }

    private async Task<List<FeedType>> GetFeedTypes(IFormFile file)
    {
        var feedTypes = new List<FeedType>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var stream = file.OpenReadStream();
        using var package = new ExcelPackage(stream);
        
        ValidateWorkbook(package);

        var worksheet = package.Workbook.Worksheets[0]; 
        var categoryName = file.FileName;

        var category = await GetCategoryAsync(categoryName);

        const int voersoortColumnIndex = 2;
        const int codeColumnIndex = 1;
        var dsProcentIndex = DetermineDsProcentIndex(worksheet);

        for (var row = 2; row <= worksheet.Dimension.Rows; row++) 
        {
            var voersoort = worksheet.Cells[row, voersoortColumnIndex].Text;
            var code = worksheet.Cells[row, codeColumnIndex].Text;
            var dsProcent = worksheet.Cells[row, dsProcentIndex].Text;

            if (string.IsNullOrWhiteSpace(voersoort))
                continue; 

            if (await FeedTypeExistsAsync(code))
                continue;

            var feedType = await CreateFeedTypeAsync(voersoort, code, dsProcent, category.Id);

            for (var col = 3; col <= worksheet.Dimension.Columns; col++)
            {
                var nutrientValue = worksheet.Cells[row, col].Text;
                var nutrientType = await GetOrCreateNutrientTypeAsync(worksheet.Cells[1, col].Text);

                if (decimal.TryParse(nutrientValue, out var decimalValue))
                {
                    feedType.Nutrients.Add(new Nutrient
                    {
                        NutrientTypeId = nutrientType.Id,
                        Value = decimalValue,
                        NutrientType = nutrientType
                    });
                }
            }

            await PublishFeedTypeAsync(feedType, categoryName);
            feedTypes.Add(feedType);
        }

        return feedTypes;
    }

    private void ValidateWorkbook(ExcelPackage package)
    {
        if (package.Workbook.Worksheets.Count == 0)
            throw new BadRequestException("The Excel file does not contain any worksheets.");

        if (package.Workbook.Worksheets[0].Dimension == null)
            throw new BadRequestException("The worksheet is empty.");
    }

    private async Task<Category> GetCategoryAsync(string categoryName)
    {
        var category = await _context.Categories.AsTracking()
            .SingleOrDefaultAsync(x => categoryName.ToLower().Contains(x.Name.ToLower()));

        if (category == null)
            throw new BadRequestException("Category does not exist.");

        return category;
    }

    private int DetermineDsProcentIndex(ExcelWorksheet worksheet)
    {
        return worksheet.Cells[1, 3].Text == "Type" ? 4 : 3;
    }

    private async Task<bool> FeedTypeExistsAsync(string code)
    {
        return await _context.FeedTypes.AsTracking().AnyAsync(x => x.Code == code);
    }

    private async Task<FeedType> CreateFeedTypeAsync(string name, string code, string dsProcent, int categoryId)
    {
        return new FeedType
        {
            Name = name,
            Code = code,
            DsProcent = double.Parse(dsProcent),
            CategoryId = categoryId,
            Nutrients = new List<Nutrient>()
        };
    }

    private async Task<NutrientType> GetOrCreateNutrientTypeAsync(string nutrientCode)
    {
        var cleanCode = nutrientCode.ToLower().Replace(" ", string.Empty);

        var nutrient = await _context.NutrientTypes.AsTracking()
            .FirstOrDefaultAsync(x => x.Code.ToLower().Replace(" ", string.Empty) == cleanCode);

        if (nutrient == null)
        {
            nutrient = new NutrientType
            {
                Code = nutrientCode,
                Value = (NutrientTypeValue)(await _context.NutrientTypes.CountAsync() + 1)
            };

            _context.NutrientTypes.Add(nutrient);
            await _context.SaveChangesAsync();
        }

        return nutrient;
    }

    private async Task PublishFeedTypeAsync(FeedType feedType, string categoryName)
    {
        var message = JsonConvert.SerializeObject(feedType);
        await _messageService.PublishMessageAsync(message, ResolveFeedType(categoryName));
    }

    private string ResolveFeedType(string categoryName)
    {
        switch (categoryName)
        {
            case "Enkelvoudig droog.xlsx":
                return "energyFeed";
            case "Enkelvoudig vochtig.xlsx":
                return "energyFeed";
            case "Standaard mengvoeders.xlsx":
                return "energyFeed";
            default:
                return "basalFeed";
        }
    }
}