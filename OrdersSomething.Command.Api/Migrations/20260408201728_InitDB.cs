using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrdersSomething.Command.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsListening = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastHeartbeat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Properties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceEvents_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_DeviceEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "DeviceEvents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Firstname", "Surname" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999991"), "John", "Doe" },
                    { new Guid("99999999-9999-9999-9999-999999999992"), "Jane", "Smith" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "CreatedAt", "Description", "IsDeleted", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Mazury 1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Domek nad jeziorem", false, "Dom Letniskowy", new Guid("99999999-9999-9999-9999-999999999991") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Rynek 15", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Apartament w centrum", false, "Mieszkanie Kraków", new Guid("99999999-9999-9999-9999-999999999991") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Bieszczady 10", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teren pod lasem", false, "Działka Rekreacyjna", new Guid("99999999-9999-9999-9999-999999999991") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Al. Jerozolimskie 100", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Główne biuro", false, "Biuro Warszawa", new Guid("99999999-9999-9999-9999-999999999992") }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "CreatedAt", "IsListening", "LastHeartbeat", "Name", "PropertiesId", "Status", "Type" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kamera Wejście", new Guid("11111111-1111-1111-1111-111111111111"), "active", "camera" },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Czujnik Ruchu Salon", new Guid("11111111-1111-1111-1111-111111111111"), "active", "sensor" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "CreatedAt", "LastHeartbeat", "Name", "PropertiesId", "Status", "Type" },
                values: new object[] { new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mikrofon Sypialnia", new Guid("22222222-2222-2222-2222-222222222222"), "inactive", "microphone" });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "CreatedAt", "IsListening", "LastHeartbeat", "Name", "PropertiesId", "Status", "Type" },
                values: new object[] { new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kamera Recepcja", new Guid("44444444-4444-4444-4444-444444444444"), "active", "camera" });

            migrationBuilder.InsertData(
                table: "DeviceEvents",
                columns: new[] { "Id", "CreatedAt", "Description", "DeviceId", "EventType", "ImageUrl", "Severity" },
                values: new object[,]
                {
                    { new Guid("e1111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wykryto ruch pod drzwiami", new Guid("a1111111-1111-1111-1111-111111111111"), "motion", null, 2 },
                    { new Guid("e2222222-2222-2222-2222-222222222222"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Zapisano obraz twarzy", new Guid("a1111111-1111-1111-1111-111111111111"), "capture", "https://example.com/img1.jpg", 1 }
                });

            migrationBuilder.InsertData(
                table: "UserNotifications",
                columns: new[] { "Id", "Body", "EventId", "SentAt", "Title", "UserId" },
                values: new object[] { new Guid("d1111111-1111-1111-1111-111111111111"), "Ktoś jest pod Twoimi drzwiami.", new Guid("e1111111-1111-1111-1111-111111111111"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruch wykryty!", new Guid("99999999-9999-9999-9999-999999999991") });

            migrationBuilder.InsertData(
                table: "UserNotifications",
                columns: new[] { "Id", "Body", "EventId", "IsRead", "SentAt", "Title", "UserId" },
                values: new object[] { new Guid("d2222222-2222-2222-2222-222222222222"), "Nowe zdjęcie z kamery wejściowej.", new Guid("e2222222-2222-2222-2222-222222222222"), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Zapisano obraz", new Guid("99999999-9999-9999-9999-999999999991") });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceEvents_DeviceId",
                table: "DeviceEvents",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_PropertiesId",
                table: "Devices",
                column: "PropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UserId",
                table: "Properties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_EventId",
                table: "UserNotifications",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "DeviceEvents");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
