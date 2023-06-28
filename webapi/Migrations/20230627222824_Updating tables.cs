using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Updatingtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Photo_PhotoId",
                table: "Drinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.InsertData(
                table: "DrinkTypes",
                columns: new[] { "DrinkId", "TypeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 4 },
                    { 2, 1 },
                    { 2, 5 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Photos_PhotoId",
                table: "Drinks",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Photos_PhotoId",
                table: "Drinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DeleteData(
                table: "DrinkTypes",
                keyColumns: new[] { "DrinkId", "TypeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "DrinkTypes",
                keyColumns: new[] { "DrinkId", "TypeId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "DrinkTypes",
                keyColumns: new[] { "DrinkId", "TypeId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "DrinkTypes",
                keyColumns: new[] { "DrinkId", "TypeId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Photo_PhotoId",
                table: "Drinks",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
