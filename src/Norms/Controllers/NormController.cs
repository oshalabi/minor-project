using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.EntityTypeConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Norms.Commands;
using Norms.DAL;
using Norms.Models;
using OfficeOpenXml;

namespace Norms.Controllers;

[ApiController]
[Route("[controller]")]
public class NormController : ControllerBase
{
    private readonly NormDB _dbContext;

    public NormController(NormDB dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IResult> GetAllNorms()
    {
        try
        {
            var norms = await _dbContext.Norms.ToListAsync();
            return Results.Ok(norms);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("CompareNorms")]
    public async Task<IResult> CompareNorms([FromBody] List<CompareNorms> command)
    {
        try
        {
            var norms = await _dbContext.Norms.ToListAsync();
            var nutrientTypeDict = await _dbContext.NutrientTypes
                .AsTracking()
                .ToDictionaryAsync(x => x.Code, x => x.Id);

            var lactationDict = await _dbContext.LactationPeriods
                .AsTracking()
                .ToDictionaryAsync(p => p.Value, p => p.Id);

            var comparedTotals = command.Select(compare =>
            {
                var normss = norms;
                if (compare.Group == CowGroupingEnum.LACTATION)
                {
                    LactationPeriodValue parsedStatus = Enum.Parse<LactationPeriodValue>(compare.Name);
                    if (lactationDict.TryGetValue(parsedStatus, out var lactationId))
                    {
                        normss = norms.Where(n => n.LactationPeriodId == lactationId).ToList();
                    }
                }

                var totals = GetComparedColumns(compare.Totals, normss, nutrientTypeDict);
                return new ComparedTotalRowDTO
                {
                    TotalCows = compare.TotalCows,
                    Name = compare.Name,
                    Total = compare.Total,
                    Fat = compare.Fat,
                    Protein = compare.Protein,
                    Milk = compare.Milk,
                    Advices = compare.Advices,
                    Days = compare.Days,
                    RV = compare.RV,
                    Totals = totals,
                    Group = compare.Group,
                };
            }).ToList();

            return Results.Ok(comparedTotals);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("CompareNormsAllCows")]
    public async Task<IResult> CompareNormsAllCows([FromBody] List<CompareNormsCowCommand> command)
    {
        try
        {
            var norms = await _dbContext.Norms.ToListAsync();
            var nutrientTypeDict = await _dbContext.NutrientTypes
                .AsTracking()
                .ToDictionaryAsync(x => x.Code, x => x.Id);

            var comparedTotals = command.Select(compare =>
            {
                var normss = norms.Where(n => n.LactationPeriodId == compare.LactationId).ToList();
                var totals = GetComparedColumns(compare.Totals, normss, nutrientTypeDict);

                return new ComparedCowRowDTO
                {
                    Id = compare.Id,
                    Totals = totals,
                };
            }).ToList();

            return Results.Ok(comparedTotals);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private List<ComparedColumnDTO> GetComparedColumns(
        ICollection<NormCompare> totals,
        List<Norm> norms,
        Dictionary<string, int> nutrientTypeDict)
    {
        var result = new List<ComparedColumnDTO>();
        foreach (var item in totals)
        {
            if (nutrientTypeDict.TryGetValue(item.Field, out var nutrientTypeId))
            {
                var norm = norms.FirstOrDefault(x => x.NutrientTypeId == nutrientTypeId);

                if (norm != null)
                {
                    result.Add(new ComparedColumnDTO
                    {
                        Field = item.Field,
                        Value = item.Value,
                        Warning = CalculateNormWarningIndex(item.Value, norm),
                        Norm = CalculateNormWarningIndex(item.Value, norm) > 0 ? GetNormValue(norm) : null
                    });
                }
                else
                {
                    result.Add(new ComparedColumnDTO
                    {
                        Field = item.Field,
                        Value = item.Value,
                        Warning = 0
                    });
                }
            }
            else
            {
                result.Add(new ComparedColumnDTO
                {
                    Field = item.Field,
                    Value = item.Value,
                    Warning = 0
                });
            }
        }

        return result;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Please upload a valid Excel file.");

        var norms = new List<Norm>();

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var stream = file.OpenReadStream();
            using var package = new ExcelPackage(stream);

            // Validate workbook and worksheet
            if (package.Workbook.Worksheets.Count == 0)
                return BadRequest("The Excel file does not contain any worksheets.");

            var worksheet = package.Workbook.Worksheets[0]; // Use the first worksheet
            if (worksheet.Dimension == null)
                return BadRequest("The worksheet is empty.");

            var columnCount = worksheet.Dimension.Columns;
            var rowCount = worksheet.Dimension.Rows;

            // Index of the "voersoort" column (adjust if needed)
            var subjectColumnIndex = 1;
            var basalRVColumnIndex = 2;
            var remarkColumnIndex = 9;

            for (var row = 3; row <= rowCount; row++) // Start at row 2 to skip the header
            {
                var subject = worksheet.Cells[row, subjectColumnIndex].Text;

                if (string.IsNullOrWhiteSpace(subject))
                    continue; // Skip empty rows


                var basalRationTypeRV = worksheet.Cells[row, basalRVColumnIndex].Text;
                var remark = worksheet.Cells[row, remarkColumnIndex].Text;
                var totRation0Min = worksheet.Cells[row, 3].Text;
                var totRation0Max = worksheet.Cells[row, 4].Text;
                var totRation60Min = worksheet.Cells[row, 5].Text;
                var totRation60Max = worksheet.Cells[row, 6].Text;
                var basRation200Min = worksheet.Cells[row, 7].Text;
                var basRation200Max = worksheet.Cells[row, 8].Text;

                var nutrientType = await _dbContext.NutrientTypes.AsTracking().FirstOrDefaultAsync(x =>
                    subject.ToLower().Contains(x.Code.ToLower()));

                var norm = new Norm
                {
                    Name = subject,
                    RationType = RationTypeValue.BASAL,
                    MaxValue = basalRationTypeRV.Length > 0 ? Decimal.Parse(CleanInput(basalRationTypeRV)) : null,
                    Remark = remark,
                    NutrientTypeId = nutrientType?.Id
                };


                var norm0to40 = new Norm
                {
                    Name = subject,
                    RationType = RationTypeValue.BASAL,
                    MinValue = totRation0Min.Length > 0 ? Decimal.Parse(CleanInput(totRation0Min)) : null,
                    MaxValue = totRation0Max.Length > 0 ? Decimal.Parse(CleanInput(totRation0Max)) : null,
                    Remark = remark,
                    LactationPeriodId = (int)LactationPeriodValue.CA0TO40,
                    NutrientTypeId = nutrientType?.Id
                };

                var norm40to80 = new Norm
                {
                    Name = subject,
                    RationType = RationTypeValue.BASAL,
                    MinValue = totRation0Min.Length > 0 ? Decimal.Parse(CleanInput(totRation0Min)) : null,
                    MaxValue = totRation0Max.Length > 0 ? Decimal.Parse(CleanInput(totRation0Max)) : null,
                    Remark = remark,
                    LactationPeriodId = (int)LactationPeriodValue.CA41TO80,
                    NutrientTypeId = nutrientType?.Id
                };

                var norm80to120 = new Norm
                {
                    Name = subject,
                    RationType = RationTypeValue.TOTAL,
                    MinValue = totRation60Min.Length > 0 ? Decimal.Parse(CleanInput(totRation60Min)) : null,
                    MaxValue = totRation60Max.Length > 0 ? Decimal.Parse(CleanInput(totRation60Max)) : null,
                    Remark = remark,
                    LactationPeriodId = (int)LactationPeriodValue.CA81TO120,
                    NutrientTypeId = nutrientType?.Id
                };

                var norm200to300 = new Norm
                {
                    Name = subject,
                    RationType = RationTypeValue.BASAL,
                    MinValue = basRation200Min.Length > 0 ? Decimal.Parse(CleanInput(basRation200Min)) : null,
                    MaxValue = basRation200Max.Length > 0 ? Decimal.Parse(CleanInput(basRation200Max)) : null,
                    Remark = remark,
                    LactationPeriodId = (int)LactationPeriodValue.CA201TO280,
                    NutrientTypeId = nutrientType?.Id
                };

                // for (var col = 3; col <= columnCount; col++) // Start from column 3 for nutrients
                // {
                //     var cellText = worksheet.Cells[row, col].Text;
                //     var nutrient = await _dbContext.NutrientTypes
                //         .AsTracking()
                //         .FirstOrDefaultAsync(x => x.Code.Contains(worksheet.Cells[1, col].Text));
                //
                //     if (nutrient == null)
                //     {
                //         continue;
                //     }
                //     if (decimal.TryParse(cellText, out var decimalValue))
                //         
                //         basalRation.Nutrients.Add(new Nutrient
                //         {
                //             NutrientTypeId = nutrient.Id, // Use header row for nutrient code
                //             Value = decimalValue
                //         });
                // }

                norms.AddRange([norm, norm0to40, norm40to80, norm80to120, norm200to300]);
            }


            // // Get existing names from the database
            // var existingNames = await _context.BasalRations
            //     .Select(br => br.Name)
            //     .ToListAsync();
            //
            // // Filter out duplicates
            // var newBasalRations = basalRations
            //     .Where(br => !existingNames.Contains(br.Name, StringComparer.OrdinalIgnoreCase))
            //     .ToList();
            //
            // if (!newBasalRations.Any())
            //     return Ok("All basal rations already exist.");

            _dbContext.Norms.AddRange(norms);
            await _dbContext.SaveChangesAsync();
            return Ok("Data inserted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the file: {ex.Message}");
        }
    }

    private string CleanInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove non-numeric characters except for decimal points and negative signs
        return Regex.Replace(input, @"[^0-9.-]", "");
    }

    private int CalculateNormWarningIndex(decimal value, Norm norm)
    {
        var warningIndex = 0;

        if (norm.MaxValue * (decimal)1.1 < value || norm.MinValue * (decimal)0.9 > value)
            warningIndex = 2;

        else if (norm.MinValue > value || norm.MaxValue < value)
            warningIndex = 1;

        return warningIndex;
    }

    private string GetNormValue(Norm norm)
    {
        var minValue = norm.MinValue;
        var maxValue = norm.MaxValue;

        if (minValue != null && maxValue != null)
            return $"{minValue} - {maxValue}";


        if (minValue != null)
            return $"Min: {minValue}";


        if (maxValue != null)
            return "Max: {maxValue}";

        return string.Empty;
    }
}