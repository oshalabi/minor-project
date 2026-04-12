using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Norms.Migrations
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
                name: "Category",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LactationPeriods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
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
                    Code = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parity",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ParityTypeValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedType",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DsProcent = table.Column<double>(type: "double precision", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedType_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "public",
                        principalTable: "Category",
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
                    Name = table.Column<string>(type: "text", nullable: false),
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
                name: "Ration",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LivestockPropertyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ration_LivestockProperties_LivestockPropertyId",
                        column: x => x.LivestockPropertyId,
                        principalSchema: "public",
                        principalTable: "LivestockProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Norms",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RationType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MinValue = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    NutrientTypeId = table.Column<int>(type: "integer", nullable: true),
                    LactationPeriodId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Norms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Norms_LactationPeriods_LactationPeriodId",
                        column: x => x.LactationPeriodId,
                        principalSchema: "public",
                        principalTable: "LactationPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Norms_NutrientTypes_NutrientTypeId",
                        column: x => x.NutrientTypeId,
                        principalSchema: "public",
                        principalTable: "NutrientTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Nutrient",
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
                    table.PrimaryKey("PK_Nutrient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nutrient_NutrientTypes_NutrientTypeId",
                        column: x => x.NutrientTypeId,
                        principalSchema: "public",
                        principalTable: "NutrientTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cow",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Cow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cow_Farm_FarmId",
                        column: x => x.FarmId,
                        principalSchema: "public",
                        principalTable: "Farm",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cow_LactationPeriods_LactationPeriodId",
                        column: x => x.LactationPeriodId,
                        principalSchema: "public",
                        principalTable: "LactationPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cow_Parity_ParityId",
                        column: x => x.ParityId,
                        principalSchema: "public",
                        principalTable: "Parity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RationFeedType",
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
                    table.PrimaryKey("PK_RationFeedType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RationFeedType_FeedType_FeedTypeId",
                        column: x => x.FeedTypeId,
                        principalSchema: "public",
                        principalTable: "FeedType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RationFeedType_Ration_RationId",
                        column: x => x.RationId,
                        principalSchema: "public",
                        principalTable: "Ration",
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
                        name: "FK_FeedTypeNutrient_FeedType_FeedTypesId",
                        column: x => x.FeedTypesId,
                        principalSchema: "public",
                        principalTable: "FeedType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedTypeNutrient_Nutrient_NutrientsId",
                        column: x => x.NutrientsId,
                        principalSchema: "public",
                        principalTable: "Nutrient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advice",
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
                    table.PrimaryKey("PK_Advice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advice_Cow_CowId",
                        column: x => x.CowId,
                        principalSchema: "public",
                        principalTable: "Cow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Advice_CowId",
                schema: "public",
                table: "Advice",
                column: "CowId");

            migrationBuilder.CreateIndex(
                name: "IX_Cow_FarmId",
                schema: "public",
                table: "Cow",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Cow_LactationPeriodId",
                schema: "public",
                table: "Cow",
                column: "LactationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Cow_ParityId",
                schema: "public",
                table: "Cow",
                column: "ParityId");

            migrationBuilder.CreateIndex(
                name: "IX_Farm_LivestockFeedAdvisorId",
                schema: "public",
                table: "Farm",
                column: "LivestockFeedAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedType_CategoryId",
                schema: "public",
                table: "FeedType",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedTypeNutrient_NutrientsId",
                schema: "public",
                table: "FeedTypeNutrient",
                column: "NutrientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Norms_LactationPeriodId",
                schema: "public",
                table: "Norms",
                column: "LactationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Norms_NutrientTypeId",
                schema: "public",
                table: "Norms",
                column: "NutrientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nutrient_NutrientTypeId",
                schema: "public",
                table: "Nutrient",
                column: "NutrientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ration_LivestockPropertyId",
                schema: "public",
                table: "Ration",
                column: "LivestockPropertyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RationFeedType_FeedTypeId",
                schema: "public",
                table: "RationFeedType",
                column: "FeedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RationFeedType_RationId",
                schema: "public",
                table: "RationFeedType",
                column: "RationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advice",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FeedTypeNutrient",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Norms",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RationFeedType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Cow",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Nutrient",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FeedType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Ration",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Farm",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LactationPeriods",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Parity",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NutrientTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Category",
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
