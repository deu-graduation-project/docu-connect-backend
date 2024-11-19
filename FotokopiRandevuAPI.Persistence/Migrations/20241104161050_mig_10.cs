using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletedCode",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileCode",
                table: "CopyFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "d56ae976-5561-40eb-99eb-eba8d956aeae");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "053c665d-2ee3-41b5-a3bd-49fcbf4c6957");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "463932d2-0508-4518-b29c-29fc518152c2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f001d47-decd-4f6c-9870-71669c6645ab", "AQAAAAIAAYagAAAAEKCzdl9oMQs6OC/22sWnwiLFZVjsKvLyr0mgQJMvSF43/Gpe7/0A+C2/ufSR5xKZ8g==", "18fce0de-48b3-4250-a4c6-967fc8e2f54b" });

            migrationBuilder.CreateIndex(
                name: "IX_CopyFiles_FileCode",
                table: "CopyFiles",
                column: "FileCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CopyFiles_FileCode",
                table: "CopyFiles");

            migrationBuilder.DropColumn(
                name: "CompletedCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FileCode",
                table: "CopyFiles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "85c8b4af-11b3-4641-89a6-0905f09c6740");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "2bb4954c-34db-4c79-8e4f-c5c355b0b0e3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "8f4d76fd-5e21-4403-ac00-e3898429f321");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d48c0578-3781-41d5-9a10-565430422fb6", "AQAAAAIAAYagAAAAEMCO4hcsp3t1QDgO/Enq3gzV8NH7gbTuoGRRYBym/tiip2tiXndld4TYmS8HGkH/0A==", "876e9bce-b194-4683-b000-696b48a9a2b3" });
        }
    }
}
