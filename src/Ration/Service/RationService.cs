using AutoMapper;
using Domain.Entities;
using Domain.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Ration.Commands;
using Ration.DAL;
using Ration.Enums;
using Ration.Exceptions;
using Ration.Mappers;
using Ration.Model;

namespace Ration.Service;

public class RationService
{
    private readonly RationDB _dbContext;
    private readonly IMapper _mapper;

    public RationService(RationDB dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<Domain.Entities.Ration>> GetAllRations()
    {
        try
        {
            var rations = await _dbContext.Rations.ToListAsync();
            if (rations == null)
            {
                throw new RationException("There are no rations found");
            }

            return rations;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<Domain.Entities.Ration> GetRationById(int id)
    {
        try
        {
            var ration = await _dbContext.Rations
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (ration == null)
            {
                throw new RationException("Ration not found");
            }

            return ration;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<Domain.Entities.Ration> GetRationWithFeedTypes(int id)
    {
        try
        {
            var ration = await _dbContext.Rations
                .AsTracking()
                .Include(x => x.FeedTypes)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (ration == null)
            {
                throw new RationException("There are no rations found");
            }

            return ration;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<List<RationFeedType>> GetFilteredFeedTypesAsync(
        IEnumerable<RationFeedType> feedTypes,
        bool isEnergy)
    {
        try
        {
            var feedTypeList = feedTypes.ToList();

            var rationFeedTypes =
                await Task.FromResult(feedTypeList.Where(x => x.IsEnergyFeed == isEnergy).ToList());

            return rationFeedTypes;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<List<FeedType>> GetFeedTypesWithNutrients(
        List<RationFeedType> rationFeedTypes)
    {
        if (rationFeedTypes == null || !rationFeedTypes.Any())
            return new List<FeedType>();

        var feedTypeIds = rationFeedTypes.Select(rft => rft.FeedTypeId).ToList();

        var feedTypes = await _dbContext.FeedTypes.AsTracking()
            .Include(ft => ft.Nutrients)
            .ThenInclude(n => n.NutrientType)
            .Where(ft => feedTypeIds.Contains(ft.Id))
            .ToListAsync();


        if (feedTypes == null || !feedTypes.Any())
            throw new InvalidOperationException(
                "No feed types found for the given ration feed types.");

        return feedTypes;
    }

    private decimal CalculateTotalKgDs(List<RationFeedType> rationFeedTypes)
    {
        return rationFeedTypes.Sum(rft => rft.GAmount);
    }

    private List<KeyValuePair<string, int>> CalculateAverageTotalFeedTypes(
        List<FeedType> feedTypes,
        List<RationFeedType> rationFeedTypes,
        decimal rationTotalKgDs)
    {
        return feedTypes
            .SelectMany(ft => ft.Nutrients ?? new List<Nutrient>(),
                (ft, nutrient) => new { ft.Id, nutrient })
            .GroupBy(x => x.nutrient.NutrientType?.Code ?? "")
            .Select(g =>
            {
                var totalWeightedValue = g.Sum(x => x.nutrient.Value * rationFeedTypes
                    .FirstOrDefault(rft => rft.FeedTypeId == x.Id)?.GAmount ?? 0);

                return new KeyValuePair<string, int>(
                    g.Key,
                    rationTotalKgDs > 0
                        ? (int)Math.Round(totalWeightedValue / rationTotalKgDs,
                            MidpointRounding.AwayFromZero)
                        : 0
                );
            })
            .ToList();
    }

    private int CalculateDsProcentWeightedSum(List<RationFeedType> rationFeedTypes)
    {
        return (int)Math.Round(
            rationFeedTypes.Sum(bf => bf.GAmount * ((decimal)bf.FeedType.DsProcent / 100) * 1000),
            MidpointRounding.AwayFromZero);
    }

    public async Task<FeedTypeOverviewDTO> CreateFeedTypeOverviewDto(int id,
        GetFeedTypeOverview command)
    {
        var ration = await GetRationWithFeedTypes(id);

        if (ration == null)
        {
            throw new RationException($"Ration with ID {id} not found");
        }

        if (ration.FeedTypes == null || ration.FeedTypes.Count == 0)
        {
            return new FeedTypeOverviewDTO
            {
                Id = id,
                Name = ration.Name,
                FeedTypes = null
            };
        }

        var rationFeedTypes = await GetFilteredFeedTypesAsync(ration.FeedTypes, command.IsEnergy);
        var feedTypes = await GetFeedTypesWithNutrients(rationFeedTypes);

        var rationTotalKgDs = CalculateTotalKgDs(rationFeedTypes);
        var averageTotalFeedTypes =
            CalculateAverageTotalFeedTypes(feedTypes, rationFeedTypes, rationTotalKgDs);
        var dsProcentWeightedSum = CalculateDsProcentWeightedSum(rationFeedTypes);

        var result = new FeedTypeOverviewDTO
        {
            Id = id,
            Name = ration.Name,
            RationTotalKg = rationFeedTypes.Sum(rft => rft.KgAmount),
            RationTotalKgDs = rationTotalKgDs,
            AverageTotalFeedTypes = averageTotalFeedTypes,
            DsProcentWeightedSum = dsProcentWeightedSum,
            FeedTypes = feedTypes.Select(ft => new RationFeedTypeDTO
            {
                FeedType = new FeedTypeDTO
                {
                    Id = ft.Id,
                    Name = ft.Name,
                    DsProcent = ft.DsProcent,
                    Nutrients = ft.Nutrients?.Select(n => new NutrientDTO
                    {
                        Id = n.Id,
                        Code = n.NutrientType?.Code ?? "",
                        Value = n.Value
                    }).ToList() ?? new List<NutrientDTO>()
                },
                KgAmount = rationFeedTypes.Where(rft => rft.FeedTypeId == ft.Id)
                    .Sum(rft => rft.KgAmount),
                GAmount = rationFeedTypes.Where(rft => rft.FeedTypeId == ft.Id)
                    .Sum(rft => rft.GAmount)
            }).ToList()
        };

        return result;
    }


    public async Task<List<string>> GetFeedTypeKeys(int id, GetFeedTypeKeys command)
    {
        try
        {
            var associatedFeedTypeKeys = await _dbContext.RationFeedTypes
                .Where(x => x.RationId == id && x.IsEnergyFeed == command.IsEnergy)
                .Select(x =>
                    (x.FeedType.Code.Length > 0
                        ? x.FeedType.Code.ToLower() + "_"
                        : "") + x.FeedType.Name.ToLower().Replace(" ", ""))
                .ToListAsync();

            return associatedFeedTypeKeys;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<RationDTO> CreateRation(CreateRation @command)
    {
        try
        {
            var ration = await _dbContext.Rations.Where(x => x.Name.Equals(@command.Name))
                .ToListAsync();

            if (ration.Count > 0)
            {
                throw new RationFeedTypeException("There is already a Ration with the name " +
                                                  command.Name);
            }

            var livestockProperty = new LivestockProperties();
            await _dbContext.AddAsync(livestockProperty);
            await _dbContext.SaveChangesAsync();

            var newRation = _mapper.Map<Domain.Entities.Ration>(@command);
            newRation.LivestockPropertyId = livestockProperty.Id;

            await _dbContext.AddAsync(newRation);
            await _dbContext.SaveChangesAsync();

            var mappedRationDto = _mapper.Map<RationDTO>(newRation);

            return mappedRationDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> RemoveRationById(int id)
    {
        try
        {
            var ration = await GetRationById(id);
            _dbContext.Rations.Remove(ration);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> AddFeedTypeMultiple(int id, AddFeedTypeMultiple command)
    {
        try
        {
            var ration = await _dbContext.Rations
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (ration == null)
                return false;

            var feedTypesToAdd = new List<FeedType>();
            var existingFeedTypes = new List<FeedType>();

            foreach (var feedTypeDto in command.FeedTypes)
            {
                var feedTypeDTOKey = (feedTypeDto.Code?.Length > 0
                    ? feedTypeDto.Code.ToLower() + "_"
                    : "") + feedTypeDto.Name.ToLower().Replace(" ", "");

                var existingFeedType = await _dbContext.FeedTypes
                    .AsTracking()
                    .FirstOrDefaultAsync(x =>
                        (x.Code.Length > 0 ? x.Code.ToLower() + "_" : "") +
                        x.Name.ToLower().Replace(" ", "") == feedTypeDTOKey);

                if (existingFeedType != null)
                {
                    existingFeedTypes.Add(existingFeedType);
                    continue;
                }

                var newFeedType = _mapper.Map<FeedType>(feedTypeDto);
                newFeedType.Nutrients = new List<Nutrient>();

                var category = await _dbContext.Categories
                    .AsTracking()
                    .SingleOrDefaultAsync(x =>
                        x.Name.ToLower() == feedTypeDto.CategoryName.ToLower());

                if (category == null)
                    return false;

                newFeedType.CategoryId = category.Id;
                newFeedType.Category = category;

                foreach (var nutrientDto in feedTypeDto.Nutrients)
                {
                    var nutrientType = await _dbContext.NutrientTypes
                        .FirstOrDefaultAsync(nt =>
                            nt.Value == (NutrientTypeValue)nutrientDto.NutrientType.Value);

                    if (nutrientType == null)
                    {
                        nutrientType = new NutrientType
                        {
                            Code = nutrientDto.Code,
                            Value = (NutrientTypeValue)nutrientDto.NutrientType.Value
                        };
                        await _dbContext.NutrientTypes.AddAsync(nutrientType);
                    }

                    var nutrient = new Nutrient
                    {
                        NutrientType = nutrientType,
                        NutrientTypeId = nutrientType.Id,
                        Value = nutrientDto.Value
                    };

                    newFeedType.Nutrients.Add(nutrient);
                }

                feedTypesToAdd.Add(newFeedType);
            }

            if (feedTypesToAdd.Any())
            {
                await _dbContext.FeedTypes.AddRangeAsync(feedTypesToAdd);
            }

            var allFeedTypes = existingFeedTypes.Concat(feedTypesToAdd).ToList();

            var parityTypeDict = await _dbContext.Parities
                .AsTracking()
                .ToDictionaryAsync(x => x.ParityTypeValue, x => x.Id);

            var existingEnergyFeedSettings = await _dbContext.EnergyFeedSettings
                .Where(x => x.RationId == id && allFeedTypes.Select(f => f.Id).Contains(x.FeedTypeId))
                .ToListAsync();

            if (existingEnergyFeedSettings.Any())
            {
                _dbContext.EnergyFeedSettings.RemoveRange(existingEnergyFeedSettings);
            }

            var energyFeedSettings = new List<EnergyFeedSettings>();

            foreach (var feedType in allFeedTypes)
            {
                var rationFeedType = new RationFeedType
                {
                    FeedTypeId = feedType.Id,
                    RationId = id,
                    IsEnergyFeed = command.IsEnergy,
                };

                if (command.IsEnergy)
                {
                    var parityValues = new List<ParityTypeValue>
                    {
                        ParityTypeValue.OLD_COW,
                        ParityTypeValue.ADULT_COW,
                        ParityTypeValue.HEIFER
                    };

                    foreach (var parityValue in parityValues)
                    {
                        if (parityTypeDict.TryGetValue(parityValue, out var parityId))
                        {
                            var energyFeedSetting = new EnergyFeedSettings
                            {
                                RationId = id,
                                ParityId = parityId,
                                FeedTypeId = feedType.Id,
                                MaxEnergyFeed = 0,
                                MinEnergyFeed = 0
                            };

                            energyFeedSettings.Add(energyFeedSetting);
                        }
                    }
                }

                await _dbContext.RationFeedTypes.AddAsync(rationFeedType);
                await _dbContext.EnergyFeedSettings.AddRangeAsync(energyFeedSettings);
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            return false;
        }
    }

    private async Task<List<FeedType>> PrepareFeedTypesToAdd(AddFeedTypeMultiple command)
    {
        var feedTypesToAdd = new List<FeedType>();

        foreach (var feedTypeDto in command.FeedTypes)
        {
            var existingFeedType = await _dbContext.FeedTypes
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Code == feedTypeDto.Code);

            if (existingFeedType != null)
            {
                feedTypesToAdd.Add(existingFeedType);
                continue;
            }

            var newFeedType = _mapper.Map<FeedType>(feedTypeDto);
            newFeedType.Nutrients = await PrepareNutrients(feedTypeDto);

            var category = await _dbContext.Categories
                .AsTracking()
                .SingleOrDefaultAsync(x => x.Name.ToLower() == feedTypeDto.CategoryName.ToLower());

            if (category == null)
                throw new CategoryException(
                    $"Category '{feedTypeDto.CategoryName}' does not exist.");

            newFeedType.CategoryId = category.Id;
            feedTypesToAdd.Add(newFeedType);
        }

        return feedTypesToAdd;
    }

    private async Task<List<Nutrient>> PrepareNutrients(FeedTypeDTO feedTypeDto)
    {
        var nutrients = new List<Nutrient>();

        foreach (var nutrientDto in feedTypeDto.Nutrients)
        {
            var nutrientType = await _dbContext.NutrientTypes
                .FirstOrDefaultAsync(nt =>
                    nt.Value == (NutrientTypeValue)nutrientDto.NutrientType.Value);

            if (nutrientType == null)
            {
                nutrientType = new NutrientType
                {
                    Code = nutrientDto.Code,
                    Value = (NutrientTypeValue)nutrientDto.NutrientType.Value
                };
                await _dbContext.NutrientTypes.AddAsync(nutrientType);
            }

            var nutrient = new Nutrient
            {
                NutrientType = nutrientType,
                NutrientTypeId = nutrientType.Id,
                Value = nutrientDto.Value
            };

            nutrients.Add(nutrient);
        }

        return nutrients;
    }

    private async Task<bool> AddFeedTypesToRationAsync(int rationId, List<FeedType> feedTypesToAdd,
        bool isEnergy)
    {
        var rationFeedTypes = feedTypesToAdd.Select(feedType => new RationFeedType
        {
            FeedTypeId = feedType.Id,
            RationId = rationId,
            IsEnergyFeed = isEnergy
        }).ToList();

        await _dbContext.RationFeedTypes.AddRangeAsync(rationFeedTypes);
        return await _dbContext.SaveChangesAsync() > 0;
    }


    public async Task<bool> RemoveFeedType(int id, RemoveFeedType command)
    {
        try
        {
            var rationFeedType = await _dbContext.RationFeedTypes.AsTracking()
                .SingleAsync(x =>
                    x.RationId == id && x.FeedTypeId == command.FeedTypeId &&
                    x.IsEnergyFeed == command.IsEnergy);

            if (rationFeedType == null)
            {
                throw new RationException($"Basal Ration with ID {command.FeedTypeId} not found.");
            }

            _dbContext.RationFeedTypes.Remove(rationFeedType);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateRationFeedType(int id, UpdateRationCommand command)
    {
        try
        {
            var rationFeedType = await _dbContext.RationFeedTypes
                .Include(x => x.FeedType)
                .AsTracking()
                .SingleOrDefaultAsync(x =>
                    x.RationId == id && x.FeedTypeId == command.FeedTypeId &&
                    x.IsEnergyFeed == command.IsEnergy);

            if (rationFeedType == null)
            {
                throw new RationException(
                    $"RationFeedType with RationId {id} and FeedTypeId {command.FeedTypeId} not found.");
            }

            var dsProcent = rationFeedType.FeedType.DsProcent;
            if (dsProcent <= 0)
            {
                throw new RationException("Invalid DS percentage value.");
            }

            var calculated = command.FeedTypeKg * ((decimal)dsProcent / 100);

            const decimal tolerance = 1m;

            if (Math.Abs(command.FeedTypeG - calculated) > tolerance)
            {
                throw new RationException(
                    "The provided data does not match the calculated value based on DsProcent.");
            }

            rationFeedType.KgAmount = command.FeedTypeKg;
            rationFeedType.GAmount = command.FeedTypeG;

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<CowGroupingDTO>> GetCowsGroupedAsync(int id)
    {
        var mappedCows = new List<CowGroupingDTO>();

        try
        {
            var cows = await _dbContext.Cows
                .AsTracking().Include(x => x.Advices)
                .ToListAsync();

            foreach (var group in Enum.GetValues(typeof(CowGroupingEnum)))
            {
                var mappedGroup = await GetCowsGroupingAsync((CowGroupingEnum)group, cows);
                mappedCows.AddRange(mappedGroup);
            }

            return mappedCows;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    private async Task<List<CowGroupingDTO>> GetCowsGroupingAsync(CowGroupingEnum group, List<Cow> cows)
    {
        var mappedGroups = new List<CowGroupingDTO>();
        try
        {
            switch (group)
            {
                case CowGroupingEnum.TOTAL:
                    var cowsTotal = GetMappedCows(cows, CowGroupingEnum.TOTAL.ToString(), group);
                    mappedGroups.Add(await cowsTotal);
                    break;

                case CowGroupingEnum.LACTATION:
                    var lactationDict = await _dbContext.LactationPeriods
                        .AsTracking()
                        .ToDictionaryAsync(x => x.Value, x => x.Id);

                    foreach (var lactation in lactationDict)
                    {
                        var cowsForLactation =
                            cows.Where(x => x.LactationPeriodId == lactation.Value).ToList();
                        var name = lactation.Key.ToString();

                        if (cowsForLactation.Count > 0)
                        {
                            var mappedCow = GetMappedCows(cowsForLactation, name, group);
                            mappedGroups.Add(await mappedCow);
                        }
                    }

                    break;

                case CowGroupingEnum.PARITY:
                    var parityDict = await _dbContext.Parities
                        .AsTracking()
                        .ToDictionaryAsync(x => x.ParityTypeValue, x => x.Id);

                    foreach (var parity in parityDict)
                    {
                        var cowsForParity = cows.Where(x => x.ParityId == parity.Value).ToList();
                        var name = parity.Key.ToString();

                        if (cowsForParity.Count > 0)
                        {
                            var mappedCow = GetMappedCows(cowsForParity, name, group);
                            mappedGroups.Add(await mappedCow);
                        }
                    }

                    break;
            }


            return mappedGroups;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    private Task<CowGroupingDTO> GetMappedCows(List<Cow> cows, string name, CowGroupingEnum group)
    {
        var mappedCows = new CowGroupingDTO
        {
            TotalCows = cows.Count,
            Name = name,
            Days = cows.Select(x => x.Days).Average(),
            Milk = cows.Select(x => x.Milk).Average(),
            Fat = cows.Select(x => x.Fat).Average(),
            Protein = cows.Select(x => x.Protein).Average(),
            RV = cows.Select(x => x.RV).Average(),
            Total = cows.Select(x => x.Total).Average(),
            Advices = cows
                .SelectMany(x => x.Advices)
                .GroupBy(advice => advice.Id)
                .Select(group => new AdviceDTO
                {
                    Id = group.Key,
                    Value = Math.Round(group.Average(advice => advice.Value), 2)
                })
                .ToList(),
            Group = group
        };

        return Task.FromResult(mappedCows);
    }

    public async Task<bool> GetCowsInfoAsync(IFormFile file, int rationId)
    {
        var lactationPeriods = await _dbContext.LactationPeriods.ToListAsync();
        var parities = await _dbContext.Parities.ToListAsync();
        var cows = new List<Cow>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        await using var stream = file.OpenReadStream();
        using var package = new ExcelPackage(stream);
        try
        {
            var worksheet = GetWorksheet(package, "kvoverzicht alle koeien");
            if (worksheet == null) return false;

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    var cow = ProcessCowRow(worksheet, row, lactationPeriods, parities);
                    cow.Advices = GetAdvicesForCow(worksheet, row, cow.Id, rationId);

                    cows.Add(cow);

                    if (cows.Count == 100)
                    {
                        await SaveCowsAsync(cows);
                        cows.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing row {row}: {ex.Message}");
                }
            }

            if (cows.Count <= 0) return true;
            await SaveCowsAsync(cows);
            cows.Clear();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex.Message}");
            return false;
        }
    }

    private ExcelWorksheet? GetWorksheet(ExcelPackage package, string sheetName)
    {
        var worksheet = package.Workbook.Worksheets[sheetName];
        if (worksheet != null) return worksheet;
        Console.WriteLine($"Worksheet '{sheetName}' not found.");
        return null;
    }

    private Cow ProcessCowRow(
        ExcelWorksheet worksheet,
        int row,
        List<LactationPeriod> lactationPeriods,
        List<Parity> parities)
    {
        var days = Convert.ToInt32(worksheet.Cells[row, 4].Text); // Column D - Dgn
        var lactationPeriod = GetLactationPeriod(days, lactationPeriods);
        if (lactationPeriod == null)
        {
            throw new Exception($"No lactation period found for Days = {days}.");
        }

        var parityId = GetParityId(days, parities);

        var cowName = $"***** {row}";
        var cowNumber = Convert.ToInt32(worksheet.Cells[row, 2].Text);


        var milk = Convert.ToDecimal(worksheet.Cells[row, 5].Text);
        var fat = Convert.ToDecimal(worksheet.Cells[row, 6].Text);
        var protein = Convert.ToDecimal(worksheet.Cells[row, 7].Text);
        var total = Convert.ToDecimal(worksheet.Cells[row, 13].Text);
        var rv = Convert.ToDecimal(worksheet.Cells[row, 19].Text);
        var cow = new Cow
        {
            Id = cowNumber,
            Name = cowName,
            Days = days,
            Milk = milk,
            Fat = fat,
            Protein = protein,
            Total = total,
            RV = rv,
            LactationPeriodId = lactationPeriod.Id,
            ParityId = parityId,
            Advices = [],
        };

        return cow;
    }

    private static LactationPeriod GetLactationPeriod(int days, List<LactationPeriod> lactationPeriods)
    {
        return lactationPeriods.FirstOrDefault(lp => days >= lp.StartDay && days <= lp.EndDay) ??
               throw new Exception("Lactation period not found.");
    }

    private static int GetParityId(int days, List<Parity> parities)
    {
        if (days <= 150)
        {
            return parities.FirstOrDefault(p => p.ParityTypeValue == ParityTypeValue.HEIFER)?.Id ??
                   throw new Exception("Parity not found for days <= 150.");
        }
        else if (days < 300)
        {
            return parities.FirstOrDefault(p => p.ParityTypeValue == ParityTypeValue.CALF)?.Id ??
                   throw new Exception("Parity not found for days between 150 and 300.");
        }
        else
        {
            return parities.FirstOrDefault(p => p.ParityTypeValue == ParityTypeValue.ADULT_COW)?.Id ??
                   throw new Exception("Parity not found for days > 300.");
        }
    }

    private static List<Advice> GetAdvicesForCow(ExcelWorksheet worksheet, int row, int cowId, int rationId)
    {
        var columnIndexes = new List<int>([8, 9, 10, 11]);
        var advices = new List<Advice>();
        for (var i = 0; i < 4; i++)
        {
            var advice = new Advice
            {
                Id = i,
                Value = Convert.ToDecimal(worksheet.Cells[row, columnIndexes[i]].Text),
                CowId = cowId
            };
            advices.Add(advice);
        }

        return advices;
    }

    private async Task SaveCowsAsync(List<Cow> cows)
    {
        if (cows == null || cows.Count == 0) return;

        try
        {
            await _dbContext.Cows.AddRangeAsync(cows);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"{cows.Count} cows saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving cows: {ex.Message}");
        }
    }


    public async Task<List<CowDTO>> GetCowsAsync()
    {
        try
        {
            var cows = await _dbContext.Cows.Include(c => c.Advices)
                .Include(c => c.Parity)
                .Include(c => c.LactationPeriod).ToListAsync();
            return cows.Select(cow => new CowDTO
            {
                Id = cow.Id,
                Name = cow.Name,
                Days = cow.Days,
                Milk = cow.Milk,
                Fat = cow.Fat,
                Protein = cow.Protein,
                RV = cow.RV,
                Total = cow.Total,
                ParityId = cow.ParityId,
                ParityName = cow.Parity?.Name,
                LactationId = cow.LactationPeriodId,
                LactationName = cow.LactationPeriod?.Name,
                Advices = cow.Advices.Select(advice => new AdviceDTO
                {
                    Id = advice.Id,
                    Value = advice.Value
                }).ToList()
            }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IResult> UpdateLivestockPropertiesAsync(int rationId,
        UpdateLivestockProperties command)
    {
        var livestockProperties = await GetLivestockPropertiesAsync(rationId);

        var updatedLivestockProperties = _mapper.Map<LivestockProperties>(command);

        await _dbContext.SaveChangesAsync();

        return Results.Ok("Livestock properties updated.");
    }

    public async Task<LivestockPropertiesDTO> GetLivestockPropertiesAsync(int rationId)
    {
        try
        {
            var livestockProperties = await _dbContext.Rations.AsTracking().Include(x => x.LivestockProperties)
                .Select(x => x.LivestockProperties).SingleOrDefaultAsync(x => x.Id == rationId);

            if (livestockProperties == null)
                throw new Exception("No livestock properties were found for this ration.");

            var mappedLivestockProperties = _mapper.Map<LivestockPropertiesDTO>(livestockProperties);

            return mappedLivestockProperties;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<EnergyFeedSettingsDto>> GetEnergyFeedSettingsForRationAsync(int rationId)
    {
        var associatedEnergyFeedTypes = await _dbContext.RationFeedTypes
            .Where(r => r.RationId == rationId && r.IsEnergyFeed == true)
            .ToListAsync();

        if (!associatedEnergyFeedTypes.Any())
            return new List<EnergyFeedSettingsDto>();

        var energyFeedTypeIds = associatedEnergyFeedTypes.Select(r => r.FeedTypeId).ToList();

        var energyFeedSettings = await _dbContext.EnergyFeedSettings
            .Where(e => e.RationId == rationId && energyFeedTypeIds.Contains(e.FeedTypeId))
            .Join(
                _dbContext.FeedTypes,
                e => e.FeedTypeId,
                f => f.Id,
                (e, f) => new EnergyFeedSettingsDto
                {
                    RationId = e.RationId,
                    ParityId = e.ParityId,
                    FeedTypeId = e.FeedTypeId,
                    FeedTypeName = f.Name,
                    MinEnergyFeed = e.MinEnergyFeed,
                    MaxEnergyFeed = e.MaxEnergyFeed
                }
            )
            .ToListAsync();

        return energyFeedSettings;
    }

    public async Task<bool> UpdateEnergyFeedSettingsAsync(EnergyFeedSettingsDto dto)
    {
        var existingSettings = await _dbContext.EnergyFeedSettings
            .FirstOrDefaultAsync(e =>
                e.RationId == dto.RationId && e.FeedTypeId == dto.FeedTypeId && e.ParityId == dto.ParityId);

        if (existingSettings != null)
        {
            existingSettings.MinEnergyFeed = dto.MinEnergyFeed;
            existingSettings.MaxEnergyFeed = dto.MaxEnergyFeed;
            _dbContext.EnergyFeedSettings.Update(existingSettings);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}