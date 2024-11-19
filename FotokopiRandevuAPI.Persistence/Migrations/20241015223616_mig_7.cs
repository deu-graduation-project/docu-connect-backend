using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopyFile_Orders_OrderId",
                table: "CopyFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CopyFile",
                table: "CopyFile");

            migrationBuilder.RenameTable(
                name: "CopyFile",
                newName: "CopyFiles");

            migrationBuilder.RenameIndex(
                name: "IX_CopyFile_OrderId",
                table: "CopyFiles",
                newName: "IX_CopyFiles_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CopyFiles",
                table: "CopyFiles",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CopyFiles_Orders_OrderId",
                table: "CopyFiles",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopyFiles_Orders_OrderId",
                table: "CopyFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CopyFiles",
                table: "CopyFiles");

            migrationBuilder.RenameTable(
                name: "CopyFiles",
                newName: "CopyFile");

            migrationBuilder.RenameIndex(
                name: "IX_CopyFiles_OrderId",
                table: "CopyFile",
                newName: "IX_CopyFile_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CopyFile",
                table: "CopyFile",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "011f228e-a2e2-421c-ab93-41f87e337cfb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "4beb4e18-d380-4a01-a0fe-be3a686ffd39");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "0a403444-75ae-421d-9a2e-89e3cfce3edd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0151b16a-6764-45dd-930d-d4476ca2e6b0", "AQAAAAIAAYagAAAAECwrkw4nGsb+gzKHIYy8yoMSUaheO7kx8Tm95kUEnKDcGuODJryRToSi6WqevA3YdQ==", "abe91395-ab0f-4934-90e2-188c64c011da" });

            migrationBuilder.AddForeignKey(
                name: "FK_CopyFile_Orders_OrderId",
                table: "CopyFile",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
