using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastxWebApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    BusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusOperatorId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.BusId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusId = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                    table.ForeignKey(
                        name: "FK_Routes_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoOfSeats = table.Column<int>(type: "int", nullable: false),
                    TotalFare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingsSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingsSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingsSeats_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingsSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "SeatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    RefundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_Refunds_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Users_ProcessedBy",
                        column: x => x.ProcessedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "BusId", "Amenities", "BusName", "BusNumber", "BusOperatorId", "BusType", "IsDeleted", "Status", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "Wi-Fi, Water Bottle,TV", "Volvo ", "KA-01-AB-8754", 0, "AC Sleeper", false, "Active", 40 },
                    { 2, "Wi-Fi", "BENZ  ", "TN-02-CD-9999", 0, "Non-AC", false, "Active", 45 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" },
                    { 3, "Bus Operator" }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "ArrivalTime", "BusId", "DepartureTime", "Destination", "Fare", "IsDeleted", "Origin", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 3, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 8, 2, 22, 0, 0, 0, DateTimeKind.Unspecified), "Chidambaram", 1500.00m, false, "Bangalore", "Active" },
                    { 2, new DateTime(2025, 8, 3, 7, 30, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 8, 2, 23, 30, 0, 0, DateTimeKind.Unspecified), "Cuddalore", 800.00m, false, "Chennai", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "BusId", "ContactNumber", "CreatedAt", "Email", "Gender", "HashKey", "IsDeleted", "Name", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, "CDM", null, "9876543210", new DateTime(2025, 8, 8, 20, 21, 54, 341, DateTimeKind.Local).AddTicks(4162), "Nimal@fastx.com", "Male", new byte[] { 167, 141, 120, 144, 63, 42, 42, 66, 17, 81, 170, 248, 100, 4, 19, 193, 131, 66, 165, 230, 168, 229, 216, 157, 49, 79, 11, 211, 133, 254, 191, 168, 42, 94, 47, 205, 172, 111, 16, 213, 22, 188, 106, 107, 79, 242, 122, 54, 27, 24, 199, 247, 4, 2, 159, 145, 178, 197, 183, 249, 24, 140, 114, 50 }, false, "Nimalan", new byte[] { 102, 127, 122, 215, 138, 228, 214, 47, 207, 240, 42, 69, 96, 83, 234, 54, 181, 10, 24, 11, 50, 252, 230, 148, 58, 79, 238, 172, 169, 105, 113, 91 }, 1 },
                    { 2, "LA,california", null, "785423654", new DateTime(2025, 8, 8, 20, 21, 54, 341, DateTimeKind.Local).AddTicks(4326), "GM@fastx.com", "Male", new byte[] { 168, 239, 149, 193, 24, 112, 114, 43, 109, 89, 39, 172, 26, 50, 155, 192, 4, 73, 208, 218, 235, 23, 30, 68, 22, 162, 95, 182, 144, 190, 10, 206, 210, 191, 102, 68, 122, 31, 22, 74, 60, 72, 57, 111, 164, 96, 94, 125, 91, 200, 144, 162, 102, 51, 50, 179, 150, 54, 65, 168, 96, 191, 207, 67 }, false, "Guru", new byte[] { 171, 140, 18, 37, 193, 160, 174, 200, 14, 86, 129, 85, 210, 17, 130, 46, 196, 173, 216, 85, 141, 168, 239, 99, 125, 155, 85, 194, 250, 23, 212, 108 }, 2 },
                    { 3, "Heaven block 1", 1, "4552155741", new DateTime(2025, 8, 8, 20, 21, 54, 341, DateTimeKind.Local).AddTicks(4566), "thordriver@fastx.com", "Male", new byte[] { 247, 197, 113, 199, 133, 86, 159, 220, 126, 90, 95, 7, 121, 68, 239, 120, 121, 166, 121, 31, 234, 160, 27, 157, 226, 74, 172, 192, 2, 125, 241, 89, 68, 183, 107, 96, 245, 176, 228, 58, 212, 146, 53, 151, 139, 111, 10, 114, 54, 85, 182, 158, 149, 28, 239, 85, 73, 80, 187, 213, 63, 7, 117, 232 }, false, "Driver Thor", new byte[] { 139, 255, 153, 51, 27, 211, 190, 52, 232, 98, 89, 106, 226, 26, 25, 82, 79, 152, 232, 190, 161, 176, 85, 7, 133, 91, 210, 225, 39, 149, 79, 111 }, 3 }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "SeatId", "IsBooked", "IsDeleted", "RouteId", "SeatNumber", "Status" },
                values: new object[,]
                {
                    { 1, false, false, 1, "A1", "Active" },
                    { 2, false, false, 1, "A2", "Active" },
                    { 3, false, false, 1, "B1", "Active" },
                    { 4, false, false, 2, "C1", "Active" },
                    { 5, false, false, 2, "C2", "Active" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RouteId",
                table: "Bookings",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingsSeats_BookingId",
                table: "BookingsSeats",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingsSeats_SeatId",
                table: "BookingsSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_BookingId",
                table: "Refunds",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_ProcessedBy",
                table: "Refunds",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_UserId",
                table: "Refunds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_BusId",
                table: "Routes",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_RouteId",
                table: "Seats",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BusId",
                table: "Users",
                column: "BusId",
                unique: true,
                filter: "[BusId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingsSeats");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
