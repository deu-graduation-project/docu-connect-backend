using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "AgencyProductId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "KopyaSayısı",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderState",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SayfaSayısı",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "5fe321e8-a910-4231-a879-76a6523e37fe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "c900c21e-142b-425c-89ef-4c439cdef401");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "82b13b11-5a53-42bc-a9ba-ff2e43345608");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dc41c705-e80f-4526-9ea6-aaf9f10bddda", "AQAAAAIAAYagAAAAEPGCBe1L5QiymvcOuVDiLnbrvyzur5m8AP7yEnNv8ASTefWe6WQGOMialK3Nz9TJjg==", "607e0e92-9dc1-44ee-9ee8-e5bb53c0cb68" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AgencyProductId",
                table: "Orders",
                column: "AgencyProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AgencyProducts_AgencyProductId",
                table: "Orders",
                column: "AgencyProductId",
                principalTable: "AgencyProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AgencyProducts_AgencyProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AgencyProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AgencyProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "KopyaSayısı",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderState",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SayfaSayısı",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "128f0e53-f259-411a-b4be-e050e48c199e",
                column: "ConcurrencyStamp",
                value: "f542fbc3-6c5e-4426-9655-99d86397fbee");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                column: "ConcurrencyStamp",
                value: "4cd2a75d-bd28-4515-a1ec-e6ef18bf761f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                column: "ConcurrencyStamp",
                value: "57ffa01d-29b6-4131-a1ed-3cb01cae74c1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5accbc71-441e-4522-9eb9-daa60b355cd8", "AQAAAAIAAYagAAAAEBC13kh32C5Z62ARhXGQm5VslBH4vOaRgYk2/JLStFG6wWW9ZaHgIS5BRYC4kB61jA==", "43305137-d3bf-4afd-a25f-5e470ced4231" });
        }
    }
}
