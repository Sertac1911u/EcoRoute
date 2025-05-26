using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoRoute.Settings.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DateFormats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormatString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sample = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FontTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CssValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FontTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CultureCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThemeColors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DarkMode = table.Column<bool>(type: "bit", nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableAnimations = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LocationTracking = table.Column<bool>(type: "bit", nullable: false),
                    SessionTimeout = table.Column<int>(type: "int", nullable: false),
                    ActiveSessionLimit = table.Column<int>(type: "int", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    SmsNotifications = table.Column<bool>(type: "bit", nullable: false),
                    PushNotifications = table.Column<bool>(type: "bit", nullable: false),
                    GoogleMapsApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FontTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateFormatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSettings_DateFormats_DateFormatId",
                        column: x => x.DateFormatId,
                        principalTable: "DateFormats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemSettings_FontTypes_FontTypeId",
                        column: x => x.FontTypeId,
                        principalTable: "FontTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemSettings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Avatars",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { new Guid("452023df-7041-49a6-b734-5126425b0df1"), "Avatar 6", "https://api.dicebear.com/9.x/shapes/svg?seed=Emir" },
                    { new Guid("5cdff67f-c74f-4f5d-a5a3-8759ba4f0408"), "Avatar 14", "https://api.dicebear.com/9.x/shapes/svg?seed=Riley" },
                    { new Guid("5d4e5768-ca9e-4bcf-8f55-372448562cfb"), "Avatar 10", "https://api.dicebear.com/9.x/shapes/svg?seed=Felix" },
                    { new Guid("6a761e65-69cc-4495-ba89-a5b8b9d20090"), "Avatar 3", "https://api.dicebear.com/9.x/shapes/svg?seed=Atlas" },
                    { new Guid("6cce009f-c386-4e07-a861-db3ef77ab823"), "Avatar 5", "https://api.dicebear.com/9.x/shapes/svg?seed=Nova" },
                    { new Guid("996fe3c8-2dbd-4df4-92b1-20b66b95c58f"), "Avatar 9", "https://api.dicebear.com/9.x/shapes/svg?seed=Arda" },
                    { new Guid("9ef68935-8de3-4738-b6e1-079bc2fd8dc4"), "Avatar 7", "https://api.dicebear.com/9.x/shapes/svg?seed=Jasper" },
                    { new Guid("aaa09375-9210-4c3e-8a10-a470605aadef"), "Avatar 1", "https://api.dicebear.com/9.x/shapes/svg?seed=Can" },
                    { new Guid("aafd1b74-1c4b-4fdf-a059-4803351e48ea"), "Avatar 8", "https://api.dicebear.com/9.x/shapes/svg?seed=Lina" },
                    { new Guid("ac9f3a06-0b84-4947-8ad9-c9eacac72ebf"), "Avatar 13", "https://api.dicebear.com/9.x/shapes/svg?seed=Selin" },
                    { new Guid("b5854e59-78df-439e-9d63-1502b8383628"), "Avatar 15", "https://api.dicebear.com/9.x/shapes/svg?seed=Ayaz" },
                    { new Guid("bdd6e1bc-a4d3-4b14-9c7e-2db63b3f3414"), "Avatar 4", "https://api.dicebear.com/9.x/shapes/svg?seed=Deniz" },
                    { new Guid("c3a2e93c-d9f6-4f7f-9f3f-2ed508943d4b"), "Avatar 11", "https://api.dicebear.com/9.x/shapes/svg?seed=Zeynep" },
                    { new Guid("c6c1d684-6a48-440f-8295-28ea05aaea0c"), "Avatar 2", "https://api.dicebear.com/9.x/shapes/svg?seed=Olivia" },
                    { new Guid("f3c2c6ab-ed9d-47c4-bb8d-bf1346ba2d37"), "Avatar 12", "https://api.dicebear.com/9.x/shapes/svg?seed=Leo" }
                });

            migrationBuilder.InsertData(
                table: "DateFormats",
                columns: new[] { "Id", "FormatString", "Sample" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "DD/MM/YYYY", "31/12/2025" },
                    { new Guid("33333333-3333-3333-3333-333333333334"), "MM/DD/YYYY", "12/31/2025" },
                    { new Guid("33333333-3333-3333-3333-333333333335"), "YYYY-MM-DD", "2025-12-31" }
                });

            migrationBuilder.InsertData(
                table: "FontTypes",
                columns: new[] { "Id", "CssValue", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "'Open Sans', sans-serif", "Open Sans" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Georgia, serif", "Georgia" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "'Montserrat', sans-serif", "Montserrat" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "'Playfair Display', serif", "Playfair Display" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "CultureCode", "Name" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), "tr-TR", "Türkçe" },
                    { new Guid("22222222-2222-2222-2222-222222222223"), "en-US", "English" },
                    { new Guid("22222222-2222-2222-2222-222222222224"), "de-DE", "Deutsch" }
                });

            migrationBuilder.InsertData(
                table: "ThemeColors",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { new Guid("033f8fb5-270e-4621-a78a-b9377d66bb04"), "Indigo", "#6366F1" },
                    { new Guid("3de0037a-bfdd-4369-aca4-2efeb25d83ff"), "Sarı", "#F59E0B" },
                    { new Guid("4b1f0e59-b2e9-4320-8f90-4715a20f1b58"), "Turuncu", "#F97316" },
                    { new Guid("5034ce99-ce6c-4cb6-84a8-54026b359d78"), "Lime", "#84CC16" },
                    { new Guid("539847fb-39b3-496c-875d-76b8f5e632f3"), "Teal", "#14B8A6" },
                    { new Guid("641f071d-884f-416d-bb65-070a39c0fe8d"), "Kırmızı", "#EF4444" },
                    { new Guid("9dce0c0f-af8c-4137-84ba-be874c630e4b"), "Mavi", "#3B82F6" },
                    { new Guid("9f9a823f-2b81-4985-939a-bd22ac1d3c91"), "Orijinal", "#2ba86d" },
                    { new Guid("ab027dcb-da39-408e-9eae-44641a93b63e"), "Pembe", "#EC4899" },
                    { new Guid("afdb6015-7e65-4ee6-b1de-100a2a6d76d7"), "Gri", "#6B7280" },
                    { new Guid("cba6b11a-4151-4447-a546-4097d96ea327"), "Mor", "#8B5CF6" },
                    { new Guid("fa15e5fd-2656-418d-8351-3c07b444136f"), "Turkuaz", "#06B6D4" }
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "ActiveSessionLimit", "AvatarUrl", "DarkMode", "DateFormatId", "EmailNotifications", "EnableAnimations", "FontTypeId", "GoogleMapsApiKey", "LanguageId", "LocationTracking", "PushNotifications", "SessionTimeout", "SmsNotifications", "ThemeColor", "TwoFactorEnabled", "UserId" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), 0, "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton", false, new Guid("33333333-3333-3333-3333-333333333333"), true, true, new Guid("11111111-1111-1111-1111-111111111111"), "", new Guid("22222222-2222-2222-2222-222222222222"), false, true, 0, false, "#2ba86d", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_DateFormatId",
                table: "SystemSettings",
                column: "DateFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_FontTypeId",
                table: "SystemSettings",
                column: "FontTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_LanguageId",
                table: "SystemSettings",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "ThemeColors");

            migrationBuilder.DropTable(
                name: "DateFormats");

            migrationBuilder.DropTable(
                name: "FontTypes");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
