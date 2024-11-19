using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StarRating",
                table: "Comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "b2e7f01a-8b84-4cc3-bade-49d10a4cf3e1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "a93913b2-39fe-4bbe-bab9-97fdbdf12059");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "4efd42b2-5f4a-4deb-8c1c-1ee52e1bcc0e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa1a6f21-19d5-4eca-aa2c-481466b511c9", "AQAAAAIAAYagAAAAEKnJ9AWzs8uYMPR9iTZN7buYZcft+sitCOZXLyhAAtGPniZymPIRCdDSwOJvi7/lHA==", "d3601859-2d27-4874-a00d-2fb95d6aedd7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "StarRating",
                table: "Comments",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "7f6ece9a-64e8-43af-b81a-b1fe5d7dec9e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "fd12bb21-fe51-4f63-9490-7c80d7cab049");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "e5597155-52ee-44dd-bca7-b76c89ac0544");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1292babc-7b64-40fd-9f83-16ce10414e48", "AQAAAAIAAYagAAAAECsLkorGnsIFF3H47cUUJQqHcrKtWrmLOJPBbbEjMrv6haFGVIkvjaZrNBsIy5DCig==", "e2850ead-975f-4f1d-82c7-3a38a73c107b" });
        }
    }
}
