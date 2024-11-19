using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "2313df7f-691f-4ec1-a257-213d32c99cb6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "24805b60-9d55-4ea2-b95f-0600387c8f82");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "f47042f9-7f0c-420c-b0c1-0ed5bb13a648");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0defe50-0dad-40e5-9b6c-59e0766b426f", "AQAAAAIAAYagAAAAEAXCsNnP+fCK+JTqIPidkbkF291NrjB6LmxoXCmZJtDqe96b2DjqZCD4MjvqywJIwA==", "9ad709e2-4f6e-44e5-b885-046867f6cbae" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "3886aa98-698a-4551-b306-9433a7934767");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "e6795851-79d4-4388-ba2f-5761f6e7c2a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "0f38a681-2109-4c05-9520-77decfe00567");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d19ab4b9-ecb6-4333-8efc-44d6045025bb", "AQAAAAIAAYagAAAAEMIDjYXU8/RKAHbca1uCeptNAdiCKPsq3R8olkMCSKiNgrXHTC1PWegpTAJXcwmcHQ==", "46434e34-f77b-4589-955c-09cc1db195b1" });
        }
    }
}
