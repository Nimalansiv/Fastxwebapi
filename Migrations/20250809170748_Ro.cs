using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastxWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Ro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    BusOperatorId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.BusId);
                    table.ForeignKey(
                        name: "FK_Buses_Users_BusOperatorId",
                        column: x => x.BusOperatorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "BusId", "Amenities", "BusName", "BusNumber", "BusOperatorId", "BusType", "IsDeleted", "Status", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "Wi-Fi, Water Bottle,TV", "Volvo", "KA-01-AB-8754", null, "AC Sleeper", false, "Active", 40 },
                    { 2, "Wi-Fi", "BENZ", "TN-02-CD-9999", null, "Non-AC", false, "Active", 45 }
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
                    { 1, "CDM", null, "9876543210", new DateTime(2025, 8, 9, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(8969), "Nimal@fastx.com", "Male", new byte[] { 217, 197, 204, 209, 254, 114, 74, 182, 104, 164, 250, 113, 152, 66, 171, 99, 26, 199, 70, 223, 129, 102, 185, 137, 148, 100, 143, 77, 159, 127, 129, 67, 227, 245, 10, 250, 94, 138, 203, 253, 25, 94, 45, 144, 36, 160, 114, 193, 220, 130, 228, 248, 148, 139, 83, 166, 25, 33, 115, 63, 78, 12, 3, 157 }, false, "Nimalan", new byte[] { 157, 84, 42, 68, 91, 49, 212, 41, 144, 150, 251, 81, 124, 247, 137, 52, 51, 96, 96, 147, 155, 164, 209, 1, 157, 147, 61, 118, 167, 187, 109, 77 }, 1 },
                    { 2, "LA, california", null, "785423654", new DateTime(2025, 8, 9, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(9009), "GM@fastx.com", "Male", new byte[] { 247, 183, 243, 104, 230, 167, 98, 210, 61, 65, 107, 79, 247, 41, 147, 221, 116, 181, 0, 7, 157, 6, 151, 139, 196, 27, 57, 107, 104, 74, 243, 82, 146, 77, 201, 230, 18, 191, 108, 54, 31, 138, 3, 134, 24, 247, 242, 104, 121, 86, 162, 227, 204, 222, 13, 109, 164, 93, 242, 165, 195, 111, 245, 245 }, false, "Guru", new byte[] { 119, 109, 90, 71, 35, 232, 249, 8, 166, 176, 104, 104, 224, 111, 82, 14, 128, 25, 223, 69, 142, 207, 252, 166, 247, 97, 241, 18, 105, 166, 2, 62 }, 2 },
                    { 3, "Heaven block 1", 1, "4552155741", new DateTime(2025, 8, 9, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(9104), "thordriver@fastx.com", "Male", new byte[] { 217, 116, 147, 42, 12, 188, 141, 255, 83, 47, 229, 0, 110, 185, 138, 239, 134, 49, 199, 241, 205, 250, 198, 87, 150, 235, 204, 190, 235, 13, 167, 71, 11, 200, 221, 130, 110, 13, 236, 152, 86, 175, 181, 212, 67, 234, 147, 241, 102, 133, 153, 152, 103, 241, 246, 164, 180, 10, 48, 198, 98, 9, 134, 87 }, false, "Driver Thor", new byte[] { 94, 60, 11, 97, 177, 0, 150, 103, 250, 190, 130, 190, 153, 67, 156, 131, 178, 245, 82, 229, 44, 56, 235, 24, 16, 208, 38, 27, 57, 176, 124, 200 }, 3 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "IsDeleted", "NoOfSeats", "RouteId", "Status", "TotalFare", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 8, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(9343), true, 1, 1, "Cancelled", 1500.00m, 1 },
                    { 2, new DateTime(2025, 8, 8, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(9355), false, 2, 2, "Booked", 1600.00m, 1 }
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

            migrationBuilder.InsertData(
                table: "Refunds",
                columns: new[] { "RefundId", "BookingId", "ProcessedBy", "RefundAmount", "RefundDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, null, 1500.00m, new DateTime(2025, 8, 9, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(9390), "Pending", 1 },
                    { 2, 2, 2, 800.00m, new DateTime(2025, 8, 9, 22, 37, 47, 706, DateTimeKind.Local).AddTicks(9395), "Approved", 1 }
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
                name: "IX_Buses_BusOperatorId",
                table: "Buses",
                column: "BusOperatorId");

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
                name: "Buses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
