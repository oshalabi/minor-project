using AutoMapper;
using BasalRation.Commands;
using Domain.Entities;
using EnergyFood.Commands;
using EnergyFood.DAL;
using EnergyFood.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GetAvailableFeedTypes = EnergyFood.Model.GetAvailableFeedTypes;

namespace EnergyFood.Controllers;

[Route("[controller]/[action]")]
public class EnergyFoodController : Controller
{
    private readonly EnergyFoodDB _dbContext;
    private readonly IMapper _mapper;

    public EnergyFoodController(EnergyFoodDB dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpPost("{rationId}/Graph")]
    public async Task<IResult> GetGraphInformation([FromBody] ShowMilkProduction @command,
        int rationId)
    {
        try
        {
            var livestockProperties = await _dbContext.Rations.AsTracking()
                .Include(x => x.LivestockProperties).Select(x => x.LivestockProperties).SingleOrDefaultAsync(x => x.Id == rationId);

            if (livestockProperties == null)
            {
                return Results.NotFound();
            }
            if (livestockProperties.AvgWeightCow == null || livestockProperties.MilkFat == null || livestockProperties.MilkProtein == null)
                return Results.BadRequest();
            
            
            var energyVetEiwit = 0.337m + 0.116m * livestockProperties.MilkFat +
                                 0.06m * livestockProperties.MilkProtein;
            
            var VEM_Maintenance = 431.0605 +
                                  0.06996 * Math.Pow((double)livestockProperties.AvgWeightCow,
                                      0.75);
            var termA =
                (decimal)Math.Pow(VEM_Maintenance,
                    2); // Cast to double for Math.Pow, then cast to decimal
            var termB = 4 * 0.7293m *
                        ((41.3506m * (decimal)Math.Pow((double)livestockProperties.AvgWeightCow,
                             0.75)) -
                         @command.BasalRationVEMBasicAmount); // Convert Math.Pow result to decimal
            var discriminant = termA - termB; // Both are now decimals

            if (discriminant < 0)
            {
                return Results.Problem("Negative discriminant, unable to calculate square root.");
            }

            var melkproductieVEM =
                ((-(decimal)VEM_Maintenance + (decimal)Math.Sqrt((double)discriminant)) / 1.4586m) /
                energyVetEiwit;

            return Results.Ok(new GraphDTO
            {
                MelkproductieVEM = melkproductieVEM ?? 0,
                EnergyFoodAmount = @command.EnergyFoodAmount,
            });
        }
        catch (Exception ex)
        {
            return Results.Problem("An unexpected error occurred: " + ex.Message);
        }
    }

    [HttpGet]
    public async Task<IResult> GetAllEnergyFoods()
    {
        try
        {
            var energyFoodTypes = await _dbContext.FeedTypes.ToListAsync();
            return Results.Ok(energyFoodTypes);
        }
        catch (Exception e)
        {
            return Results.Problem("An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPatch]
    public async Task<IResult> RemoveEnergyFood(RemoveEnergyFood @command)
    {
        try
        {
            var energyFood = await _dbContext.FeedTypes.AsTracking()
                .SingleAsync(x => x.Id == @command.Id);

            if (energyFood == null)
                return Results.NotFound();

            _dbContext.FeedTypes.Remove(energyFood);
            await _dbContext.SaveChangesAsync();

            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.Problem("An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPost("AvailableFeedTypes")]
    public async Task<IResult> GetAvailableFeedTypes([FromBody] GetAvailableFeedTypes @command)
    {
        try
        {
            var catogies = _dbContext.Categories
                .Where(x => @command.CategoryValues.Contains(x.Value)).ToList();

            if (catogies.Count == 0)
                return Results.NotFound();

            var availableFeedTypes = await _dbContext.FeedTypes.Include(x => x.Category)
                .Include(x => x.Nutrients).ThenInclude(x => x.NutrientType)
                .Where(ft => @command.CategoryValues.Contains(ft.Category.Value))
                .Where(ft => !@command.FeedTypeKeys.Contains(ft.Code.Length > 0
                    ? ft.Code.ToLower()
                    : "" + "_" + ft.Name.ToLower().Trim()))
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
    public async Task<IResult> GetAllNutrients()
    {
        try
        {
            var nutrientTypes = await _dbContext.NutrientTypes.ToListAsync();
            return Results.Ok(nutrientTypes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}