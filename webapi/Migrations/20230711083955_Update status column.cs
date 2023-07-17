using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Updatestatuscolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Ingredients",
                newName: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_StatusId",
                table: "Ingredients",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Statuses_StatusId",
                table: "Ingredients",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Statuses_StatusId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_StatusId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Ingredients",
                newName: "Status");

        }
    }
}
