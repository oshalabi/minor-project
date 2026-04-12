using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ration.Commands;
using Ration.Exceptions;
using Ration.Model;
using Ration.Service;

namespace Ration.Controllers;

[Route("/[controller]")]
public class RationController : Controller
{
    private readonly RationService service;

    public RationController(RationService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IResult> GetAllRations()
    {
        try
        {
            var rations = await service.GetAllRations();
            return Results.Ok(rations);
        }
        catch (RationException ex)
        {
            Console.WriteLine($"Ration not found: {ex.Message}");
            return Results.NotFound(ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            return Results.Problem("An error occurred while processing your request. Please try again.");
        }
    }


    [HttpGet("{id}")]
    public async Task<IResult> GetFeedTypeOverview(int id, GetFeedTypeOverview command)
    {
        if (id == 0)
            return Results.BadRequest("Invalid ration ID.");

        try
        {
            var feedTypeOverview = await service.CreateFeedTypeOverviewDto(
                id,
                command
            );

            return Results.Ok(feedTypeOverview);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in {nameof(GetFeedTypeOverview)}({id}): {ex.Message}");
            return Results.Problem("An error occurred while processing your request. Please try again.");
        }
    }


    [HttpGet("{id}/FeedTypeKeys")]
    public async Task<IResult> GetFeedTypeKeys(int id, [FromQuery] GetFeedTypeKeys command)
    {
        try
        {
            if (id == 0 || command == null)
                return Results.NoContent();

            var associatedFeedTypeIds = await service.GetFeedTypeKeys(id, command);

            return Results.Ok(associatedFeedTypeIds);
        }
        catch (Exception e)

        {
            Console.WriteLine(e);
            return Results.Problem("An error occurred while fetching the available feed types.");
        }
    }

    [HttpPost]
    public async Task<IResult> CreateRation([FromBody] CreateRation command)
    {
        try
        {
            var ration = await service.CreateRation(command);

            return Results.Created($"Ration with name {@command.Name}, ", ration);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteRation(int id)
    {
        try
        {
            var ration = await service.RemoveRationById(id);

            return ration ? Results.NoContent() : Results.NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("{id}/AddFeedType")]
    public async Task<IResult> AddFeedType([FromBody] AddFeedTypeMultiple command, int id)
    {
        try
        {
            var feedTypes = await service.AddFeedTypeMultiple(id, command);

            return feedTypes ? Results.Ok("Feed types successfully added.") : Results.BadRequest();
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            return Results.StatusCode(500);
        }
    }


    [HttpPut("{id}/RemoveFeedType")]
    public async Task<IResult> RemoveRationFeedType(int id, [FromBody] RemoveFeedType command)
    {
        try
        {
            if (command == null)
            {
                return Results.BadRequest();
            }

            var removeFeedType = await service.RemoveFeedType(id, command);

            return removeFeedType ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRationFeedType([FromBody] UpdateRationCommand command,
        int id)
    {
        try
        {
            if (command == null || id == null)
            {
                throw new Exception("Invalid request data.");
            }

            var rationFeedType = await service.UpdateRationFeedType(id, command);


            return rationFeedType
                ? Ok(new { message = "Ration updated successfully" })
                : throw new Exception("Ration updated failed.");
        }
        catch (Exception e)
        {
            // Log the error and return an internal server error response
            Console.WriteLine(e);
            return StatusCode(500, "An error occurred while updating the ration.");
        }
    }

    [HttpGet("{id}/GetLivestockProperty")]
    public async Task<IResult> GetLivestockProperty(int id)
    {
        try
        {
            var livestockProperty = await service.GetLivestockPropertiesAsync(id);
            return Results.Ok(livestockProperty);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("{id}/CowsPerGroup")]
    public async Task<IResult> GetCowsGrouped(int id)
    {
        try
        {
            var cows = await service.GetCowsGroupedAsync(id);
            return Results.Ok(cows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("{rationId}/upload")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public async Task<IActionResult> UploadCow([FromForm] IFormFile file, int rationId)
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
            var cowsInfo = await service.GetCowsInfoAsync(file, rationId);

            return cowsInfo
                ? Ok(new { message = "Cows upload successfully" })
                : throw new Exception("Cows upload failed.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the file: {ex.Message}");
        }
    }
    
    [HttpGet("cows")]
    public async Task<IResult> GetCows()
    {
        try
        {
            var cows = await service.GetCowsAsync();
            return Results.Ok(cows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPut("livestockProperties/{rationId}")]
    public async Task<IResult> UpdateLivestockProperties([FromBody] UpdateLivestockProperties command, int rationId)
    {
        try
        {
            var result = await service.UpdateLivestockPropertiesAsync(rationId, command);
            
            return Results.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("{id}/EnergyFeedSettings")]
    public async Task<IResult> GetEnergyFeedSettings(int id)
    {
        try
        {
            var energyFeedSettings = await service.GetEnergyFeedSettingsForRationAsync(id);

            if (!energyFeedSettings.Any())
                return Results.NoContent();

            return Results.Ok(energyFeedSettings);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return Results.Problem("An error occurred while fetching the energy feed settings.");
        }
    }

    [HttpPost("SetEnergyFeedSettings")]
    public async Task<IActionResult> SetEnergyFeedSettings([FromBody] EnergyFeedSettingsDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Invalid data.");
        }

        var result = await service.UpdateEnergyFeedSettingsAsync(dto);

        if (result)
        {
            return Ok("Energy feed settings saved successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while saving the data.");
        }
    }
}