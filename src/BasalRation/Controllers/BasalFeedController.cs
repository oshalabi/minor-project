using AutoMapper;
using BasalRation.Commands;
using BasalRation.DAL;
using BasalRation.Model;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasalRation.Controllers;

[Route("[controller]")]
public class BasalFeedController(BasalFeedDB dbContext, IMapper mapper) : Controller
{
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IResult> GetAllFeedTypes()
    {
        try
        {
            var feedTypes = await dbContext.FeedTypes
                .Include(ft => ft.Category)
                .Include(ft => ft.Nutrients)!
                .ThenInclude(n => n.NutrientType)
                .Select(ft => new FeedTypeDTO
                {
                    Id = ft.Id,
                    Code = ft.Code,
                    Name = ft.Name,
                    DsProcent = ft.DsProcent,
                    CategoryName = ft.Category.Name,
                    Nutrients = ft.Nutrients != null
                        ? ft.Nutrients.Select(n => new NutrientDTO
                        {
                            Id = n.Id,
                            Code = n.NutrientType != null ? n.NutrientType.Code : null,
                            Value = n.Value
                        }).ToList()
                        : new List<NutrientDTO>()
                })
                .ToListAsync();

            return Results.Ok(feedTypes);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching feed types: {e.Message}");
            return Results.Problem("An error occurred while fetching feed types.");
        }
    }
    
    [HttpPost("AvailableFeedTypes")]
    public async Task<IResult> GetAvailableFeedTypes([FromBody] GetAvailableFeedTypes @command)
    {
        try
        {

            var categories = dbContext.Categories.Where(x => @command.CategoryValues.Contains(x.Value)).ToList();
            
            if (categories.Count == 0)
                return Results.NotFound();
            
            var availableFeedTypes = await dbContext.FeedTypes.Include(x => x.Category).Include(x => x.Nutrients).ThenInclude(x => x.NutrientType)
                .Where(ft => @command.CategoryValues.Contains(ft.Category.Value))
                .Where(ft => !@command.FeedTypeKeys.Contains((ft.Code.Length > 0 ? ft.Code.ToLower() + "_" : "") + ft.Name.ToLower().Replace(" ", "")))
                .Select(ft => new FeedTypeDTO
                {
                    Id = ft.Id,
                    Code = ft.Code,
                    Name = ft.Name,
                    DsProcent = ft.DsProcent,
                    CategoryName = ft.Category.Name,
                    Nutrients = ft.Nutrients.Select(n => new NutrientDTO
                    {
                        Id = n.Id,
                        Code = n.NutrientType != null ? n.NutrientType.Code : null,
                        Value = n.Value,
                        NutrientType = new NutrientTypeDTO
                        {
                            Id = n.NutrientTypeId,
                            Code = n.NutrientType != null ? n.NutrientType.Code : null,
                            Value = (int)n.NutrientType.Value
                        }
                    }).ToList()
                })
                .ToListAsync();

            return Results.Ok(availableFeedTypes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem("An error occurred while fetching the available feed types.");
        }
    }


    [HttpGet]
    [Route("GetAllNutrients")]
    public async Task<IResult> GetAllNutrients()
    {
        try
        {
            var nutrientTypes = await dbContext.NutrientTypes.ToListAsync();
            return Results.Ok(nutrientTypes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}