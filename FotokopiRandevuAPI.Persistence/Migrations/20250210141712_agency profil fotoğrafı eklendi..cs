using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class agencyprofilfotoğrafıeklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "4c072aec-670c-43a3-ab5d-ec663fbaeba7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "a767c3ac-4d12-41d9-8cff-0d1ec67332db");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "c6b9a167-11da-43b9-b77c-15a959aa25dd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab09f45c-0cae-4003-9d71-11b4eb36a200", "AQAAAAIAAYagAAAAEFWESkhstrz8S+YnAR9qM6siuyFTrk1k8M+xf+y/wq7MQGZaKujRESHcLxewcKdz9w==", "01f8b061-f979-475b-97c1-6699ce520d92" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "AspNetUsers");

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
        }
    }
}
