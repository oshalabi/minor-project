using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ration.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyFeedSettings",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RationId = table.Column<int>(type: "integer", nullable: false),
                    ParityId = table.Column<int>(type: "integer", nullable: false),
                    FeedTypeId = table.Column<int>(type: "integer", nullable: false),
                    MaxEnergyFeed = table.Column<int>(type: "integer", nullable: false),
                    MinEnergyFeed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyFeedSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LactationPeriods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDay = table.Column<int>(type: "integer", nullable: false),
                    EndDay = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LactationPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockFeedAdvisor",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockFeedAdvisor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockProperties",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MilkFat = table.Column<decimal>(type: "numeric", nullable: true),
                    MilkProtein = table.Column<decimal>(type: "numeric", nullable: true),
                    AvgWeightCow = table.Column<int>(type: "integer", nullable: true),
                    TotalCows = table.Column<int>(type: "integer", nullable: true),
                    OldCows = table.Column<int>(type: "integer", nullable: true),
                    SecondCalves = table.Column<int>(type: "integer", nullable: true),
                    Heiffers = table.Column<int>(type: "integer", nullable: true),
                    ProductionLevel = table.Column<decimal>(type: "numeric", nullable: true),
                    Netto = table.Column<decimal>(type: "numeric", nullable: true),
                    CalvingAge = table.Column<int>(type: "integer", nullable: true),
                    AvgLactationDays = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutrientTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parities",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ParityTypeValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DsProcent = table.Column<double>(type: "double precision", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedTypes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "public",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farm",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    LivestockFeedAdvisorId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farm_LivestockFeedAdvisor_LivestockFeedAdvisorId",
                        column: x => x.LivestockFeedAdvisorId,
                        principalSchema: "public",
                        principalTable: "LivestockFeedAdvisor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LivestockPropertyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rations_LivestockProperties_LivestockPropertyId",
                        column: x => x.LivestockPropertyId,
                        principalSchema: "public",
                        principalTable: "LivestockProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Norm",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RationType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MinValue = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric", nullable: true),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    NutrientTypeId = table.Column<int>(type: "integer", nullable: true),
                    LactationPeriodId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Norm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Norm_LactationPeriods_LactationPeriodId",
                        column: x => x.LactationPeriodId,
                        principalSchema: "public",
                        principalTable: "LactationPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Norm_NutrientTypes_NutrientTypeId",
                        column: x => x.NutrientTypeId,
                        principalSchema: "public",
                        principalTable: "NutrientTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Nutrients",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    NutrientTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nutrients_NutrientTypes_NutrientTypeId",
                        column: x => x.NutrientTypeId,
                        principalSchema: "public",
                        principalTable: "NutrientTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cows",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    Milk = table.Column<decimal>(type: "numeric", nullable: false),
                    Fat = table.Column<decimal>(type: "numeric", nullable: false),
                    Protein = table.Column<decimal>(type: "numeric", nullable: false),
                    RV = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    ParityId = table.Column<int>(type: "integer", nullable: false),
                    LactationPeriodId = table.Column<int>(type: "integer", nullable: false),
                    FarmId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cows_Farm_FarmId",
                        column: x => x.FarmId,
                        principalSchema: "public",
                        principalTable: "Farm",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cows_LactationPeriods_LactationPeriodId",
                        column: x => x.LactationPeriodId,
                        principalSchema: "public",
                        principalTable: "LactationPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cows_Parities_ParityId",
                        column: x => x.ParityId,
                        principalSchema: "public",
                        principalTable: "Parities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farmer",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FarmId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farmer_Farm_FarmId",
                        column: x => x.FarmId,
                        principalSchema: "public",
                        principalTable: "Farm",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RationFeedTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KgAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    GAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsEnergyFeed = table.Column<bool>(type: "boolean", nullable: false),
                    FeedTypeId = table.Column<int>(type: "integer", nullable: false),
                    RationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RationFeedTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RationFeedTypes_FeedTypes_FeedTypeId",
                        column: x => x.FeedTypeId,
                        principalSchema: "public",
                        principalTable: "FeedTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ration_BasalFeeds",
                        column: x => x.RationId,
                        principalSchema: "public",
                        principalTable: "Rations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedTypeNutrient",
                schema: "public",
                columns: table => new
                {
                    FeedTypesId = table.Column<int>(type: "integer", nullable: false),
                    NutrientsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedTypeNutrient", x => new { x.FeedTypesId, x.NutrientsId });
                    table.ForeignKey(
                        name: "FK_FeedTypeNutrient_FeedTypes_FeedTypesId",
                        column: x => x.FeedTypesId,
                        principalSchema: "public",
                        principalTable: "FeedTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedTypeNutrient_Nutrients_NutrientsId",
                        column: x => x.NutrientsId,
                        principalSchema: "public",
                        principalTable: "Nutrients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advices",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    CowId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advices_Cows_CowId",
                        column: x => x.CowId,
                        principalSchema: "public",
                        principalTable: "Cows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, null, "Enkelvoudig droog", 1 },
                    { 2, null, "Enkelvoudig vochtig", 2 },
                    { 3, null, "Mineralen", 3 },
                    { 4, null, "Standaard mengvoeders", 4 },
                    { 5, null, "Standaard ruwvoeders", 5 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "LactationPeriods",
                columns: new[] { "Id", "EndDay", "Name", "StartDay", "Value" },
                values: new object[,]
                {
                    { 1, 40, "ca. 0-40 dgn", 0, 1 },
                    { 2, 80, "ca. 41-80 dgn", 41, 2 },
                    { 3, 120, "ca. 81-120 dgn", 81, 3 },
                    { 4, 200, "ca. 121-200 dgn", 121, 4 },
                    { 5, 280, "ca. 201-280 dgn", 201, 5 },
                    { 6, 2147483647, "from 281 dgn", 281, 6 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "LivestockProperties",
                columns: new[] { "Id", "AvgLactationDays", "AvgWeightCow", "CalvingAge", "Heiffers", "MilkFat", "MilkProtein", "Netto", "OldCows", "ProductionLevel", "SecondCalves", "TotalCows" },
                values: new object[,]
                {
                    { 1, null, 650, null, null, 4m, 3m, null, null, null, null, null },
                    { 2, null, 650, null, null, 4m, 3m, null, null, null, null, null },
                    { 3, null, 650, null, null, 4m, 3m, null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "NutrientTypes",
                columns: new[] { "Id", "Code", "Value" },
                values: new object[,]
                {
                    { 1, "Vem", 1 },
                    { 2, "Bzb", 2 },
                    { 3, "Dvp", 3 },
                    { 4, "Oep", 4 },
                    { 5, "PercentRv", 5 },
                    { 6, "VemDvp", 6 },
                    { 7, "Zw", 7 },
                    { 8, "Bw", 8 },
                    { 9, "BzbRv", 9 },
                    { 10, "Fop", 10 },
                    { 11, "Wfkh", 11 },
                    { 12, "Wfre", 12 },
                    { 13, "WfreWfkh", 13 },
                    { 14, "Sfkh", 14 },
                    { 15, "Sfre", 15 },
                    { 16, "Lacto", 16 },
                    { 17, "Re", 17 },
                    { 18, "ReInc", 18 },
                    { 19, "PercentDs", 19 },
                    { 20, "Ndf", 20 },
                    { 21, "DpMeth", 21 },
                    { 22, "DpLys", 22 },
                    { 23, "DpHis", 23 },
                    { 24, "DpLeu", 24 },
                    { 25, "Sui", 25 },
                    { 26, "Zet", 26 },
                    { 27, "Nfc", 27 },
                    { 28, "Rc", 28 },
                    { 29, "SuSaz", 29 },
                    { 30, "Bzet", 30 },
                    { 31, "Ras", 31 },
                    { 32, "Nh3", 32 },
                    { 33, "Ca", 33 },
                    { 34, "CaP", 34 },
                    { 35, "NP", 35 },
                    { 36, "P", 36 },
                    { 37, "Na", 37 },
                    { 38, "KNa", 38 },
                    { 39, "K", 39 },
                    { 40, "MgK", 40 },
                    { 41, "Mg", 41 },
                    { 42, "Cl", 42 },
                    { 43, "S", 43 },
                    { 44, "Kav", 44 },
                    { 45, "MnMg", 45 },
                    { 46, "ZnMg", 46 },
                    { 47, "FeMg", 47 },
                    { 48, "CuMg", 48 },
                    { 49, "MoMg", 49 },
                    { 50, "JoMg", 50 },
                    { 51, "CoMcg", 51 },
                    { 52, "SeMcg", 52 },
                    { 53, "AIE", 53 },
                    { 54, "D3IE", 54 },
                    { 55, "EIE", 55 },
                    { 56, "Niacine", 56 },
                    { 57, "Choline", 57 },
                    { 58, "Biotine", 58 },
                    { 59, "Rvet", 59 },
                    { 60, "Klauw", 60 },
                    { 61, "Uier", 61 },
                    { 62, "Vruchtbaarheid", 62 },
                    { 63, "KgMelk", 63 },
                    { 64, "EiwitPercent", 64 },
                    { 65, "EiwitGram", 65 },
                    { 66, "VetGram", 66 },
                    { 67, "VEGram", 67 },
                    { 68, "TotaleZuren", 68 },
                    { 69, "Ureum", 69 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Parities",
                columns: new[] { "Id", "Description", "Name", "ParityTypeValue" },
                values: new object[,]
                {
                    { 1, "A fetus", "Fetus", 1 },
                    { 2, "A calf", "Calf", 2 },
                    { 3, "Young female cow", "Heifer", 3 },
                    { 4, "Fully grown cow", "Adult Cow", 4 },
                    { 5, "Older cow past its prime", "Old Cow", 5 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "FeedTypes",
                columns: new[] { "Id", "CategoryId", "Code", "DsProcent", "Name" },
                values: new object[,]
                {
                    { 1, 1, null, 47.5, "Mais" },
                    { 2, 1, null, 60.5, "Gras" },
                    { 3, 1, null, 90.5, "Graan" },
                    { 4, 1, null, 45.5, "Pellets" },
                    { 5, 1, null, 60.5, "Mais+" },
                    { 6, 1, null, 90.5, "Pellets+" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Nutrients",
                columns: new[] { "Id", "NutrientTypeId", "Value" },
                values: new object[,]
                {
                    { 1, 6, 80m },
                    { 2, 8, 92m },
                    { 3, 30, 70m },
                    { 4, 5, 85m },
                    { 5, 5, 75m },
                    { 6, 30, 88m },
                    { 7, 30, 65m },
                    { 8, 5, 80m },
                    { 9, 7, 85m },
                    { 10, 8, 95m },
                    { 11, 7, 90m },
                    { 12, 8, 100m },
                    { 13, 1, 120m },
                    { 14, 4, 90m },
                    { 15, 2, 75m },
                    { 16, 3, 110m },
                    { 17, 1, 95m },
                    { 18, 4, 85m },
                    { 19, 2, 65m },
                    { 20, 3, 105m },
                    { 21, 1, 110m },
                    { 22, 4, 70m },
                    { 23, 2, 80m },
                    { 24, 3, 120m },
                    { 25, 1, 130m },
                    { 26, 4, 75m },
                    { 27, 2, 85m },
                    { 28, 3, 115m },
                    { 29, 1, 90m },
                    { 30, 4, 95m },
                    { 31, 2, 70m },
                    { 32, 3, 125m },
                    { 33, 1, 90m },
                    { 34, 4, 95m },
                    { 35, 2, 70m },
                    { 36, 3, 125m }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Rations",
                columns: new[] { "Id", "LivestockPropertyId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Ration A" },
                    { 2, 2, "Ration B" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "FeedTypeNutrient",
                columns: new[] { "FeedTypesId", "NutrientsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 13 },
                    { 1, 14 },
                    { 1, 15 },
                    { 1, 16 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 17 },
                    { 2, 18 },
                    { 2, 19 },
                    { 2, 20 },
                    { 3, 5 },
                    { 3, 6 },
                    { 3, 21 },
                    { 3, 22 },
                    { 3, 23 },
                    { 3, 24 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 25 },
                    { 4, 26 },
                    { 4, 27 },
                    { 4, 28 },
                    { 5, 9 },
                    { 5, 10 },
                    { 5, 29 },
                    { 5, 30 },
                    { 5, 31 },
                    { 5, 32 },
                    { 6, 11 },
                    { 6, 12 },
                    { 6, 33 },
                    { 6, 34 },
                    { 6, 35 },
                    { 6, 36 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "RationFeedTypes",
                columns: new[] { "Id", "FeedTypeId", "GAmount", "IsEnergyFeed", "KgAmount", "RationId" },
                values: new object[,]
                {
                    { 1, 1, 0m, false, 0m, 1 },
                    { 2, 2, 0m, false, 0m, 1 },
                    { 3, 3, 0m, false, 0m, 1 },
                    { 4, 4, 0m, false, 0m, 2 },
                    { 5, 5, 0m, true, 0m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advices_CowId",
                schema: "public",
                table: "Advices",
                column: "CowId");

            migrationBuilder.CreateIndex(
                name: "IX_Cows_FarmId",
                schema: "public",
                table: "Cows",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Cows_LactationPeriodId",
                schema: "public",
                table: "Cows",
                column: "LactationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Cows_ParityId",
                schema: "public",
                table: "Cows",
                column: "ParityId");

            migrationBuilder.CreateIndex(
                name: "IX_Farm_LivestockFeedAdvisorId",
                schema: "public",
                table: "Farm",
                column: "LivestockFeedAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Farmer_FarmId",
                schema: "public",
                table: "Farmer",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTypeNutrient_NutrientsId",
                schema: "public",
                table: "FeedTypeNutrient",
                column: "NutrientsId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTypes_CategoryId",
                schema: "public",
                table: "FeedTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Norm_LactationPeriodId",
                schema: "public",
                table: "Norm",
                column: "LactationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Norm_NutrientTypeId",
                schema: "public",
                table: "Norm",
                column: "NutrientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_NutrientTypeId",
                schema: "public",
                table: "Nutrients",
                column: "NutrientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RationFeedTypes_FeedTypeId",
                schema: "public",
                table: "RationFeedTypes",
                column: "FeedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RationFeedTypes_RationId",
                schema: "public",
                table: "RationFeedTypes",
                column: "RationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rations_LivestockPropertyId",
                schema: "public",
                table: "Rations",
                column: "LivestockPropertyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advices",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EnergyFeedSettings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Farmer",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FeedTypeNutrient",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Norm",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RationFeedTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Cows",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Nutrients",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FeedTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Rations",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Farm",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LactationPeriods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Parities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NutrientTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LivestockProperties",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LivestockFeedAdvisor",
                schema: "public");
        }
    }
}
