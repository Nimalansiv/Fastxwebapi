using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastxWebApi.Migrations
{
    /// <inheritdoc />
    public partial class @in : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                column: "BookingDate",
                value: new DateTime(2025, 8, 11, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                column: "BookingDate",
                value: new DateTime(2025, 8, 11, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6787));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                column: "BookingDate",
                value: new DateTime(2025, 8, 11, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6792));

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "BusId", "Amenities", "BusName", "BusNumber", "BusOperatorId", "BusType", "IsDeleted", "Status", "TotalSeats" },
                values: new object[] { 3, "Wi-Fi", "Bharat", "TM-08-CM-2022", null, "Non-AC", false, "Active", 25 });

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 1,
                column: "RefundDate",
                value: new DateTime(2025, 8, 12, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6858));

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 2,
                column: "RefundDate",
                value: new DateTime(2025, 8, 12, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6865));

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 3,
                column: "RefundDate",
                value: new DateTime(2025, 8, 12, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6869));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 12, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6154), new byte[] { 103, 21, 76, 36, 126, 231, 195, 124, 240, 61, 171, 141, 202, 233, 240, 151, 134, 160, 3, 85, 0, 75, 182, 100, 240, 44, 198, 250, 101, 26, 246, 143, 52, 112, 204, 218, 132, 109, 75, 116, 253, 181, 80, 26, 208, 223, 42, 125, 143, 115, 105, 3, 214, 254, 61, 142, 49, 6, 67, 33, 146, 202, 223, 150 }, new byte[] { 81, 231, 142, 13, 129, 151, 247, 201, 10, 157, 143, 107, 94, 229, 67, 64, 130, 52, 220, 171, 39, 80, 152, 80, 47, 176, 18, 250, 13, 87, 202, 38 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 12, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6235), new byte[] { 162, 195, 184, 207, 214, 249, 110, 238, 34, 232, 80, 112, 215, 186, 204, 27, 48, 36, 87, 128, 32, 175, 234, 36, 162, 242, 74, 45, 149, 210, 200, 44, 13, 155, 64, 203, 240, 251, 208, 201, 0, 224, 144, 50, 192, 248, 170, 178, 47, 215, 177, 10, 26, 28, 115, 206, 17, 24, 196, 176, 76, 102, 92, 143 }, new byte[] { 159, 100, 222, 197, 78, 140, 211, 2, 159, 248, 242, 93, 64, 64, 185, 158, 96, 120, 252, 170, 41, 76, 65, 172, 126, 136, 242, 229, 111, 192, 41, 150 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 12, 15, 1, 40, 495, DateTimeKind.Local).AddTicks(6302), new byte[] { 62, 255, 224, 204, 73, 0, 94, 93, 237, 21, 241, 82, 213, 140, 101, 84, 8, 187, 54, 65, 171, 107, 65, 72, 169, 15, 149, 9, 193, 231, 24, 93, 238, 64, 178, 106, 151, 220, 127, 12, 142, 38, 165, 250, 67, 168, 254, 174, 154, 244, 221, 231, 57, 40, 163, 231, 60, 100, 117, 10, 88, 116, 252, 14 }, new byte[] { 162, 140, 225, 122, 158, 51, 145, 115, 116, 124, 49, 87, 168, 65, 235, 151, 219, 18, 57, 140, 129, 67, 50, 4, 125, 33, 28, 217, 29, 43, 61, 138 } });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "ArrivalTime", "BusId", "DepartureTime", "Destination", "Fare", "IsDeleted", "Origin", "Status" },
                values: new object[] { 3, new DateTime(2025, 8, 3, 7, 30, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2025, 8, 2, 23, 30, 0, 0, DateTimeKind.Unspecified), "CDM", 600.00m, false, "Chennai", "Active" });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "SeatId", "IsBooked", "IsDeleted", "RouteId", "SeatNumber", "Status" },
                values: new object[,]
                {
                    { 6, false, false, 3, "D1", "Active" },
                    { 7, false, false, 3, "D2", "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "SeatId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "SeatId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumn: "RouteId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Buses",
                keyColumn: "BusId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                column: "BookingDate",
                value: new DateTime(2025, 8, 11, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                column: "BookingDate",
                value: new DateTime(2025, 8, 11, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                column: "BookingDate",
                value: new DateTime(2025, 8, 11, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7747));

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 1,
                column: "RefundDate",
                value: new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7787));

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 2,
                column: "RefundDate",
                value: new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7793));

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 3,
                column: "RefundDate",
                value: new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7797));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7465), new byte[] { 199, 173, 25, 97, 228, 126, 62, 7, 112, 66, 42, 41, 156, 219, 14, 230, 144, 205, 73, 246, 42, 233, 110, 49, 18, 12, 165, 169, 101, 153, 191, 147, 180, 249, 239, 196, 34, 211, 118, 219, 58, 87, 51, 246, 78, 210, 192, 22, 174, 142, 241, 93, 20, 14, 145, 25, 20, 110, 137, 224, 182, 19, 80, 24 }, new byte[] { 197, 94, 28, 25, 61, 13, 212, 61, 23, 69, 101, 187, 246, 23, 84, 172, 84, 184, 12, 200, 119, 137, 49, 83, 15, 141, 72, 214, 118, 80, 227, 112 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7516), new byte[] { 177, 81, 123, 51, 170, 29, 246, 210, 222, 236, 164, 73, 107, 84, 228, 95, 244, 222, 252, 142, 229, 0, 245, 4, 2, 30, 116, 23, 39, 68, 58, 149, 42, 197, 198, 143, 58, 70, 124, 95, 179, 219, 106, 164, 225, 165, 15, 42, 116, 98, 239, 36, 195, 81, 182, 131, 125, 198, 5, 42, 229, 187, 22, 252 }, new byte[] { 38, 78, 97, 221, 176, 186, 244, 10, 142, 84, 253, 94, 142, 53, 137, 106, 64, 4, 128, 29, 102, 148, 41, 225, 44, 242, 62, 177, 58, 57, 124, 61 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 12, 11, 39, 43, 714, DateTimeKind.Local).AddTicks(7560), new byte[] { 147, 52, 161, 130, 218, 232, 37, 226, 174, 62, 97, 155, 211, 138, 122, 188, 2, 207, 159, 72, 109, 220, 49, 108, 225, 107, 148, 101, 140, 123, 153, 1, 130, 131, 161, 94, 90, 55, 158, 136, 8, 18, 31, 239, 139, 120, 203, 171, 208, 188, 168, 168, 199, 66, 205, 224, 69, 129, 157, 155, 174, 1, 47, 135 }, new byte[] { 155, 161, 95, 50, 108, 130, 168, 199, 13, 217, 146, 36, 22, 142, 168, 149, 183, 44, 64, 65, 82, 210, 82, 55, 31, 23, 250, 249, 160, 203, 109, 245 } });
        }
    }
}
