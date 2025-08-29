using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastxWebApi.Migrations
{
    /// <inheritdoc />
    public partial class FinalSeedData : Migration
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
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    BusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seats_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" },
                    { 3, "Bus Operator" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "BusId", "ContactNumber", "CreatedAt", "Email", "Gender", "HashKey", "IsDeleted", "Name", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, "CDM", null, "9876543210", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7472), "Nimal@fastx.com", "Male", new byte[] { 24, 67, 162, 153, 53, 215, 180, 76, 211, 103, 117, 137, 30, 135, 245, 116, 121, 142, 162, 89, 78, 164, 118, 35, 211, 129, 116, 223, 201, 14, 117, 9, 126, 109, 4, 184, 24, 225, 17, 115, 252, 81, 14, 77, 61, 107, 236, 241, 14, 89, 127, 122, 225, 61, 93, 238, 68, 72, 198, 5, 216, 186, 175, 172 }, false, "Nimalan", new byte[] { 199, 24, 152, 102, 183, 36, 12, 64, 44, 78, 4, 96, 155, 28, 39, 175, 150, 6, 9, 36, 152, 207, 250, 88, 47, 247, 48, 6, 25, 132, 4, 137 }, 1 },
                    { 2, "LA, california", null, "785423654", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7544), "GM@fastx.com", "Male", new byte[] { 167, 64, 160, 232, 217, 118, 151, 127, 167, 245, 17, 245, 183, 235, 213, 68, 54, 230, 2, 13, 80, 61, 70, 148, 111, 75, 193, 220, 125, 47, 16, 223, 214, 214, 199, 46, 102, 210, 203, 177, 43, 25, 2, 201, 230, 247, 241, 147, 253, 19, 245, 142, 207, 204, 228, 153, 63, 77, 198, 170, 177, 201, 27, 194 }, false, "Guru", new byte[] { 24, 110, 70, 161, 116, 202, 236, 143, 73, 66, 33, 184, 183, 50, 16, 1, 128, 165, 41, 157, 176, 131, 138, 151, 140, 173, 147, 31, 89, 232, 59, 67 }, 2 },
                    { 3, "Heaven block 1", null, "4552155741", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7610), "thordriver@fastx.com", "Male", new byte[] { 5, 254, 214, 212, 164, 112, 178, 141, 236, 180, 175, 23, 188, 198, 194, 87, 90, 137, 182, 248, 73, 9, 98, 2, 164, 206, 83, 143, 201, 121, 212, 50, 41, 214, 37, 106, 185, 38, 127, 35, 1, 227, 142, 159, 83, 97, 61, 93, 201, 233, 156, 36, 132, 211, 109, 188, 184, 221, 251, 191, 61, 100, 213, 199 }, false, "Driver Thor", new byte[] { 114, 86, 193, 207, 196, 231, 158, 245, 152, 243, 3, 71, 61, 66, 65, 109, 38, 198, 211, 237, 136, 53, 150, 85, 158, 95, 171, 125, 16, 145, 26, 194 }, 3 },
                    { 4, "Chennai", null, "7845213698", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7674), "Kumardriver@fast.com", "Male", new byte[] { 213, 217, 124, 188, 149, 254, 17, 210, 248, 89, 57, 215, 186, 102, 120, 193, 101, 208, 145, 27, 82, 170, 30, 3, 113, 114, 17, 225, 174, 207, 0, 113, 86, 219, 161, 10, 9, 220, 97, 115, 225, 250, 253, 252, 61, 236, 183, 73, 99, 141, 170, 190, 175, 239, 175, 90, 86, 208, 227, 37, 96, 115, 70, 13 }, false, "Driver Kumar", new byte[] { 108, 236, 148, 2, 178, 100, 160, 219, 161, 35, 99, 243, 50, 183, 131, 254, 226, 227, 3, 1, 97, 90, 138, 110, 39, 236, 69, 122, 246, 61, 47, 220 }, 3 },
                    { 5, "Chennai", null, "7458961235", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7736), "sivan@fast.com", "Male", new byte[] { 190, 104, 103, 70, 178, 102, 89, 54, 135, 207, 238, 234, 164, 197, 93, 99, 18, 49, 234, 202, 48, 50, 186, 189, 32, 239, 154, 237, 222, 2, 38, 23, 242, 243, 89, 106, 218, 54, 125, 243, 12, 32, 179, 216, 114, 39, 184, 136, 110, 190, 64, 52, 239, 7, 84, 225, 157, 48, 76, 115, 211, 108, 108, 244 }, false, "Sivan", new byte[] { 128, 17, 182, 84, 114, 103, 191, 249, 115, 37, 112, 61, 168, 15, 93, 225, 212, 218, 224, 179, 18, 97, 203, 218, 109, 207, 125, 53, 16, 61, 75, 134 }, 2 },
                    { 6, "CDM", null, "7854123695", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7800), "sankar@fastx.com", "Male", new byte[] { 105, 128, 148, 255, 132, 55, 26, 227, 163, 58, 78, 149, 41, 197, 34, 223, 216, 230, 109, 160, 130, 105, 148, 228, 6, 56, 40, 159, 220, 186, 224, 188, 136, 174, 53, 157, 40, 55, 112, 135, 169, 162, 91, 244, 196, 197, 51, 223, 33, 186, 205, 42, 34, 165, 31, 181, 186, 82, 93, 240, 99, 76, 221, 59 }, false, "sankar", new byte[] { 52, 49, 245, 99, 130, 59, 165, 69, 178, 176, 87, 117, 247, 55, 206, 223, 83, 253, 48, 74, 107, 44, 204, 230, 247, 191, 206, 63, 73, 218, 53, 140 }, 1 },
                    { 7, "Chepauk", null, "7436985212", new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(7942), "venkat@gmail.com", "Male", new byte[] { 172, 209, 232, 55, 174, 188, 5, 175, 103, 227, 15, 202, 159, 176, 77, 105, 185, 192, 193, 9, 215, 56, 106, 174, 81, 150, 69, 149, 202, 117, 179, 103, 187, 26, 3, 103, 75, 175, 50, 152, 1, 209, 26, 28, 59, 69, 239, 121, 74, 189, 227, 255, 16, 80, 129, 177, 54, 164, 54, 92, 90, 152, 153, 212 }, false, "Venkat", new byte[] { 172, 50, 97, 86, 92, 104, 28, 99, 181, 38, 63, 77, 153, 1, 78, 119, 163, 244, 15, 144, 110, 172, 11, 27, 112, 95, 71, 118, 221, 176, 133, 106 }, 1 }
                });

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "BusId", "Amenities", "BusName", "BusNumber", "BusOperatorId", "BusType", "IsDeleted", "Status", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "Wi-Fi, Water Bottle,TV", "Volvo", "KA-01-AB-8754", 3, "AC Sleeper", false, "Active", 40 },
                    { 2, "Wi-Fi", "BENZ", "TN-02-CD-9999", 3, "Non-AC", false, "Active", 45 },
                    { 3, "Wi-Fi", "Bharat", "TM-08-CM-2022", 3, "Non-AC", false, "Active", 25 },
                    { 4, "Wi-Fi", "ECR express", "SN-02-TR-2354", 4, "Non-AC", false, "Active", 18 },
                    { 5, "Wi-Fi", "Rameshwaram exp", "ER-02-NM-5421", 4, "AC", false, "Active", 20 },
                    { 6, "Wi-Fi", "Kovai exp", "YR-03-RF-8976", 4, "AC", false, "Active", 13 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "ArrivalTime", "BusId", "DepartureTime", "Destination", "Fare", "IsDeleted", "Origin", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 30, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 9, 27, 22, 0, 0, 0, DateTimeKind.Unspecified), "Chidambaram", 1500.00m, false, "Bangalore", "Active" },
                    { 2, new DateTime(2025, 9, 6, 7, 30, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 9, 4, 23, 30, 0, 0, DateTimeKind.Unspecified), "Cuddalore", 800.00m, false, "Chennai", "Active" },
                    { 3, new DateTime(2025, 9, 23, 7, 30, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2025, 9, 21, 23, 30, 0, 0, DateTimeKind.Unspecified), "CDM", 600.00m, false, "Chennai", "Active" },
                    { 4, new DateTime(2025, 9, 10, 7, 30, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2025, 9, 9, 23, 30, 0, 0, DateTimeKind.Unspecified), "Cuddalore", 800.00m, false, "Chennai", "Active" },
                    { 5, new DateTime(2025, 9, 13, 7, 30, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2025, 9, 11, 23, 30, 0, 0, DateTimeKind.Unspecified), "CDM", 1800.00m, false, "Rameshwaram", "Active" },
                    { 6, new DateTime(2025, 9, 18, 7, 30, 0, 0, DateTimeKind.Unspecified), 6, new DateTime(2025, 9, 15, 23, 30, 0, 0, DateTimeKind.Unspecified), "CDM", 2800.00m, false, "Kovai", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "IsDeleted", "NoOfSeats", "RouteId", "Status", "TotalFare", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 1, "Cancelled", 1500.00m, 1 },
                    { 2, new DateTime(2025, 8, 26, 11, 30, 0, 0, DateTimeKind.Unspecified), false, 2, 2, "Booked", 1600.00m, 1 },
                    { 3, new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 2, "Booked", 1600.00m, 1 },
                    { 4, new DateTime(2025, 8, 23, 13, 0, 0, 0, DateTimeKind.Unspecified), true, 3, 3, "Cancelled", 1800.00m, 6 },
                    { 5, new DateTime(2025, 8, 22, 14, 0, 0, 0, DateTimeKind.Unspecified), false, 1, 4, "Booked", 800.00m, 6 },
                    { 6, new DateTime(2025, 8, 22, 15, 0, 0, 0, DateTimeKind.Unspecified), true, 2, 5, "Cancelled", 3600.00m, 7 },
                    { 7, new DateTime(2025, 8, 26, 16, 0, 0, 0, DateTimeKind.Unspecified), false, 1, 6, "Booked", 2800.00m, 7 }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "SeatId", "BusId", "IsBooked", "IsDeleted", "RouteId", "SeatNumber", "Status" },
                values: new object[,]
                {
                    { 1, 1, false, false, 1, "A1", "Active" },
                    { 2, 1, false, false, 1, "A2", "Active" },
                    { 3, 1, false, false, 1, "A3", "Active" },
                    { 4, 1, false, false, 1, "A4", "Active" },
                    { 5, 1, false, false, 1, "A5", "Active" },
                    { 6, 1, false, false, 1, "A6", "Active" },
                    { 7, 1, false, false, 1, "A7", "Active" },
                    { 8, 1, false, false, 1, "A8", "Active" },
                    { 9, 1, false, false, 1, "A9", "Active" },
                    { 10, 1, false, false, 1, "A10", "Active" },
                    { 11, 2, false, false, 2, "B1", "Active" },
                    { 12, 2, false, false, 2, "B2", "Active" },
                    { 13, 2, false, false, 2, "B3", "Active" },
                    { 14, 2, false, false, 2, "B4", "Active" },
                    { 15, 2, false, false, 2, "B5", "Active" },
                    { 16, 2, false, false, 2, "B6", "Active" },
                    { 17, 2, false, false, 2, "B7", "Active" },
                    { 18, 2, false, false, 2, "B8", "Active" },
                    { 19, 2, false, false, 2, "B9", "Active" },
                    { 20, 2, false, false, 2, "B10", "Active" },
                    { 21, 3, false, false, 3, "C1", "Active" },
                    { 22, 3, false, false, 3, "C2", "Active" },
                    { 23, 3, false, false, 3, "C3", "Active" },
                    { 24, 3, false, false, 3, "C4", "Active" },
                    { 25, 3, false, false, 3, "C5", "Active" },
                    { 26, 3, false, false, 3, "C6", "Active" },
                    { 27, 3, false, false, 3, "C7", "Active" },
                    { 28, 3, false, false, 3, "C8", "Active" },
                    { 29, 3, false, false, 3, "C9", "Active" },
                    { 30, 3, false, false, 3, "C10", "Active" },
                    { 31, 4, false, false, 4, "D1", "Active" },
                    { 32, 4, false, false, 4, "D2", "Active" },
                    { 33, 4, false, false, 4, "D3", "Active" },
                    { 34, 4, false, false, 4, "D4", "Active" },
                    { 35, 4, false, false, 4, "D5", "Active" },
                    { 36, 4, false, false, 4, "D6", "Active" },
                    { 37, 4, false, false, 4, "D7", "Active" },
                    { 38, 4, false, false, 4, "D8", "Active" },
                    { 39, 4, false, false, 4, "D9", "Active" },
                    { 40, 4, false, false, 4, "D10", "Active" },
                    { 41, 5, false, false, 5, "E1", "Active" },
                    { 42, 5, false, false, 5, "E2", "Active" },
                    { 43, 5, false, false, 5, "E3", "Active" },
                    { 44, 5, false, false, 5, "E4", "Active" },
                    { 45, 5, false, false, 5, "E5", "Active" },
                    { 46, 5, false, false, 5, "E6", "Active" },
                    { 47, 5, false, false, 5, "E7", "Active" },
                    { 48, 5, false, false, 5, "E8", "Active" },
                    { 49, 5, false, false, 5, "E9", "Active" },
                    { 50, 5, false, false, 5, "E10", "Active" },
                    { 51, 6, false, false, 6, "F1", "Active" },
                    { 52, 6, false, false, 6, "F2", "Active" },
                    { 53, 6, false, false, 6, "F3", "Active" },
                    { 54, 6, false, false, 6, "F4", "Active" },
                    { 55, 6, false, false, 6, "F5", "Active" },
                    { 56, 6, false, false, 6, "F6", "Active" },
                    { 57, 6, false, false, 6, "F7", "Active" },
                    { 58, 6, false, false, 6, "F8", "Active" },
                    { 59, 6, false, false, 6, "F9", "Active" },
                    { 60, 6, false, false, 6, "F10", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "BookingId", "PaymentDate", "PaymentMethod", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1500.00m, 1, new DateTime(2025, 8, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Completed", 1 },
                    { 2, 1600.00m, 2, new DateTime(2025, 8, 9, 11, 30, 0, 0, DateTimeKind.Unspecified), "UPI", "Completed", 1 },
                    { 3, 1600.00m, 3, new DateTime(2025, 9, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Completed", 1 },
                    { 4, 1800.00m, 4, new DateTime(2025, 8, 23, 13, 0, 0, 0, DateTimeKind.Unspecified), "UPI", "Completed", 6 },
                    { 5, 800.00m, 5, new DateTime(2025, 8, 26, 14, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Completed", 6 },
                    { 6, 3600.00m, 6, new DateTime(2025, 9, 26, 15, 0, 0, 0, DateTimeKind.Unspecified), "Debit Card", "Completed", 7 },
                    { 7, 2800.00m, 7, new DateTime(2025, 9, 26, 16, 0, 0, 0, DateTimeKind.Unspecified), "UPI", "Completed", 7 }
                });

            migrationBuilder.InsertData(
                table: "Refunds",
                columns: new[] { "RefundId", "BookingId", "ProcessedBy", "RefundAmount", "RefundDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, null, 1500.00m, new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(8568), "Pending", 1 },
                    { 2, 4, 2, 1800.00m, new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(8574), "Pending", 6 },
                    { 3, 6, null, 3600.00m, new DateTime(2025, 8, 26, 10, 39, 17, 217, DateTimeKind.Local).AddTicks(8578), "Pending", 7 }
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
                name: "IX_Seats_BusId",
                table: "Seats",
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
