using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastxWebApi.Migrations
{
    /// <inheritdoc />
    public partial class @do : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 24, 40, 483, DateTimeKind.Local).AddTicks(352), new byte[] { 250, 38, 109, 221, 173, 107, 91, 96, 155, 190, 244, 162, 94, 32, 60, 122, 126, 142, 66, 168, 53, 248, 11, 243, 91, 177, 59, 235, 219, 54, 251, 61, 248, 218, 17, 84, 246, 109, 39, 218, 179, 191, 192, 172, 197, 67, 86, 23, 89, 23, 4, 12, 145, 202, 107, 168, 56, 11, 28, 85, 179, 54, 209, 231 }, new byte[] { 90, 161, 88, 65, 227, 109, 52, 101, 9, 142, 24, 69, 104, 100, 246, 77, 96, 16, 146, 103, 144, 78, 168, 177, 31, 76, 21, 218, 213, 74, 230, 54 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 24, 40, 483, DateTimeKind.Local).AddTicks(448), new byte[] { 50, 232, 167, 25, 222, 78, 4, 175, 91, 37, 118, 38, 103, 69, 158, 153, 9, 190, 70, 136, 242, 44, 252, 78, 6, 44, 29, 115, 236, 180, 244, 108, 240, 204, 164, 192, 166, 123, 171, 208, 161, 42, 185, 147, 34, 85, 28, 109, 100, 187, 135, 146, 1, 185, 1, 153, 65, 188, 250, 40, 223, 76, 194, 175 }, new byte[] { 106, 68, 228, 29, 244, 209, 113, 182, 12, 42, 75, 242, 252, 205, 207, 74, 212, 75, 160, 177, 26, 3, 39, 118, 75, 246, 214, 123, 80, 224, 90, 115 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 24, 40, 483, DateTimeKind.Local).AddTicks(556), new byte[] { 145, 11, 58, 154, 141, 56, 109, 34, 44, 244, 51, 128, 51, 243, 224, 225, 228, 176, 74, 13, 173, 41, 188, 172, 41, 230, 85, 28, 207, 90, 239, 215, 74, 215, 245, 114, 157, 148, 151, 87, 88, 147, 18, 59, 216, 168, 109, 25, 103, 233, 55, 123, 156, 6, 176, 34, 169, 187, 254, 97, 96, 131, 37, 107 }, new byte[] { 242, 163, 242, 87, 223, 177, 81, 67, 110, 137, 252, 113, 245, 131, 29, 254, 142, 48, 192, 247, 12, 70, 240, 180, 225, 108, 206, 116, 109, 102, 74, 144 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 21, 54, 341, DateTimeKind.Local).AddTicks(4162), new byte[] { 167, 141, 120, 144, 63, 42, 42, 66, 17, 81, 170, 248, 100, 4, 19, 193, 131, 66, 165, 230, 168, 229, 216, 157, 49, 79, 11, 211, 133, 254, 191, 168, 42, 94, 47, 205, 172, 111, 16, 213, 22, 188, 106, 107, 79, 242, 122, 54, 27, 24, 199, 247, 4, 2, 159, 145, 178, 197, 183, 249, 24, 140, 114, 50 }, new byte[] { 102, 127, 122, 215, 138, 228, 214, 47, 207, 240, 42, 69, 96, 83, 234, 54, 181, 10, 24, 11, 50, 252, 230, 148, 58, 79, 238, 172, 169, 105, 113, 91 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 21, 54, 341, DateTimeKind.Local).AddTicks(4326), new byte[] { 168, 239, 149, 193, 24, 112, 114, 43, 109, 89, 39, 172, 26, 50, 155, 192, 4, 73, 208, 218, 235, 23, 30, 68, 22, 162, 95, 182, 144, 190, 10, 206, 210, 191, 102, 68, 122, 31, 22, 74, 60, 72, 57, 111, 164, 96, 94, 125, 91, 200, 144, 162, 102, 51, 50, 179, 150, 54, 65, 168, 96, 191, 207, 67 }, new byte[] { 171, 140, 18, 37, 193, 160, 174, 200, 14, 86, 129, 85, 210, 17, 130, 46, 196, 173, 216, 85, 141, 168, 239, 99, 125, 155, 85, 194, 250, 23, 212, 108 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "HashKey", "Password" },
                values: new object[] { new DateTime(2025, 8, 8, 20, 21, 54, 341, DateTimeKind.Local).AddTicks(4566), new byte[] { 247, 197, 113, 199, 133, 86, 159, 220, 126, 90, 95, 7, 121, 68, 239, 120, 121, 166, 121, 31, 234, 160, 27, 157, 226, 74, 172, 192, 2, 125, 241, 89, 68, 183, 107, 96, 245, 176, 228, 58, 212, 146, 53, 151, 139, 111, 10, 114, 54, 85, 182, 158, 149, 28, 239, 85, 73, 80, 187, 213, 63, 7, 117, 232 }, new byte[] { 139, 255, 153, 51, 27, 211, 190, 52, 232, 98, 89, 106, 226, 26, 25, 82, 79, 152, 232, 190, 161, 176, 85, 7, 133, 91, 210, 225, 39, 149, 79, 111 } });
        }
    }
}
