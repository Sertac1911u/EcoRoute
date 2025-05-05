using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoRoute.Settings.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
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
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DarkMode = table.Column<bool>(type: "bit", nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FontSize = table.Column<int>(type: "int", nullable: false),
                    EnableAnimations = table.Column<bool>(type: "bit", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    SmsNotifications = table.Column<bool>(type: "bit", nullable: false),
                    PushNotifications = table.Column<bool>(type: "bit", nullable: false),
                    GoogleMapsApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
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

            migrationBuilder.InsertData(
                table: "Avatars",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { new Guid("25e28709-b062-4ccf-8f4a-a3f0e1c8035a"), "Avatar 2", "https://api.dicebear.com/9.x/adventurer/svg?seed=Avery" },
                    { new Guid("5f9fe6f1-a0d0-49be-ad79-f3164841a31f"), "Avatar 3", "https://api.dicebear.com/9.x/adventurer/svg?seed=Jude" },
                    { new Guid("925370be-4902-48b9-a3ea-8ae41ca19232"), "Avatar 4", "https://api.dicebear.com/9.x/adventurer/svg?seed=Jack" },
                    { new Guid("ab7890a6-0e94-4dc7-8a3f-74f85f80353d"), "Avatar 6", "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton" },
                    { new Guid("dcd3c63f-48c4-4d31-a416-0c9b969b60dd"), "Avatar 1", "https://api.dicebear.com/9.x/adventurer/svg?seed=Luis" },
                    { new Guid("e6898c3d-b58e-4da5-9114-7cb6c76cf790"), "Avatar 5", "https://api.dicebear.com/9.x/adventurer/svg?seed=Jocelyn" }
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "AvatarUrl", "DarkMode", "EmailNotifications", "EnableAnimations", "FontSize", "GoogleMapsApiKey", "PushNotifications", "SmsNotifications", "ThemeColor", "UserId" },
                values: new object[] { new Guid("ae0443f1-917c-44db-9ecb-265f03c5b4ed"), "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton", false, true, true, 14, "", true, false, "#2ba86d", null });

            migrationBuilder.InsertData(
                table: "ThemeColors",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { new Guid("04a59f81-2372-413b-8760-2150acdeba1e"), "Pembe", "#EC4899" },
                    { new Guid("11b56dde-e8e5-4e87-b43b-4c58acf8b996"), "Mor", "#8B5CF6" },
                    { new Guid("1dccb679-b3b3-4039-be89-58259c01df5e"), "Indigo", "#6366F1" },
                    { new Guid("2d356529-28bb-45bb-80b5-b25da1073d6e"), "Orijinal", "#2ba86d" },
                    { new Guid("5408af70-b683-4c54-83ef-e7cec5c3d18a"), "Kırmızı", "#EF4444" },
                    { new Guid("7f5c0536-f408-4f83-b19d-a76892df23ea"), "Turkuaz", "#06B6D4" },
                    { new Guid("c7a4da28-f770-478d-8ae0-84563fa558ea"), "Sarı", "#F59E0B" },
                    { new Guid("d3aefb83-49b3-4497-835b-919b5754cebe"), "Mavi", "#3B82F6" },
                    { new Guid("f649b485-4de1-4197-9212-b077cd729789"), "Gri", "#6B7280" },
                    { new Guid("fa62d005-3ad7-4919-a2c2-e3f058405160"), "Teal", "#14B8A6" },
                    { new Guid("fac9cdaf-beeb-49a4-9efb-84395499096b"), "Turuncu", "#F97316" },
                    { new Guid("fef03528-4ac0-49a6-adca-7ad7888dcd6a"), "Lime", "#84CC16" }
                });
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
        }
    }
}
