using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastxWebApi.Migrations
{
    /// <inheritdoc />
    public partial class wow : Migration
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
                    { 1, "CDM", null, "9876543210", new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7465), "Nimal@fastx.com", "Male", new byte[] { 199, 173, 25, 97, 228, 126, 62, 7, 112, 66, 42, 41, 156, 219, 14, 230, 144, 205, 73, 246, 42, 233, 110, 49, 18, 12, 165, 169, 101, 153, 191, 147, 180, 249, 239, 196, 34, 211, 118, 219, 58, 87, 51, 246, 78, 210, 192, 22, 174, 142, 241, 93, 20, 14, 145, 25, 20, 110, 137, 224, 182, 19, 80, 24 }, false, "Nimalan", new byte[] { 197, 94, 28, 25, 61, 13, 212, 61, 23, 69, 101, 187, 246, 23, 84, 172, 84, 184, 12, 200, 119, 137, 49, 83, 15, 141, 72, 214, 118, 80, 227, 112 }, 1 },
                    { 2, "LA, california", null, "785423654", new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7516), "GM@fastx.com", "Male", new byte[] { 177, 81, 123, 51, 170, 29, 246, 210, 222, 236, 164, 73, 107, 84, 228, 95, 244, 222, 252, 142, 229, 0, 245, 4, 2, 30, 116, 23, 39, 68, 58, 149, 42, 197, 198, 143, 58, 70, 124, 95, 179, 219, 106, 164, 225, 165, 15, 42, 116, 98, 239, 36, 195, 81, 182, 131, 125, 198, 5, 42, 229, 187, 22, 252 }, false, "Guru", new byte[] { 38, 78, 97, 221, 176, 186, 244, 10, 142, 84, 253, 94, 142, 53, 137, 106, 64, 4, 128, 29, 102, 148, 41, 225, 44, 242, 62, 177, 58, 57, 124, 61 }, 2 },
                    { 3, "Heaven block 1", 1, "4552155741", new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7560), "thordriver@fastx.com", "Male", new byte[] { 147, 52, 161, 130, 218, 232, 37, 226, 174, 62, 97, 155, 211, 138, 122, 188, 2, 207, 159, 72, 109, 220, 49, 108, 225, 107, 148, 101, 140, 123, 153, 1, 130, 131, 161, 94, 90, 55, 158, 136, 8, 18, 31, 239, 139, 120, 203, 171, 208, 188, 168, 168, 199, 66, 205, 224, 69, 129, 157, 155, 174, 1, 47, 135 }, false, "Driver Thor", new byte[] { 155, 161, 95, 50, 108, 130, 168, 199, 13, 217, 146, 36, 22, 142, 168, 149, 183, 44, 64, 65, 82, 210, 82, 55, 31, 23, 250, 249, 160, 203, 109, 245 }, 3 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "IsDeleted", "NoOfSeats", "RouteId", "Status", "TotalFare", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 11, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7732), true, 1, 1, "Cancelled", 1500.00m, 1 },
                    { 2, new DateTime(2025, 8, 11, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7743), false, 2, 2, "Booked", 1600.00m, 1 },
                    { 3, new DateTime(2025, 8, 11, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7747), false, 2, 2, "Booked", 1600.00m, 1 }
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
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "BookingId", "PaymentDate", "PaymentMethod", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1500.00m, 1, new DateTime(2025, 8, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Completed", 1 },
                    { 2, 800.00m, 2, new DateTime(2025, 8, 9, 11, 30, 0, 0, DateTimeKind.Unspecified), "UPI", "Completed", 1 }
                });

            migrationBuilder.InsertData(
                table: "Refunds",
                columns: new[] { "RefundId", "BookingId", "ProcessedBy", "RefundAmount", "RefundDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, null, 1500.00m, new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7787), "Pending", 1 },
                    { 2, 2, 2, 800.00m, new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7793), "Approved", 1 },
                    { 3, 3, null, 900.00m, new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7797), "Pending", 1 }
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
